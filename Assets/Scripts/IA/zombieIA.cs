using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class zombieIA : MonoBehaviour {

	private Actor actor;
	private Unit unit;

	// Use this for initialization
	void Start () {
		actor = GetComponentInParent<Actor>();
		unit = GetComponentInParent<Unit>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider collider) {
		Actor colliderActor = collider.gameObject.GetComponent<Actor>();
		if (colliderActor) {
			if (colliderActor.unit && unit && colliderActor.unit.team != unit.team) {
				actor.OrderAttackTarget(colliderActor);
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		Actor colliderActor = collider.gameObject.GetComponent<Actor>();
		if (colliderActor) {
			if (colliderActor == actor.currentTarget)
				actor.OrderAttackTarget(null);
		}
	}
}
