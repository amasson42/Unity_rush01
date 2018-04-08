using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : SpellEntity {

	void Start() {
		caster.agent.speed += 12.0f;
		caster.OrderAttackTarget(actorTarget);
	}
	
	// Update is called once per frame
	void Update() {
		if (caster.IsAtAttackRange(actorTarget) || caster.currentTarget != actorTarget) {
			caster.unit.attackCooldown = 0.0f;
			caster.agent.speed -= 12.0f;
			Destroy(gameObject);
		}
	}
	
	public override bool CanBeCastedBy(SpellCaster spellCaster, out string error) {
		if (spellCaster.actorTarget == null) {
			error = "must target a unit";
			return false;
		}
		if (spellCaster.actorTarget.unit && spellCaster.actorTarget.unit.team == spellCaster.caster.unit.team) {
			error = "can't target allies";
			return false;
		}
		if (Vector3.Distance(spellCaster.caster.transform.position, spellCaster.actorTarget.transform.position) > 8.0f + 2.0f * skillLevel) {
			error = "target too far";
			return false;
		}
		error = "";
		return true;
	}
}