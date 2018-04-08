using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {
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
			Quaternion rotation = Quaternion.Euler(0, 0, 0);
			if (loot.itemPrefab.type != ItemInventory.Type.InstantPotion)
				rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
			loot = Instantiate(loot, position, rotation);
			loot.itemInstance = Instantiate(loot.itemPrefab, position, transform.rotation);
			generateStats(ref loot.itemInstance, level);
			loot.itemInstance.entityInstance = loot;
		}
	}

	ItemInventory.Rarity getItemRarity()
	{
		float rand = Random.Range(0f, 100f);
		if (rand > 99f) return (ItemInventory.Rarity.Legendary);
		if (rand > 95f) return (ItemInventory.Rarity.Epic);
		if (rand > 90f) return (ItemInventory.Rarity.Rare);
		if (rand > 70f) return (ItemInventory.Rarity.Uncommon);
		return (ItemInventory.Rarity.Common);
	}

	bool winLotery(float lootChance)
	{
		if (Random.Range(0f, 100f) <= lootChance)
			return (true);
		return (false);
	}

	void generateStats(ref ItemInventory instance, int level)
	{
		if (instance.type == ItemInventory.Type.InstantPotion)
			return;
		instance.rarity = getItemRarity();
		instance.weaponName = ItemInventory.RarityStrings[(int)instance.rarity] + " " + instance.weaponName;
		instance.level = level;
		instance.minDamage = Random.Range(20f, 25f) * Mathf.Pow(1.1f, level - 1) * ItemInventory.RarityStatsCoef[(int)instance.rarity] * instance.attackSpeed;
		instance.maxDamage = Random.Range(instance.minDamage, instance.minDamage * 1.5f);
		instance.attackSpeed = Random.Range(instance.attackSpeed * 0.75f, instance.attackSpeed * 1.25f);
	}
}