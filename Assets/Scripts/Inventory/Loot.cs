﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {
	public enum Rarity {Common, Uncommon, Magic, Epic, Legendary};
	public enum Type {Weapon, Potion};
	public static readonly Color[] RarityColors = {Color.white, new Color(0.333f,0.807f,0.196f,1), new Color(0.243f,0.478f,0.909f,1), new Color(0.549f,0.231f,0.76f,1), new Color(0.972f,0.694f,0.168f,1)};
	public static readonly string[] RarityStrings = {"Common", "Uncommon", "Magic", "Epic", "Legendary"};
	public static readonly float[] RarityStatsCoef = {1f, 1.2f, 1.4f, 1.8f, 2.5f};

	public float	lootChance = 20f;
	public ItemEntity[]	loots;

	void Start () {
	}

	void Update () {
	}

	public void generateLoot(int level)
	{
		if (!winLotery(lootChance))
			return;
		
		ItemEntity loot = loots[Random.Range(0, loots.Length)];
		int lootNB = Random.Range(1, loot.maxLootNB + 1);
		for (int i = 0; i < lootNB; ++i)
		{
			Vector3 position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + 2f, transform.position.z + Random.Range(-1f, 1f));
			Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
			loot = Instantiate(loot, position, rotation);
			loot.itemInstance = Instantiate(loot.itemPrefab, position, transform.rotation);
			generateStats(ref loot.itemInstance, level);
			loot.itemInstance.entityInstance = loot;
		}
	}

	Rarity getItemRarity()
	{
		float rand = Random.Range(0f, 100f);
		if (rand > 99f) return (Rarity.Legendary);
		if (rand > 95f) return (Rarity.Epic);
		if (rand > 90f) return (Rarity.Magic);
		if (rand > 70f) return (Rarity.Uncommon);
		return (Rarity.Common);
	}

	bool winLotery(float lootChance)
	{
		if (Random.Range(0f, 100f) <= lootChance)
			return (true);
		return (false);
	}

	void generateStats(ref ItemInventory instance, int level)
	{
		instance.rarity = getItemRarity();
		instance.weaponName = RarityStrings[(int)instance.rarity] + " " + instance.weaponName;
		instance.level = level;
		instance.minDamage = Random.Range(20f, 25f) * Mathf.Pow(1.1f, level - 1) * RarityStatsCoef[(int)instance.rarity] * instance.attackSpeed;
		instance.maxDamage = Random.Range(instance.minDamage, instance.minDamage * 1.5f);
		instance.attackSpeed = Random.Range(instance.attackSpeed * 0.75f, instance.attackSpeed * 1.25f);
	}
}