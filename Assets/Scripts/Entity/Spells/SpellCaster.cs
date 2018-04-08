using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour {

	public Actor caster;
	public float manaCost;
	public float cooldown;
	public float currentCooldown;
	public string name;
	public string info;
	public Sprite icon;

	[HideInInspector] public Actor actorTarget;
	[HideInInspector] public Vector3 pointTarget;
	public SpellEntity spellEntityPrefab; // this prefab contains only a spell scripts child of spellEntity

	void Start () {
		
	}
	
	void Update () {
		if (currentCooldown > 0)
			currentCooldown -= Time.deltaTime;
	}

	public bool TryCast(out string error) {
		if (spellEntityPrefab == null) {
			error = "the spell is empty";
			return false;
		}
		if (currentCooldown > 0.0f) {
			error = "not charged yet";
			return false;
		}
		if (caster && caster.unit)
			if (!caster.unit.CanPayCostForSpell(this, out error))
				return false;
		spellEntityPrefab.skillLevel = 3;
		if (!spellEntityPrefab.CanBeCastedBy(this, out error))
			return false;
		Cast();
		error = "casted";
		return true;
	}

	private void Cast() {
		if (caster && caster.unit)
			caster.unit.PayManaCost(this);
		currentCooldown = cooldown;
		GameObject spellObject = GameObject.Instantiate(spellEntityPrefab.gameObject, caster ? caster.transform.position : pointTarget, Quaternion.identity);
		SpellEntity spell = spellObject.GetComponent<SpellEntity>();
		spell.caster = caster;
		spell.actorTarget = actorTarget;
		spell.pointTarget = pointTarget;
	}
	public bool CanUse()
	{
		string error;
		return (caster.unit && caster.unit.CanPayCostForSpell(this, out error));
	}
}
