using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecVortex : SpellEntity {

	Vector3 orbScaler;
	float delay = 1.0f;
	bool delayPassed = false;
	bool dead = false;

	List<Actor> grabeds;

	// Use this for initialization
	void Start() {
		caster.transform.LookAt(pointTarget);
		caster.animator.SetTrigger("WeaponRaise");
		caster.OrderStopMove();
		transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
		float scaler = 1.0f + skillLevel * 0.2f;
		orbScaler = new Vector3(scaler, scaler, scaler);
		grabeds = new List<Actor>();
	}
	
	// Update is called once per frame
	void Update() {
		delay -= Time.deltaTime;
		if (!delayPassed) {
			transform.position = Vector3.MoveTowards(transform.position, pointTarget, 5.0f * Time.deltaTime);
			if (delay <= 0.0f) {
				delayPassed = true;
				delay = 5.0f;
			}
		} else if (!dead) {
			transform.position = Vector3.MoveTowards(transform.position, pointTarget, 8.0f * Time.deltaTime);
			transform.localScale = Vector3.MoveTowards(transform.localScale, orbScaler, 8.0f * Time.deltaTime);
			foreach (var grab in grabeds) {
				if (grab) {
					grab.transform.position = Vector3.MoveTowards(grab.transform.position, pointTarget, 2.0f * Time.deltaTime);
					if (grab.unit && grab.unit.isAlive)
						grab.unit.TakeDamages(30.0f * Time.deltaTime * caster.unit.currentSpellDamages, caster.unit);
				}
			}
			if (delay <= 0.0f)
			{
				for (int i = grabeds.Count - 1; i >= 0; i--) {
					FreeActor(grabeds[i]);
				}
				Destroy(gameObject, 1.0f);
				dead = true;
			}
		} else {
			transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
		}
	}

	void OnTriggerEnter(Collider collider) {
		Actor colactor = collider.gameObject.GetComponent<Actor>();
		if (colactor) {
			if (caster.unit && caster.unit.team != colactor.unit.team) {
				grabeds.Add(colactor);
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		Actor colactor = collider.gameObject.GetComponent<Actor>();
		if (colactor)
			FreeActor(colactor);
	}

	void FreeActor(Actor actor) {
		if (actor && grabeds.Contains(actor)) {
			grabeds.Remove(actor);
		}
	}

	public override bool CanBeCastedBy(SpellCaster spellCaster, out string error) {

		float distance = Vector3.Distance(spellCaster.caster.transform.position, spellCaster.pointTarget);

		if (distance > 12.0f) {
			error = "target too far";
			return false;
		}
		if (distance < 4.0f) {
			error = "target to close";
		}

		error = "";
		return true;
	}
}
