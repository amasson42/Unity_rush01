using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEnemyInfo : MonoBehaviour {


	public Actor player;

	private GameObject menu;
	private RectTransform life;
	private Text lifeTxt;
	private Text txt;

	void Start ()
	{
		menu = transform.Find("MenuEnemie").gameObject;
		life = transform.Find("MenuEnemie/LifeBar/Life").GetComponent<RectTransform>();
		lifeTxt = transform.Find("MenuEnemie/LifeBar/TxtLife").GetComponent<Text>();
		txt = transform.Find("MenuEnemie/Info").GetComponent<Text>();
	}
	
	void Update ()
	{
		Actor cibleA = HoverUi.hover;
		if (player && !cibleA)
			cibleA = player.currentTarget;
		menu.SetActive(cibleA != null);
		if (cibleA)
		{
			Unit cible = cibleA.unit;
			life.anchorMax = new Vector2((float)cible.currentHealth / (float)cible.maxHealth, 1f);
			lifeTxt.text = "Life : " + Mathf.RoundToInt(cible.currentHealth) + " / " + Mathf.RoundToInt(cible.maxHealth);
			txt.text = "Lvl " + cible.level;
		}
	}
}
