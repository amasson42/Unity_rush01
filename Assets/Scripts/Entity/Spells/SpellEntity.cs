using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellEntity : MonoBehaviour {

	public Actor caster;
	public Actor actorTarget;
	public Vector3 pointTarget;
	public int skillLevel = 1;

	public virtual bool CanBeCastedBy(SpellCaster caster, out string error) {
		// childs should check here if there is a valid target or position point close enough
		error = "valid target";
		return true;
	}
}
