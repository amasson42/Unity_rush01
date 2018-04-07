using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Weapon, Potion};

public class ItemEntity : MonoBehaviour {

	public ItemInventory itemPrefab;
	[HideInInspector]public ItemInventory itemInstance = null;

	public int maxLootNB = 1;
	public ItemType type;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
