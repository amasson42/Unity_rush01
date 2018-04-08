using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoostEntity : SpellEntity {
	
	float duration;

	void Start () {
		caster.unit.factorDamages *= 1.1f * skillLevel;
		duration = 60.0f + 30.0f * skillLevel;
	}
	
	// Update is called once per frame
	void Update () {
		if (caster == null)
			Destroy(this);
		transform.position = caster.transform.position;
		duration -= Time.deltaTime;
		if (duration <= 0.0f) {
			caster.unit.factorDamages /= 1.1f * skillLevel;
			Destroy(this);
		}
	}
}
