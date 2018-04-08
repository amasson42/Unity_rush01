using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class MenuInventoryCase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {

	public ItemInventory item;

	private Image img;
	private CanvasGroup group;

	private Vector3 position;
	private Transform parent;
	private int index;

	void Awake()
	{
		img = GetComponent<Image>();
		group = GetComponent<CanvasGroup>();
	}


	public void OnBeginDrag(PointerEventData data)
    {
		if (!item)
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
		}
	}
    public void OnDrop(PointerEventData data)
	{
		GameObject obj = data.pointerDrag;
		if (obj)
		{
			MenuInventoryCase gg = obj.GetComponent<MenuInventoryCase>();
			if (gg)
			{
			}
		}
	}
}
