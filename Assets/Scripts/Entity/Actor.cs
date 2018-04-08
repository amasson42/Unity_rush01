﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour {

	public Animator animator;
	public Transform weaponTransformSlot = null;
	public NavMeshAgent agent;
	public Unit unit;
	public List<SpellCaster> spells;
	[HideInInspector] public Loot[] loots;
	[HideInInspector] public Actor currentTarget = null;
	[HideInInspector] public ItemEntity currentTargetItem = null;
	[HideInInspector] public List<ItemInventory> items;
	public ItemInventory weaponSlot = null;
	public int inventoryCapacity = 12;
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
			if (currentTargetItem && pathComplete()) {
				if (weaponSlot && currentTargetItem.itemInstance.type == ItemInventory.Type.Weapon)
				{
					removeWeapon(false);
					addWeapon(currentTargetItem.itemInstance);
				}
				else if (!weaponSlot && currentTargetItem.itemInstance.type == ItemInventory.Type.Weapon)
					addWeapon(currentTargetItem.itemInstance);
				else if (items.Count < inventoryCapacity)
				{
					items.Add(currentTargetItem.itemInstance);
					currentTargetItem.gameObject.SetActive(false);
				}
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
			}
		}
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

	public bool pathComplete()
    {
		return (agent.remainingDistance == 0 && agent.pathStatus == NavMeshPathStatus.PathComplete);
    }

	public void PlayDeadAnimation() {
		animator.SetTrigger("Die");
	}

	public void addWeapon(ItemInventory weapon)
	{
		weaponSlot = weapon;
		weapon.entityInstance.gameObject.SetActive(true);
		weapon.entityInstance.GetComponent<Rigidbody>().isKinematic = true;
		weapon.entityInstance.GetComponent<BoxCollider>().enabled = false;
		weapon.entityInstance.transform.parent = weaponTransformSlot;
		weapon.entityInstance.transform.localPosition = Vector3.zero;
		weapon.entityInstance.transform.localEulerAngles = Vector3.zero;
	}

	public void removeWeapon(bool toInventory)
	{
		if (toInventory)
			weaponSlot.entityInstance.gameObject.SetActive(false);
		weaponTransformSlot.DetachChildren();
		weaponSlot.entityInstance.GetComponent<Rigidbody>().isKinematic = false;
		weaponSlot.entityInstance.GetComponent<BoxCollider>().enabled = true;
		weaponSlot.entityInstance.transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + 2f, transform.position.z + Random.Range(-1f, 1f));
		weaponSlot.entityInstance.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
		weaponSlot = null;
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
