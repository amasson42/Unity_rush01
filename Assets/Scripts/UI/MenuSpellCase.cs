using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class MenuSpellCase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {



	public KeyCode actionKey;

	private Image img;
	private CanvasGroup group;

	[SerializeField]
	private SpellCaster spell = null;
	private Vector3 position;


	void Awake()
	{
		img = GetComponent<Image>();
		group = GetComponent<CanvasGroup>();
		position = transform.position;
	}
	void Start()
	{
	}
	


	public void OnBeginDrag(PointerEventData data)
    {
		group.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData data)
    {
		transform.position = data.position;
    }
	public void OnEndDrag(PointerEventData data)
	{
		group.blocksRaycasts = true;
		transform.position = position;
		GameObject obj = data.pointerCurrentRaycast.gameObject;
		Debug.Log(obj);
		if (obj == null)
		{
			spell = null;
			img.color = new Color(1, 1, 1, 0.5f);
		}
	}
    public void OnDrop(PointerEventData data)
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
		if (spell)
		{
			img.color = spell.CanUse() ? Color.white : Color.red;
			if (Input.GetKey(actionKey))
			{
				PlayerEntityController.instance.UseSpell(spell);
				// spell.Use();
			}
		} 
	}
}
