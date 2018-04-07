using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventSystem))]
public class MenuManager : MonoBehaviour {


	static private MenuManager instance;

	private EventSystem eventSystem;

	void Awake()
	{
		instance = this;
		eventSystem = GetComponent<EventSystem>();
	}

	public static bool OnInterface()
	{
		return (instance.eventSystem.IsPointerOverGameObject());
	}
}
