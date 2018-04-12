using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenStrike_meteor : SpellEntity {

	public Transform modelTransform;
	public float durationTime;

	// Use this for initialization
	void Start () {
		modelTransform.localScale = new Vector3(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));
		durationTime = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		durationTime -= Time.deltaTime;
		if (durationTime <= 1.0f)
			transform.localScale = new Vector3(durationTime, durationTime, durationTime);
		if (durationTime <= 0.0f)
			Destroy(gameObject);
	}

	void OnTriggerEnter(Collider collider) {
		Actor colactor = collider.gameObject.GetComponent<Actor>();
		if (colactor)
			ApplyEffect(colactor);
	}

	void ApplyEffect(Actor target) {
		if (target.unit && caster && caster.unit)
			if (target.unit.team != caster.unit.team) {
				target.unit.TakeDamages(caster.unit.currentSpellDamages * skillLevel * 20.0f, caster.unit);
			}
	}

}
