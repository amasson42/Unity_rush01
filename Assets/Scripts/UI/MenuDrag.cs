using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuDrag : MonoBehaviour, IDragHandler, IBeginDragHandler {

	private MenuInterface _menu;
	private Button _button;
	public Button button{get{return _button;}}

	public void OnBeginDrag(PointerEventData data)
    {
		_menu.MoveStart(data.position);
    }

    public void OnDrag(PointerEventData data)
    {
		_menu.Move(data.position);
    }

	private void Awake()
	{
		_menu = GetComponentInParent<MenuInterface>();
		_button = GetComponentInChildren<Button>();
		_button.onClick.AddListener(_menu.Hide);
	}
}
