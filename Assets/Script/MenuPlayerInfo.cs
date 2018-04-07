using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayerInfo : MonoBehaviour {

	public Unit player;

	private RectTransform xp;
	private RectTransform life;
	private Text xpTxt;
	private Text lifeTxt;




	void Start ()
	{
		xp = transform.Find("XpBar/Xp").GetComponent<RectTransform>();
		xpTxt = transform.Find("TxtXpVal").GetComponent<Text>();
		life = transform.Find("LifeBar/val").GetComponent<RectTransform>();
		lifeTxt = transform.Find("LifeBar/txt").GetComponent<Text>();
	}
	
	void Update ()
	{
		if (!player)
		{
			Destroy(gameObject);
			// return ;
		}
		xp.anchorMax = new Vector2((float)player.currentExperience/ (float)player.requiredExperience, 1f);
		life.anchorMax = new Vector2((float)player.currentHealth / (float)player.maxHealth, 1f);
		lifeTxt.text = "Life : " + player.currentHealth + " / " + player.maxHealth;
		xpTxt.text = player.currentExperience + "/" + player.requiredExperience;
	}
}
