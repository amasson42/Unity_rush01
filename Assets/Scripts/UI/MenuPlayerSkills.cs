using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MenuInterface))]
public class MenuPlayerSkills : MonoBehaviour {


	public Actor player;

	[SerializeField]
	private MenuSpellCase menuSpellCasePrefab;

	private MenuInterface menu;
	private Text txtDescription;
	private Text txtSpell;
	private Text txtStats;

	void Start ()
	{
		menu = GetComponent<MenuInterface>();
		txtDescription = transform.Find("Menu/TabDescription/Text").GetComponent<Text>();
		txtSpell = transform.Find("Menu/TabSpell/Text").GetComponent<Text>();
		txtStats = transform.Find("Menu/TabStats/Text").GetComponent<Text>();

		
		txtSpell.text = "";
		txtDescription.text = "";
		foreach (SpellCaster cast in player.spells)
		{
			txtSpell.text += cast.name + ": mana cost(" + cast.manaCost + ")\n";
			txtDescription.text += cast.info + "\n";
		}
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.N))
			menu.Toggle();
		if (!player || !menu.Visible())
			return ;
	}
}