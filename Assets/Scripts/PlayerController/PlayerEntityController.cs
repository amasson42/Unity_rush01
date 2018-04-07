using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerEntityController : MonoBehaviour {

	public FollowCamera followCamera;
	public Text errorActionText;

	public GameObject moveMarkNode;

	private Actor actor;
	private Unit unit;
	private float lastErrorText;

	// Use this for initialization
	void Start () {
		actor = GetComponent<Actor>();
		unit = GetComponent<Unit>();
		followCamera.visualTarget = actor.gameObject;
		errorActionText.text = "";
		lastErrorText = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (MenuManager.OnInterface())
			return ;
		if (Input.GetMouseButtonDown(0)) {
			var hit = GetClickedRaycast();
			Actor hitActor = hit.collider.GetComponent<Actor>();
			if (hitActor && hitActor.unit && hitActor.unit.team != unit.team) {
				actor.OrderAttackTarget(hitActor);
			} else {
				actor.OrderMoveToTarget(hit.point);
				moveMarkNode.transform.position = hit.point;
				moveMarkNode.SetActive(true);
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			if (actor.currentTarget)
				actor.OrderAttackTarget(null);
		}
		followCamera.horizontalAngle += Input.GetAxis("Vertical");
		followCamera.verticalAngle += -Input.GetAxis("Horizontal");
		if (Input.GetKeyDown(KeyCode.Q)) {
			var hit = GetClickedRaycast();
			string error;
			if (!actor.OrderUseSpell(0, hit.point, hit.collider.GetComponent<Actor>(), out error))
				PutErrorText(error);
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			var hit = GetClickedRaycast();
			string error;
			if (!actor.OrderUseSpell(1, hit.point, hit.collider.GetComponent<Actor>(), out error))
				PutErrorText(error);
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			var hit = GetClickedRaycast();
			string error;
			if (!actor.OrderUseSpell(2, hit.point, hit.collider.GetComponent<Actor>(), out error))
				PutErrorText(error);
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			var hit = GetClickedRaycast();
			string error;
			if (!actor.OrderUseSpell(3, hit.point, hit.collider.GetComponent<Actor>(), out error))
				PutErrorText(error);
		}
		if (Time.time - lastErrorText > 3.0f)
			errorActionText.text = "";
	}

	RaycastHit GetClickedRaycast() {
		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		Physics.Raycast(cameraRay, out hit);
		return hit;
	}

	void PutErrorText(string text) {
		errorActionText.text = text;
		lastErrorText = Time.time;
	}
}
