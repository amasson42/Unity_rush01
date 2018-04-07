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
			loot.level = level;
			Vector3 position = new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y + 2f, transform.position.z + Random.Range(-3f, 3f));
			Instantiate(loot, position, transform.rotation);
		}
	}

	bool winLotery(float lootChance)
	{
		if (Random.Range(0f, 100f) <= lootChance)
			return (true);
		return (false);
	}
}
