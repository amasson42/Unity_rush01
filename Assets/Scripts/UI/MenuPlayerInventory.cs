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
	private Image border;
	public List<MenuInventoryCase> list = new List<MenuInventoryCase>();
	public MenuInventoryCase equip;

	void Awake() {
		instance = this;
	}



	void Start ()
	{
		menu = GetComponent<MenuInterface>();
		description = transform.Find("Menu/TabDescription/Text").GetComponent<Text>();
		border = transform.Find("Menu/TabDescription/Border").GetComponent<Image>();
		for (int i = 0; i < list.Count; ++i)
		{
			list[i].caseId = i;
			list[i].inv = this;
			MenuHoverEvent hove = list[i].gameObject.AddComponent<MenuHoverEvent>();
			hove.OnHoverEnterEvent += OnHoverEnter;
			hove.OnHoverExitEvent += OnHoverExit;
		}
		equip.inv = this;
		MenuHoverEvent hover = equip.gameObject.AddComponent<MenuHoverEvent>();
		hover.OnHoverEnterEvent += OnHoverEnter;
		hover.OnHoverExitEvent += OnHoverExit;
	}
	
	void UpdateInv()
	{
		int i;
		for (i = 0; i < list.Count && i < player.items.Count; ++i)
			list[i].LoadItem(player.items[i]);
		for (; i < list.Count; ++i)
			list[i].LoadItem(null);
		equip.LoadItem(player.weaponSlot);
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
	}

	void SetDescription(ItemInventory ca)
	{
		if (!ca)
		{
			description.text = "";
			border.enabled = false;
		}
		else
		{
			description.text = "<size=20>" + ca.weaponName + "</size>" +
				"\nLevel : " + ca.level +
				"\nDamage : " + (long)ca.minDamage + "-" + (long)ca.maxDamage +
				"\nAttackSpeed : " + ca.attackSpeed +
				"\nDps : " + (long)((ca.minDamage + ca.maxDamage) / ca.attackSpeed * 0.5f);
			border.color = ItemInventory.RarityColors[(int)ca.rarity];
			border.enabled = true;
		}
	}

	void OnHoverEnter(GameObject obj)
	{
		MenuInventoryCase sp = obj.GetComponent<MenuInventoryCase>();
		SetDescription(sp ? sp.item : null);
	}
	void OnHoverExit(GameObject obj)
	{
		SetDescription(null);
	}
}