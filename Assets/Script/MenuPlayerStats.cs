using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayerStats : MonoBehaviour {


	public Unit player;

	private GameObject menu;
	private Text txt;
	private bool show = false;


	public void ButtonUpStrengh()
	{
		// player.AddStrengh(1);
	}
	public void ButtonUpAgility()
	{
		// player.AddAgility(1);
	}
	public void ButtonUpConstitution()
	{
		// player.AddConstitution(1);
	}
	public void ButtonHide()
	{
		show = false;
		menu.SetActive(false);
	}

	void Start ()
	{
		menu = transform.Find("MenuPlayerStats").gameObject;
		txt = transform.Find("MenuPlayerStats/Menu/TxtInfo").GetComponent<Text>();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			show = !show;
			menu.SetActive(show);
		}
		if (!player || !show)
			return ;
		txt.text = "temp";
		/*txt.text =
			"[Lv." + player.data._lvl + "]\n\n" + 
			player.data._strengh + "\n\n" + 
			player.data._agility + "\n\n" + 
			player.data._constitution + "\n\n" + 
			player.data._armor + "\n\n" + 
			player.data._point + "\n\n\n\n" + 
			player.data._damagePhysMin + "-" + player.data._damagePhysMax + "\n\n" + 
			player.data._attackSpeed + "\n\n" + 
			player.data._healthMax + "\n\n" + 
			player.data._xp + "\n\n" + 
			player.data._xpLvl + "\n\n" + 
			player.data._credit;*/
	}
}
