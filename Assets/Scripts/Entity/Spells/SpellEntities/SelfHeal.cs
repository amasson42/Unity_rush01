using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfHeal : SpellEntity {

	// Use this for initialization
	void Start () {
		if (caster.unit)
			caster.unit.TakeDamages(-15.0f * caster.unit.currentSpellDamages, caster.unit);
		Destroy(this, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = caster.transform.position;
		if (caster.unit.isAlive)
			caster.unit.TakeDamages(-10.0f * caster.unit.currentSpellDamages * Time.deltaTime, caster.unit);
		else
			Destroy(this);
	}

	public override bool CanBeCastedBy(SpellCaster caster, out string error) {
		if (caster.caster.unit && caster.caster.unit.currentHealth >= caster.caster.unit.maxHealth) {
			error = "life is already full";
			return false;
		}
		error = "";
		return true;
	}
}
