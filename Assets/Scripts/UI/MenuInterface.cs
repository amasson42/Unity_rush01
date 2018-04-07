using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuInterface : MonoBehaviour
{
	private RectTransform _rect;
	private Canvas _canvas;

	private void Awake()
	{
		_rect = GetComponent<RectTransform>();
		_canvas = GetComponent<Canvas>();
	}


	private Vector3 _moveOffset;
	public void MoveStart(Vector3 pos)
	{
		_moveOffset = _rect.position - pos;
	}
	public void Move(Vector3 pos)
	{
		pos += _moveOffset;
		float x = _rect.rect.width * 0.5f;
		float y = _rect.rect.height * 0.5f;
		_rect.position = new Vector3(
			Mathf.Clamp(pos.x, x, x + Screen.width - _rect.rect.width),
			Mathf.Clamp(pos.y, y, y + Screen.height - _rect.rect.height),
			_rect.position.y);
	}

	public void Hide()
	{
		_canvas.enabled = false;
	}
	public void Show()
	{
		_canvas.enabled = true;
	}
	public void Toggle()
	{
		if (_canvas.enabled)
			Hide();
		else
			Show();
	}
	public bool Visible()
	{
		return (_canvas.enabled);
	}


	void Start()
	{
		
	}
	
	void Update()
	{
	}
}
