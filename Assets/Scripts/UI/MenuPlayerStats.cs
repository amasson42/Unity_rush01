using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuInterface))]
public class MenuPlayerStats : MonoBehaviour {


	public Unit player;

	private MenuInterface menu;
	private Text txt;

	public void ButtonUpStrength()
	{
		if (player.availableStatsPoints > 0) {
			player.strength++;
			player.availableStatsPoints--;
		}
	}
	public void ButtonUpDexterity()
	{
		if (player.availableStatsPoints > 0) {
			player.dexterity++;
			player.availableStatsPoints--;
		}
	}
	public void ButtonUpVitality()
	{
		if (player.availableStatsPoints > 0) {
			player.vitality++;
			player.availableStatsPoints--;
		}
	}
	public void ButtonUpEnergy()
	{
		if (player.availableStatsPoints > 0) {
			player.energy++;
			player.availableStatsPoints--;
		}
	}

	void Start ()
	{
		menu = GetComponent<MenuInterface>();
		txt = transform.Find("Menu/TxtInfo").GetComponent<Text>();
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
			menu.Toggle();
		if (!player || !menu.Visible())
			return ;
		txt.text =
			"[Lv." + player.level + "]\n\n" + 
			player.strength + "\n\n" + 
			player.dexterity + "\n\n" + 
			player.vitality + "\n\n" + 
			player.energy + "\n\n" + 
			"nan" + "\n\n" +  //armor
			player.availableStatsPoints + "\n\n\n\n" + 
			// player.currentDamage + "\n\n" + 
			(long)player.weaponAttackMin + "-" + (long)player.weaponAttackMax + "\n\n" + 
			player.currentWeaponPeriod + "\n\n" + 
			(long)((player.weaponAttackMin + player.weaponAttackMax) / player.currentWeaponPeriod * 0.5f) + "\n\n" + 
			player.maxHealth + "\n\n" + 
			player.currentExperience + "\n\n" + 
			player.requiredExperience;
	}
}