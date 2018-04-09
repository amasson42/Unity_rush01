using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostNova : SpellEntity {

	public GameObject iceBlockPrefab;
	float duration;
	Vector3 targetScale;
	List<Actor> frosted;
	List<GameObject> iceBlocks;
	
	void Start () {
		duration = 5.0f;
		transform.localScale = Vector3.zero;
		float scaler = 1.0f + skillLevel * 0.2f;
		targetScale = new Vector3(scaler, scaler, scaler);
		frosted = new List<Actor>();
		iceBlocks = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		duration -= Time.deltaTime;
		if (duration > 2.0f) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, 4.0f * Time.deltaTime);
		} else if (duration > 1.0f) {
			Vector3 deeperPosition = transform.position;
			deeperPosition.y -= Time.deltaTime;
			transform.position = deeperPosition;
		} else if (duration <= 0.0f) {
			Debug.Log("destroy time !");
			for (int i = frosted.Count - 1; i >= 0; i--)
				FreeActor(frosted[i]);
			foreach (var block in iceBlocks)
				Destroy(block);
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider collider) {
		Actor colactor = collider.gameObject.GetComponent<Actor>();
		if (colactor) {
			if (caster.unit && caster.unit.team != colactor.unit.team) {
				iceBlocks.Add(GameObject.Instantiate(iceBlockPrefab, colactor.transform.position, colactor.transform.rotation));
				colactor.RetainMove();
				frosted.Add(colactor);
			}
		}
	}

	void FreeActor(Actor actor) {
		if (actor && frosted.Contains(actor)) {
			actor.ReleaseMove();
			frosted.Remove(actor);
		}
	}

	public override bool CanBeCastedBy(SpellCaster caster, out string error) {
		error = "valid target";
		return true;
	}
}
