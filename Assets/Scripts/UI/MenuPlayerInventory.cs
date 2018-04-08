using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuInterface))]
public class MenuPlayerInventory : MonoBehaviour {


	public Actor player;

	private MenuInterface menu;
	private Text description;

	void Start ()
	{
		menu = GetComponent<MenuInterface>();
		description = transform.Find("Menu/TabDescription/Text").GetComponent<Text>();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.I))
			menu.Toggle();
		if (!player || !menu.Visible())
			return ;
		description.text = "test";
	}
}