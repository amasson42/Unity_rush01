﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour {

	public ItemEntity entityPrefab;
	public ItemEntity entityInstance = null;
	public Loot.Type type = Loot.Type.Weapon;
	public Loot.Rarity rarity;
	public int level;
	public float minDamage;
	public float maxDamage;
	public float attackSpeed;
	
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}