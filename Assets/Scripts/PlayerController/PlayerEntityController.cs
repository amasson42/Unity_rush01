using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerEntityController : MonoBehaviour {

	public FollowCamera followCamera;
	// public Text errorActionText;

	public GameObject moveMarkNode;

	private Actor actor;
	private Unit unit;
	// private float lastErrorText;

	// sounds
	public AudioSource ambiantMusicPlayer;
	public AudioSource fightMusicPlayer;

	public static PlayerEntityController instance;

	public int fighters = 0;

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

	void Awake()
	{
		instance = this;	
		actor = GetComponent<Actor>();
		unit = GetComponent<Unit>();
	}

	void Start () {
		followCamera.visualTarget = actor.gameObject;
		// errorActionText.text = "";
		// lastErrorText = Time.time;
	}
	

	public bool UseSpell(SpellCaster sc) {
		if (actor.unit && !actor.unit.isAlive) {
			// error = "can't attack when you're dead... noob";
			return false;
		}
		RaycastHit hit = PlayerEntityController.GetClickedRaycast();
		sc.actorTarget = hit.transform.GetComponent<Actor>();
		sc.pointTarget = hit.point;
		string error;
		return sc.TryCast(out error);
	}

	private bool onMove = false;
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z))//TODO for debug
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				for (int i = 0; i < 10; ++i)
					unit.LevelUp();
			}
			else
				unit.LevelUp();
		}
		
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
		if (actor.PathComplete())
			moveMarkNode.SetActive(false);
		followCamera.horizontalAngle += Input.GetAxis("Vertical");
		followCamera.verticalAngle += -Input.GetAxis("Horizontal");
		// if (Time.time - lastErrorText > 3.0f)
			// errorActionText.text = "";
	}

	void LateUpdate()
	{
		_rayOk = false;
	}

	void PutErrorText(string text) {
		// errorActionText.text = text;
		// lastErrorText = Time.time;
	}
}
