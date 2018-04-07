using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour {

	public ItemInventory itemPrefab;
	[HideInInspector]public ItemInventory itemInstance = null;

	public int maxLootNB = 1;
	public Loot.Type type;
	public Loot.Rarity rarity;

	void Start () {
	}

	void Update () {
		
	}
}
