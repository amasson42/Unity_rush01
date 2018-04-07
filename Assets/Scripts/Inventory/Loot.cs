using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour {

	public float	lootChance = 20f;
	public ItemEntity[]	loots;

	void Start () {
	}
	
	// Update is called once per frame
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
			Vector3 position = new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y + 2f, transform.position.z + Random.Range(-3f, 3f));
			loot = Instantiate(loot, position, transform.rotation);
			generateStats(ref loot, level);
			loot.itemInstance = Instantiate(loot.itemPrefab, position, transform.rotation);
			loot.itemInstance.entityInstance = loot;
		}
	}

	bool winLotery(float lootChance)
	{
		if (Random.Range(0f, 100f) <= lootChance)
			return (true);
		return (false);
	}

	void generateStats(ref ItemEntity entity, int level)
	{
		entity.itemPrefab.type = entity.type;
		entity.itemPrefab.level = level;
		entity.itemPrefab.minDamage = Random.Range(2.5f, 5f) * level;
		entity.itemPrefab.maxDamage = Random.Range(entity.itemPrefab.minDamage, entity.itemPrefab.minDamage * 2f);
		entity.itemPrefab.attackSpeed = Random.Range(1f, 2f);
	}
}