﻿using System.Collections;
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
	
	private bool onMove = false;
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z))//TODO for debug
			unit.LevelUp();
		if (MenuManager.OnInterface())
			return ;
		RaycastHit hit = GetClickedRaycast();
		if (Input.GetMouseButton(0))
		{
			Actor hitActor;
			ItemEntity item;
			if ((hitActor = hit.collider.GetComponent<Actor>()) && hitActor.unit && hitActor.unit.team != unit.team)
			{
				actor.OrderAttackTarget(hitActor);
				moveMarkNode.SetActive(false);
			}
			else if (Input.GetMouseButtonDown(0) && (item = hit.collider.GetComponent<ItemEntity>()))
			{
				actor.OrderLootItem(item);
				moveMarkNode.SetActive(false);
			}
			else if (onMove || Input.GetMouseButtonDown(0))
			{
				actor.OrderMoveToTarget(hit.point);
				moveMarkNode.transform.position = hit.point;
				moveMarkNode.SetActive(true);
				onMove = true;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			if (actor.currentTarget)
				actor.OrderAttackTarget(null);
			onMove = false;
		}
		followCamera.horizontalAngle += Input.GetAxis("Vertical");
		followCamera.verticalAngle += -Input.GetAxis("Horizontal");
		/*
		if (Input.GetKeyDown(KeyCode.Q)) {
			string error;
			if (!actor.OrderUseSpell(0, hit.point, hit.collider.GetComponent<Actor>(), out error))
				PutErrorText(error);
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			string error;
			if (!actor.OrderUseSpell(1, hit.point, hit.collider.GetComponent<Actor>(), out error))
				PutErrorText(error);
		}
		if (Input.GetKeyDown(KeyCode.E)) {
			string error;
			if (!actor.OrderUseSpell(2, hit.point, hit.collider.GetComponent<Actor>(), out error))
				PutErrorText(error);
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			string error;
			if (!actor.OrderUseSpell(3, hit.point, hit.collider.GetComponent<Actor>(), out error))
				PutErrorText(error);
		}*/
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
