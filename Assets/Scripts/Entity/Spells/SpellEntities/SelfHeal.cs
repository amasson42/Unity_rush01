using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfHeal : SpellEntity {

	// Use this for initialization
	void Start () {
		if (caster.unit)
			caster.unit.TakeDamages(-50.0f * caster.unit.currentSpellDamages, caster.unit);
	}
	
	// Update is called once per frame
	void Update () {
		Destroy(this, 1.0f);
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
