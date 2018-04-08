using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour {

	public AudioClip[] attackingSound;
	public AudioClip[] footStepSounds;
	public Animator animator;
	public Transform weaponTransformSlot = null;
	public NavMeshAgent agent;
	public Unit unit;
	public List<SpellCaster> spells;
	[HideInInspector] public Loot[] loots;
	[HideInInspector] public Actor currentTarget = null;
	[HideInInspector] public ItemEntity currentTargetItem = null;
	[HideInInspector] public List<ItemInventory> items;
	public bool inventoryChanged = false;
	public ItemInventory weaponSlot = null;
	public int inventoryCapacity = 15;
	public bool moveRetained {get {return moveRetainer > 0;}}
	private int moveRetainer = 0;

	void Start() {
		if (animator == null)
			animator = GetComponent<Animator>();
		if (agent == null)
			agent = GetComponent<NavMeshAgent>();
		if (unit == null)
			unit = GetComponent<Unit>();
		loots = GetComponents<Loot>();
	}
	
	void Update() {
		if (agent == null)
			return ;
		if (unit && unit.isAlive && !moveRetained) {
			if (currentTarget && currentTarget.unit && currentTarget.unit.isAlive) {
				agent.SetDestination(currentTarget.transform.position);
				agent.stoppingDistance = agent.radius + unit.weaponAttackRange + currentTarget.agent.radius;
				TryAttackTarget();
			} else {
				currentTarget = null;
			}
			if (currentTargetItem && PathComplete()) {
				if (currentTargetItem.itemInstance.type == ItemInventory.Type.Weapon)
					AddWeapon(currentTargetItem.itemInstance);
				else
					AddInInventory(currentTargetItem.itemInstance);
				currentTargetItem = null;
			}
		}
		animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
	}

	public void RetainMove(bool stopMove = true) {
		moveRetainer++;
		if (stopMove) {
			Debug.Log("move stopped");
			agent.SetDestination(transform.position);
		}
	}

	public void ReleaseMove() {
		moveRetainer--;
	}

	void TryAttackTarget() {
		if (moveRetained)
			return ;
		if (IsAtAttackRange(currentTarget)) {
			transform.LookAt(currentTarget.transform);
			if (unit.attackCooldown <= 0.0f) {
				unit.ResetAttackCooldown();
				animator.SetTrigger("Attack" + unit.weaponAttackAnimation);
				if (attackingSound.Length > 0)
					AudioSource.PlayClipAtPoint(attackingSound[Random.Range(0, attackingSound.Length)], transform.position);
			}
		}
	}

	public void PlayFoodStep() {
		if (footStepSounds.Length > 0)
			AudioSource.PlayClipAtPoint(footStepSounds[Random.Range(0, footStepSounds.Length)], transform.position);
	}

	public void AnimationAttackPointEvent() {
		if (currentTarget)
			unit.AutoAttack(currentTarget.unit);
	}

	public bool IsAtAttackRange(Actor target) {
		if (agent == null || target.agent == null)
			return false;
		float range = agent.radius + unit.weaponAttackRange + target.agent.radius;
		return (transform.position - target.transform.position).sqrMagnitude <= range * range;
	}

	public void OrderStopMove() {
		if (agent) {
			agent.SetDestination(transform.position);
		}
		currentTarget = null;
	}

	public void OrderMoveToTarget(Vector3 destination) {
		if (moveRetained || (unit && !unit.isAlive))
			return ;
		if (agent) {
			agent.SetDestination(destination);
			agent.stoppingDistance = 0.0f;
		}
		currentTarget = null;
		currentTargetItem = null;
	}

	public void OrderAttackTarget(Actor target) {
		if (moveRetained || (unit && !unit.isAlive))
			return ;
		currentTarget = target;
		currentTargetItem = null;
	}

	public bool OrderUseSpell(int spellIndex, Vector3 targetPoint, Actor targetActor, out string error) {
		if (moveRetained) {
			error = "you can't attack right now";
			return false;
		}
		if (unit && !unit.isAlive) {
			error = "can't attack when you're dead... noob";
			return false;
		}
		if (spellIndex >= spells.Count) {
			error = "spell doesn't exist";
			return false;
		}
		SpellCaster sc = spells[spellIndex];
		sc.actorTarget = targetActor;
		sc.pointTarget = targetPoint;
		return sc.TryCast(out error);
	}

	public void OrderLootItem(ItemEntity item) {
		if (moveRetained || (unit && !unit.isAlive))
			return ;
		if (agent) {
			agent.SetDestination(item.transform.position);
			agent.stoppingDistance = 0.0f;
		}
		currentTargetItem = item;
		currentTarget = null;
	}

	public bool PathComplete()
    {
		return (agent.remainingDistance == 0 && agent.pathStatus == NavMeshPathStatus.PathComplete);
    }

	public void PlayDeadAnimation() {
		animator.SetTrigger("Die");
	}

	public void AddInInventory(ItemInventory item, bool hasLimits = true)
	{
		if (!hasLimits || items.Count < inventoryCapacity)
		{
			items.Add(item);
			item.entityInstance.gameObject.SetActive(false);
			inventoryChanged = true;
		}
		else
			item.entityInstance.transform.position = new Vector3(item.entityInstance.transform.position.x, item.entityInstance.transform.position.y + 0.5f, item.entityInstance.transform.position.z);
	}					

	public void AddWeapon(ItemInventory weapon)
	{
		if (!weapon)
			return;
		if (!weaponSlot && weapon.type == ItemInventory.Type.Weapon)
		{
			weaponSlot = weapon;
			weapon.entityInstance.gameObject.SetActive(true);
			weapon.entityInstance.GetComponent<Rigidbody>().isKinematic = true;
			weapon.entityInstance.GetComponent<BoxCollider>().enabled = false;
			weapon.entityInstance.transform.parent = weaponTransformSlot;
			weapon.entityInstance.transform.localPosition = Vector3.zero;
			weapon.entityInstance.transform.localEulerAngles = Vector3.zero;

			//Add Stats to Unit
			unit.weaponAttackMin = weaponSlot.minDamage;
			unit.weaponAttackMax = weaponSlot.maxDamage;
			unit.weaponAttackPeriod = weaponSlot.attackSpeed;
			inventoryChanged = true;
		}
		else if (items.Count < inventoryCapacity)
			AddInInventory(weapon);
		else if (weapon.entityInstance.gameObject.activeSelf)
			weapon.entityInstance.transform.position = new Vector3(weapon.entityInstance.transform.position.x, weapon.entityInstance.transform.position.y + 0.5f, weapon.entityInstance.transform.position.z);
	}

	public void SwapInventoryToWeaponSlot(ItemInventory item)
	{
		if (weaponSlot)
			RemoveWeapon(true, false);
		RemoveFromInventory(item, true);
	}

	public void RemoveFromInventory(ItemInventory item, bool toWeaponSlot = false)
	{
		if (items.Remove(item))
		{
			item.entityInstance.gameObject.SetActive(true);
			item.entityInstance.transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + 2f, transform.position.z + Random.Range(-1f, 1f));
		}
		if (toWeaponSlot)
			AddWeapon(item);
		inventoryChanged = true;
	}

	public void RemoveWeapon(bool toInventory = false, bool hasLimits = true)
	{
		if (toInventory)
			AddInInventory(weaponSlot, hasLimits);
		weaponTransformSlot.DetachChildren();
		weaponSlot.entityInstance.GetComponent<Rigidbody>().isKinematic = false;
		weaponSlot.entityInstance.GetComponent<BoxCollider>().enabled = true;
		weaponSlot.entityInstance.transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + 2f, transform.position.z + Random.Range(-1f, 1f));
		weaponSlot.entityInstance.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
		weaponSlot = null;
		inventoryChanged = true;
	}

	public void RemoveFromGame(float deathTime) {
		foreach(Loot loot in loots)
			loot.generateLoot(unit.level);
		Destroy(GetComponent<Collider>());
		Destroy(agent);
		Destroy(gameObject, deathTime);
		StartCoroutine(Deeping(deathTime));
	}

	private IEnumerator Deeping(float deathTime) {
		for (float f = 0.0f; f < deathTime; f += 0.05f) {
			if (f > 2.0f)
				transform.Translate(0, -0.02f, 0);
			yield return new WaitForSeconds(0.05f);
		}
	}
}