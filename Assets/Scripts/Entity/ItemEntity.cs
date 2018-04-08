using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour {

	public ItemInventory itemPrefab;// Prefab, do not modify
	[HideInInspector]public ItemInventory itemInstance = null;// Linked item

	public int maxLootNB = 1;// Max loot in once

	void Start () {
	}

	void Update () {
		
	}
}
