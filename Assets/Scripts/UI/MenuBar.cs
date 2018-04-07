using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{

	private RectTransform val;
	private Text txt;

	void Start()
	{
		val = transform.Find("val").GetComponent<RectTransform>();
		txt = transform.Find("txt").GetComponent<Text>();
	}
	
	public void SetVal(string str, float v, float min, float max)
	{
		float len = max - min;
		float prc = (v - min) / len;
		val.anchorMax = new Vector2(Mathf.Clamp(prc, 0f, 1f), 1f);
		if (str == "")
			txt.text = "";
		else
			txt.text = str + Mathf.RoundToInt(v) + " / " + Mathf.RoundToInt(max);
	}
}