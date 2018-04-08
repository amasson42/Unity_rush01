using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class MenuSpellCase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {



	public KeyCode actionKey;

	private Image img;
	private Image cooldown;
	private CanvasGroup group;

	public bool locked = false;
	public SpellCaster spell = null;
	private Vector3 position;
	private Transform parent;
	private int index;

	void Awake()
	{
		img = GetComponent<Image>();
		cooldown = transform.Find("Cooldown").GetComponent<Image>();
		group = GetComponent<CanvasGroup>();
	}
	void Start()
	{
		LoadSpell(spell);
	}


	public void OnBeginDrag(PointerEventData data)
    {
		if (!spell)
		{
			data.pointerDrag = null;
			return ;
		}
		parent = transform.parent;
		index = transform.GetSiblingIndex();
		transform.SetParent(transform.root);
		position = transform.position;
		group.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData data)
    {
		transform.position = data.position;
    }
	public void OnEndDrag(PointerEventData data)
	{
		transform.SetParent(parent);
		group.blocksRaycasts = true;
		transform.position = position;
		transform.SetSiblingIndex(index);
		GameObject obj = data.pointerCurrentRaycast.gameObject;
		if (obj == null)
		{
			LoadSpell(null);
		}
	}
    public void OnDrop(PointerEventData data)
	{
		GameObject obj = data.pointerDrag;
		if (obj)
		{
			MenuSpellCase gg = obj.GetComponent<MenuSpellCase>();
			if (gg)
			{
				LoadSpell(gg.spell);
				gg.LoadSpell(null);
			}
		}
	}

	public void LoadSpell(SpellCaster spellCaster)
	{
		if (spell && locked)
			return;
		spell = spellCaster;
		if (!spell)
		{
			spell = null;
			img.color = new Color(1, 1, 1, 0.5f);
			img.sprite = null;
		}
		else
		{
			img.sprite = spell.icon;
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
			}
			if (spell.currentCooldown > 0f)
			{
				float prc = spell.currentCooldown / spell.cooldown;
				cooldown.fillAmount = prc;
				cooldown.enabled = true;
			}
			else
			{
				cooldown.enabled = false;
				
			}
		}
	}
}
