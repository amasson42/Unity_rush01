using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public delegate void OnHover(GameObject obj);
    public event OnHover OnHoverEvent;
	public delegate void OnHoverEnter(GameObject obj);
    public event OnHoverEnter OnHoverEnterEvent;
	public delegate void OnHoverExit(GameObject obj);
    public event OnHoverExit OnHoverExitEvent;

	private bool hovered = false;
	public void OnPointerEnter(PointerEventData data)
	{
		hovered = true;
		if (OnHoverEnterEvent != null)
			OnHoverEnterEvent(gameObject);
	}

	public void OnPointerExit(PointerEventData data)
	{
		hovered = false;
		if (OnHoverExitEvent != null)
			OnHoverExitEvent(gameObject);
	}
	
	void Update()
	{
		if (hovered && OnHoverEvent != null)
			OnHoverEvent(gameObject);
	}
}
