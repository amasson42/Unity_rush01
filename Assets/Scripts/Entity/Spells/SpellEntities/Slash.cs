using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : SpellEntity {

	private float lifeTime;
	private List<Actor> hits;

	// Use this for initialization
	void Start () {
		lifeTime = 1.0f;
		caster.animator.SetTrigger("Attack2");
		hits = new List<Actor>();
		caster.OrderStopMove();
		caster.RetainMove();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = caster.transform.position;
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0.0f) {
			Destroy(gameObject);
			caster.ReleaseMove();
		}
		caster.transform.Rotate(new Vector3(0, -2 * 360.0f * Time.deltaTime, 0));
	}

	void OnTriggerEnter(Collider collider) {
		Actor colactor = collider.gameObject.GetComponent<Actor>();
		if (colactor)
			ApplyEffect(colactor);
	}

	void ApplyEffect(Actor target) {
		if (hits.Contains(target))
			return ;
		if (target.unit && caster && caster.unit)
			if (target.unit.team != caster.unit.team)
				caster.unit.AutoAttack(target.unit);
	}

	public override bool CanBeCastedBy(SpellCaster caster, out string error) {
		error = "valid target";
		return true;
	}
}
