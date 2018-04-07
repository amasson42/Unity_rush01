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

	static bool _rayOk = false;
	static RaycastHit _hitOk;
	public static RaycastHit GetClickedRaycast()
	{
		if (!_rayOk)
		{
			Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(cameraRay, out _hitOk);
			_rayOk = true;
		}
		return _hitOk;
	}
	public static T GetClickedType<T>()
	{
		Collider col = GetClickedRaycast().collider;
		return (col ? col.GetComponent<T>() : default(T));
	}

	void Start () {
		actor = GetComponent<Actor>();
		unit = GetComponent<Unit>();
		followCamera.visualTarget = actor.gameObject;
		errorActionText.text = "";
		lastErrorText = Time.time;
	}
	
	void Update () {
		if (MenuManager.OnInterface())
			return ;
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit = GetClickedRaycast();
			Actor hitActor;
			ItemEntity item;
			if ((hitActor = hit.collider.GetComponent<Actor>()) && hitActor.unit && hitActor.unit.team != unit.team)
			{
				actor.OrderAttackTarget(hitActor);
			}
			else if ((item = hit.collider.GetComponent<ItemEntity>())) 
				actor.OrderLootItem(item);
			}
			else
			{
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

	void LateUpdate()
	{
		_rayOk = false;
	}

	void PutErrorText(string text) {
		errorActionText.text = text;
		lastErrorText = Time.time;
	}
}
