using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour {

	public Animator animator;
	public Transform weaponTransformSlot;
	public NavMeshAgent agent;
	public Unit unit;
	public List<SpellCaster> spells;

	[HideInInspector] public Actor currentTarget = null;

 	void Start() {
		if (animator == null)
			animator = GetComponent<Animator>();
		if (agent == null)
			agent = GetComponent<NavMeshAgent>();
		if (unit == null)
			unit = GetComponent<Unit>();
	}
	
	void Update() {
		if (agent) {
			if (unit && unit.isAlive) {
				if (currentTarget && currentTarget.unit && currentTarget.unit.isAlive) {
					agent.SetDestination(currentTarget.transform.position);
					agent.stoppingDistance = agent.radius + unit.weaponAttackRange + currentTarget.agent.radius;
					TryAttackTarget();
				} else {
					currentTarget = null;
				}
			}
			animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
		}
	}

	void TryAttackTarget() {
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
		if (unit && !unit.isAlive)
			return ;
		if (agent) {
			agent.SetDestination(destination);
			agent.stoppingDistance = 0.0f;
		}
		currentTarget = null;
	}

	public void OrderAttackTarget(Actor target) {
		if (unit && !unit.isAlive)
			return ;
		currentTarget = target;
	}

	public bool OrderUseSpell(int spellIndex, Vector3 targetPoint, Actor targetActor, out string error) {
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

	public void PlayDeadAnimation() {
		animator.SetTrigger("Die");
	}

	public void RemoveFromGame(float deathTime) {
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
