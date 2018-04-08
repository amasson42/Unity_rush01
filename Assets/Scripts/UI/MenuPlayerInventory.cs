using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuInterface))]
public class MenuPlayerInventory : MonoBehaviour {


	public static MenuPlayerInventory instance;

	public Actor player;

	private MenuInterface menu;
	private Text description;
	// public static MenuInventoryCase[] list;
	public List<MenuInventoryCase> list = new List<MenuInventoryCase>();
	public MenuInventoryCase equip;

	void Awake() {
		instance = this;
	}

	void Start ()
	{
		menu = GetComponent<MenuInterface>();
		description = transform.Find("Menu/TabDescription/Text").GetComponent<Text>();
		for (int i = 0; i < list.Count; ++i)
			list[i].caseId = i++;
	}
	
	void UpdateInv()
	{
		for (int i = 0; i < list.Count; ++i)
			list[i].LoadItem(player.items[i]);
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.I))
			menu.Toggle();
		if (!player || !menu.Visible())
			return ;
		if (player.inventoryChanged)
		{
			UpdateInv();
			player.inventoryChanged = false;
		}
		description.text = "test";
	}
}