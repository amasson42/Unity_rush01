using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour {
	public enum Rarity {Common, Uncommon, Rare, Epic, Legendary};
	public enum Type {Weapon, Potion, InstantPotion};
	public static readonly Color[] RarityColors = {Color.white, new Color(0.333f,0.807f,0.196f,1), new Color(0.243f,0.478f,0.909f,1), new Color(0.549f,0.231f,0.76f,1), new Color(0.972f,0.694f,0.168f,1)};
	public static readonly string[] RarityStrings = {"Common", "Uncommon", "Rare", "Epic", "Legendary"};
	public static readonly float[] RarityStatsCoef = {1f, 1.2f, 1.4f, 1.8f, 2.5f};
	public ItemEntity entityPrefab;// Prefab, do not modify
	public ItemEntity entityInstance = null;// Linked entity
	public Sprite sprite;
	public Type type = Type.Weapon;
	public Rarity rarity;
	public string weaponName;
	public int level = 1;
	public float minDamage = 0;
	public float maxDamage = 0;
	public float attackSpeed = 0;
	public float InstantHealthRegen = 0;
	
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
