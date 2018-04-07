using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayerInfo : MonoBehaviour {

	public Unit player;

	private Text xpTxt;
	private MenuBar life;
	private MenuBar mana;
	private MenuBar xp;


	void Start ()
	{
		life = transform.Find("LifeBar").GetComponent<MenuBar>();
		mana = transform.Find("ManaBar").GetComponent<MenuBar>();
		xp = transform.Find("XpBar").GetComponent<MenuBar>();
		xpTxt = transform.Find("TxtXpVal").GetComponent<Text>();
	}
	
	void Update ()
	{
		if (!player)
			return ;
		life.SetVal("Life : ", player.currentHealth, 0f, player.maxHealth);
		mana.SetVal("Mana : ", player.currentMana, 0f, player.maxMana);
		xp.SetVal("", player.currentExperience, 0f, player.requiredExperience);
		xpTxt.text = player.currentExperience + "/" + player.requiredExperience;
	}
}
