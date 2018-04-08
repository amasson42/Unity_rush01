using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class MenuSpellCase : MonoBehaviour, IDragHandler, IDropHandler {



	public KeyCode actionKey;
	private Image img;
	private SpellCaster spell = null;
	private Vector3 position;


	void Awake()
	{
		img = GetComponent<Image>();
		position = transform.position;
	}
	void Start()
	{
	}
	



    public void OnDrop(PointerEventData data)
	{
		transform.position = position;
	}
    public void OnDrag(PointerEventData data)
    {
		transform.position = data.position;
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
