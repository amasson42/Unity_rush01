using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour {

	public ItemInventory itemPrefab;// Prefab, do not modify
	[HideInInspector]public ItemInventory itemInstance = null;// Linked item

	public int maxLootNB = 1;// Max loot in once

	void OnTriggerEnter(Collider other)
	{
        Unit unit = other.gameObject.GetComponent<Unit>();
		if (itemInstance.type == ItemInventory.Type.InstantPotion && unit && unit.team == 1)
		{
			unit.currentHealth += unit.maxHealth * 0.3f;
			if (unit.currentHealth > unit.maxHealth)
				unit.currentHealth = unit.maxHealth;
			Destroy(this.gameObject);
			Destroy(itemInstance.gameObject);
		}
    }
}
