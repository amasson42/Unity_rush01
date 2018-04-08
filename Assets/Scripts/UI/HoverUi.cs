using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class HoverUi : MonoBehaviour
{
	private Canvas canvas;
	private RectTransform rect;
	private Text hoverText;
	private Image border;

	public static Actor hover;

	[SerializeField]
	private GameObject highlightNodePrefab;
	private GameObject highlightNode;
	// private Light highlightNodeLight;


	void Awake()
	{
		canvas = GetComponent<Canvas>();
		rect = transform.Find("Menu").GetComponent<RectTransform>();
		hoverText = rect.transform.Find("Text").GetComponent<Text>();
		border = rect.transform.Find("Border").GetComponent<Image>();
	}
	void Start()
	{
		highlightNode = Instantiate(highlightNodePrefab, Vector3.zero, Quaternion.identity);
		// highlightNodeLight = highlightNode.GetComponentInChildren<Light>();
	}
	void Update()
	{
		RaycastHit hit = PlayerEntityController.GetClickedRaycast();
		if (hit.collider && hit.collider.gameObject.layer != LayerMask.NameToLayer("World") &&
			!MenuManager.OnInterface())
		{
			hover = hit.collider.GetComponent<Actor>();
			if (hover)
				canvas.enabled = false;
			ShowItemInfo(hit.collider.GetComponent<ItemEntity>());
			highlightNode.transform.position = hit.transform.position;
			highlightNode.SetActive(true);
		}
		else
		{
			highlightNode.SetActive(false);
			canvas.enabled = false;
		}
	}

	void ShowItemInfo(ItemEntity item)
	{
		if (item == null)
		{
			canvas.enabled = false;
			return ;
		}
		Vector3 pos = Camera.main.WorldToScreenPoint(item.transform.position);
		pos.z = 0;
		// float x = rect.rect.width * 0.5f;
		float y = rect.rect.height * 0.5f;
		pos.y += y + 20;
		canvas.enabled = true;
		rect.position = pos;
		border.color = ItemInventory.RarityColors[(int)item.itemInstance.rarity];
		ItemInventory inv = item.itemInstance;
		if (inv)
		{
			hoverText.text = 
				  "Level       : " + inv.level +
				"\nDamage      : " + (long)inv.minDamage + "-" + (long)inv.maxDamage +
				"\nAttackSpeed : " + inv.attackSpeed +
				"\nDps         : " + (long)((inv.minDamage + inv.maxDamage) / inv.attackSpeed * 0.5f);
		}
		else
			hoverText.text = item.ToString();
	}
}
