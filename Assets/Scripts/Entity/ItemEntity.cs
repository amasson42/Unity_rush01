using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour {

	public ItemInventory itemPrefab;
	[HideInInspector]public ItemInventory itemInstance = null;

	[HideInInspector]public int level;
	public int maxLootNB = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
