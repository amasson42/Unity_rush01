using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MenuSpellCase : MonoBehaviour {



	public KeyCode actionKey;
	private Image img;
	private SpellCaster spell = null;


	void Awake()
	{
		img = GetComponent<Image>();
	}
	void Start()
	{
	}
	






	public void LoadSpell(SpellCaster spellCaster)
	{
		spell = spellCaster;
		if (!spell)
		{
			img.overrideSprite = null;
		}
		else
		{
			img.overrideSprite = spell.icon;
		}
	}



	void Update()
	{
		string error;
		if (spell && Input.GetKey(actionKey))
			spell.TryCast(out error);

	}
}
