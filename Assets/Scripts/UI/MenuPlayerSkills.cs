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
	private Text txtStats;
	private GridLayoutGroup align;

	private Transform tabSpell;


	void SetDescription(SpellCaster caster)
	{
		if (!caster)
		{
			txtDescription.text = "";
		}
		else
		{
			txtDescription.text = "<size=20>" + caster.name + "</size>\n" +
				caster.info;
		}
	}

	void OnHoverEnter(GameObject obj)
	{
		MenuSpellCase sp = obj.GetComponent<MenuSpellCase>();
		SetDescription(sp ? sp.spell : null);
	}
	void OnHoverExit(GameObject obj)
	{
		SetDescription(null);
	}

	void Start ()
	{
		menu = GetComponent<MenuInterface>();
		txtDescription = transform.Find("Menu/TabDescription/Text").GetComponent<Text>();
		txtStats = transform.Find("Menu/TabStats/Text").GetComponent<Text>();
		tabSpell = transform.Find("Menu/TabSpell");
		align = tabSpell.GetComponent<GridLayoutGroup>();

		
		txtDescription.text = "";
		foreach (SpellCaster cast in player.spells)
		{
			GameObject obj = Instantiate(menuSpellCasePrefab.gameObject, Vector3.zero, Quaternion.identity,
				tabSpell);
			if (obj)
			{
				MenuSpellCase spell;
				if (spell = obj.GetComponent<MenuSpellCase>())
				{
					spell.LoadSpell(cast);
					spell.locked = true;
				}
				MenuHoverEvent hover = obj.AddComponent<MenuHoverEvent>();
				hover.OnHoverEnterEvent += OnHoverEnter;
				hover.OnHoverExitEvent += OnHoverExit;
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(tabSpell.GetComponent<RectTransform>());
		align.enabled = false;
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.N))
			menu.Toggle();
		if (!player || !menu.Visible())
			return ;
	}
}