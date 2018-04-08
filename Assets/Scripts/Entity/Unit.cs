using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

	public Actor actor;
	public int team;

	// Hero stats
	public int level = 1; // current level
	public long currentExperience = 0;
	public static readonly long baseRequiredExperience = 30;
	public static long RequiredExperienceAtLevel(int level) {
		/* ADD QHONORE */
		// return ((long)(baseRequiredExperience * Mathf.Pow(1.25f, level - 1)));
		return ((long)(baseRequiredExperience * level * level));
		/* END ADD QHONORE */
		// if (level < 2)
		// 	return 10 * level + 20;
		// return RequiredExperienceAtLevel(level - 2) + RequiredExperienceAtLevel(level - 1);
	}
	[HideInInspector] public long requiredExperience; // the remaining experience we want to level up
	public long experienceValue {get {return level * 5;}} // the experience collected when killed
	public int strength = 1; // increase melee damages by 5%
	public int dexterity = 1; // increase ranged damages by 5% and attack speed by 1% and multiply chances to miss by 0.99;
	public int vitality = 1; // increase life by 5
	public int energy = 1; // increase mana by 3
	public int availableStatsPoints = 0;

	// Unit Stats
	public string unitName = "Unknown";
	public float currentHealth = 100; // current health points
	public float baseHealth = 100; // base max health before adding bonus
	public float maxHealth {get {return baseHealth + vitality * 5;}} // computed max health from base with bonus
	public float healthRegenProportion = 0.01f; // percent life recuperation per seconds
	public bool isAlive {get {return currentHealth > 0;}}
	public float currentMana = 30; // variable mana points
	public float baseMana = 30; // base max mana before adding bonus
	public float manaRegenProportion = 0.01f;
	public float maxMana {get {return baseMana + energy * 3;}} // computed max mana from base with bonus
	public float factorDamages = 1.0f;

	// Weapon
	public float weaponAttackMin; // base weapon attack damages
	public float weaponAttackMax; // base weapon attack damages
	public float weaponAttackDamage {get {return Random.Range(weaponAttackMin, weaponAttackMax + 1);}}
	public float weaponAttackPeriod; // base weapon cooldown
	public float weaponAttackRange; // base weapon range
	public int weaponAttackAnimation; // weapon attack animation number ("Attack#")
	[HideInInspector] public float attackCooldown = 0.0f;

	// Fight
	public float currentDamage {get {return factorDamages * weaponAttackDamage * (1.0f + (float)strength * 0.05f);}} // current damage with bonus
	public float currentSpellDamages {get {return 1.0f + energy * 0.01f;}} // multiply spell damages
	public float currentWeaponPeriod {get {
		return weaponAttackPeriod * (1.0f / (1.0f + 0.01f * (float)dexterity)); // current cooldown with bonus
	}}

	void Start () {
		UpdateRequiredExperience();
		currentHealth = maxHealth;
		currentMana = maxMana;
	}

	void Update () {
		if (isAlive) {
			if (attackCooldown > 0.0f)
				attackCooldown -= Time.deltaTime;
			float maxHealth = this.maxHealth;
			currentHealth += maxHealth * healthRegenProportion * Time.deltaTime;
			if (currentHealth > maxHealth)
				currentHealth = maxHealth;
			float maxMana = this.maxMana;
			currentMana += maxMana * manaRegenProportion * Time.deltaTime;
			if (currentMana > maxMana)
				currentMana = maxMana;
		}
	}

	public void ResetAttackCooldown() {
		attackCooldown = currentWeaponPeriod;
	}

	public void AutoAttack(Unit target) {
		if (target)
			target.TakeDamages(currentDamage, this);
	}

	public void TakeDamages(float amount, Unit sender) {
		currentHealth -= amount;
		if (currentHealth <= 0.0f) {
			currentHealth = 0.0f;
			sender.GainExperiencePoints(experienceValue);
			actor.PlayDeadAnimation();
			actor.RemoveFromGame(5.0f);
		}
	}

	public bool CanPayCostForSpell(SpellCaster caster, out string error) {
		if (caster.manaCost <= currentMana) {
			error = "success";
			return true;
		} else {
			error = "not enough of mana";
			return false;
		}
	}

	public void PayManaCost(SpellCaster caster) {
		currentMana -= caster.manaCost;
	}

	private void UpdateRequiredExperience() {
		requiredExperience = RequiredExperienceAtLevel(level);
	}

	public void GainExperiencePoints(long amount) {
		currentExperience += amount;
		CheckExperience();
	}

	public void LevelUp() {
		level++;
		strength += level / 3 + 1;
		dexterity += level / 3 + 1;
		vitality += level / 2 + 1;
		energy += level / 2 + 1;
		availableStatsPoints += 5;
		currentHealth = maxHealth;
		currentMana = maxMana;
		UpdateRequiredExperience();
		CheckExperience();
	}

	private void CheckExperience() {
		if (currentExperience >= requiredExperience) {
			currentExperience -= requiredExperience;
			LevelUp();
		}
	}
}
