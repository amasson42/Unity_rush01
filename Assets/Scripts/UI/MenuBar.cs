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
	
	public void SetVal(string str, float min, float max)
	{
		if (max == 0.0f)
			txt.text = str;
		else
		{
			float len = max - min;
			float tot = min + max;
			float prc = tot / len;
			txt.text = str + Mathf.RoundToInt(min) + " / " + Mathf.RoundToInt(max);
		}
	}
}
