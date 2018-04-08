﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public PlayerEntityController playerEntityController;

	public GameObject highlightNode;
	private Light highlightNodeLight;
	public Unit unit;
	public Image lifeBarMask;
	public Text lifeText;
	public Image manaBarMask;
	public Text manaText;
	public Text levelText;
	public Image experienceBarMask;
	public Button showStatsButton;
	public Image statsPanel;
	public Text strengthText;
	public Text dexterityText;
	public Text vitalityText;
	public Text energyText;
	public Text pointsText;
	public Image enemyPanel;
	public Text enemyNameText;
	public Image enemyLifeBarMask;
	public Text enemyLifeText;

	// Use this for initialization
	void Start () {
		statsPanel.gameObject.SetActive(false);
		highlightNodeLight = highlightNode.GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		lifeBarMask.fillAmount = Mathf.Clamp(unit.currentHealth / unit.maxHealth, 0, 1);
		manaBarMask.fillAmount = Mathf.Clamp(unit.currentMana / unit.maxMana, 0, 1);
		lifeText.text = "" + (int)unit.currentHealth + "/" + (int)unit.maxHealth;
		manaText.text = "" + (int)unit.currentMana + "/" + (int)unit.maxMana;
		levelText.text = "Level " + unit.level + "\nXP: " + unit.currentExperience + "/" + unit.requiredExperience;
		experienceBarMask.fillAmount = Mathf.Clamp((float)unit.currentExperience / (float)unit.requiredExperience, 0, 1);

		showStatsButton.gameObject.SetActive(unit.availableStatsPoints > 0);

		if (statsPanel.gameObject.activeInHierarchy) {
			strengthText.text = "Strength: " + unit.strength;
			dexterityText.text = "Dexterity: " + unit.dexterity;
			vitalityText.text = "Vitality: " + unit.vitality;
			energyText.text = "Energy: " + unit.energy;
			pointsText.text = "Points: " + unit.availableStatsPoints;
		}

		Actor target = unit.actor.currentTarget;
		if (target == null) {
			Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(cameraRay, out hit);
			target = hit.collider.GetComponent<Actor>();
		}

		if (target) {
			if (target.unit) {
				enemyNameText.text = "(" + target.unit.level + ")" + target.unit.unitName;
				enemyLifeBarMask.fillAmount = Mathf.Clamp(target.unit.currentHealth / target.unit.maxHealth, 0, 1);
				enemyLifeText.text = "" + (int)target.unit.currentHealth + "/" + (int)target.unit.maxHealth;
				highlightNodeLight.color = target.unit.team == 1 ? Color.green : Color.red;
			}
			highlightNode.transform.position = target.transform.position;
			highlightNode.SetActive(true);
			enemyPanel.gameObject.SetActive(true);
		} else {
			/* ADD QHONORE */
			Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast(cameraRay, out hit);
			ItemEntity item = hit.collider.GetComponent<ItemEntity>();

			if (item && item.gameObject.activeSelf) {
				highlightNodeLight.color = Loot.RarityColors[(int)item.itemInstance.rarity];
				highlightNode.transform.position = item.transform.position;
				highlightNode.SetActive(true);
			} else
			/* END ADD QHONORE */
				highlightNode.SetActive(false);
			enemyPanel.gameObject.SetActive(false);
		}
	}

	public void ShowStatsPanel() {
		statsPanel.gameObject.SetActive(true);
	}

	public void IncreaseUnitStrength() {
		if (unit.availableStatsPoints > 0) {
			unit.strength++;
			unit.availableStatsPoints--;
		}
		CloseStatPanel();
	}

	public void IncreaseUnitDexterity() {
		if (unit.availableStatsPoints > 0) {
			unit.dexterity++;
			unit.availableStatsPoints--;
		}
		CloseStatPanel();
	}

	public void IncreaseUnitVitality() {
		if (unit.availableStatsPoints > 0) {
			unit.vitality++;
			unit.availableStatsPoints--;
		}
		CloseStatPanel();
	}

	public void IncreaseUnitEnergy() {
		if (unit.availableStatsPoints > 0) {
			unit.energy++;
			unit.availableStatsPoints--;
		}
		CloseStatPanel();
	}

	void CloseStatPanel() {
		if (unit.availableStatsPoints <= 0) {
			statsPanel.gameObject.SetActive(false);
		}
	}
}
