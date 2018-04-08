using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : SpellEntity {

	float delay = 0.4f;
	bool delayPassed = false;
	bool dead = false;

	// Use this for initialization
	void Start() {
		Vector3 direction = pointTarget - transform.position;
		direction.y = 0;
		caster.transform.LookAt(pointTarget);
		direction.Normalize();
		pointTarget = transform.position + direction * 8.0f;
		caster.animator.SetTrigger("Attack1");
		caster.OrderStopMove();
		caster.RetainMove();
	}
	
	// Update is called once per frame
	void Update() {
		if (!delayPassed) {
			delay -= Time.deltaTime;
			if (delay <= 0.0f) {
				delayPassed = true;
				caster.ReleaseMove();
			}
		} else if (!dead) {
			transform.position = Vector3.MoveTowards(transform.position, pointTarget, 8.0f * Time.deltaTime);
			if ((transform.position - pointTarget).sqrMagnitude < 0.1f)
			{
				Destroy(gameObject, 1.0f);
				dead = true;
			}
		} else {
			transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider collider) {
		Actor colactor = collider.gameObject.GetComponent<Actor>();
		if (colactor)
			ApplyEffect(colactor);
	}

	void ApplyEffect(Actor target) {
		if (target.unit) {
			if ((caster.unit && target.unit.team != caster.unit.team) || caster.unit == null)
				target.unit.TakeDamages(caster.unit.currentSpellDamages * skillLevel * 25.0f, caster.unit);
		}
	}

	public override bool CanBeCastedBy(SpellCaster spellCaster, out string error) {
		// childs should check here if there is a valid target or position point close enough
		error = "";
		return true;
	}
}
