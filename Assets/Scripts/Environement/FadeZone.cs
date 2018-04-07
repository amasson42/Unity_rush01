using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeZone : MonoBehaviour {

	public float targetAlphaValue = 0.2f;
	public float fadeTime = 1.0f;
	public List<MeshRenderer> fadeTargets;
	private List<Color> basicColors;
	private Color alphaColor;
	private bool playerPresent = false;
	private float lastPresentTime = 0.0f;

	// Use this for initialization
	void Start () {
		basicColors = new List<Color>();
		for (int i = 0; i < fadeTargets.Count; i++) {
			basicColors.Add(fadeTargets[i].material.color);
			StandardShaderUtils.ChangeRenderMode(fadeTargets[i].material, StandardShaderUtils.BlendMode.Transparent);
		}
		alphaColor = new Color(1.0f, 1.0f, 1.0f, targetAlphaValue);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastPresentTime < fadeTime + 5.0f) {
			if (playerPresent) {
				for (int i = 0; i < fadeTargets.Count; i++)
					fadeTargets[i].material.color = Color.Lerp(fadeTargets[i].material.color, alphaColor, Time.deltaTime / fadeTime);
			}
			else {
				for (int i = 0; i < fadeTargets.Count; i++)
					fadeTargets[i].material.color = Color.Lerp(fadeTargets[i].material.color, basicColors[i], Time.deltaTime / fadeTime);
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.GetComponent<PlayerEntityController>() != null) {
			playerPresent = true;
			lastPresentTime = Time.time;
		}
	}

	void OnTriggerExit(Collider collider) {
		if (collider.GetComponent<PlayerEntityController>() != null) {
			playerPresent = false;
			lastPresentTime = Time.time;
		}
	}
}

 public static class StandardShaderUtils
 {
     public enum BlendMode
     {
         Opaque,
         Cutout,
         Fade,
         Transparent
     }

     public static void ChangeRenderMode(Material standardShaderMaterial, BlendMode blendMode)
     {
         switch (blendMode)
         {
             case BlendMode.Opaque:
                 standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                 standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                 standardShaderMaterial.SetInt("_ZWrite", 1);
                 standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                 standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                 standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                 standardShaderMaterial.renderQueue = -1;
                 break;
             case BlendMode.Cutout:
                 standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                 standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                 standardShaderMaterial.SetInt("_ZWrite", 1);
                 standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
                 standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                 standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                 standardShaderMaterial.renderQueue = 2450;
                 break;
             case BlendMode.Fade:
                 standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                 standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                 standardShaderMaterial.SetInt("_ZWrite", 0);
                 standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                 standardShaderMaterial.EnableKeyword("_ALPHABLEND_ON");
                 standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                 standardShaderMaterial.renderQueue = 3000;
                 break;
             case BlendMode.Transparent:
                 standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                 standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                 standardShaderMaterial.SetInt("_ZWrite", 0);
                 standardShaderMaterial.DisableKeyword("_ALPHATEST_ON");
                 standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
                 standardShaderMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                 standardShaderMaterial.renderQueue = 3000;
                 break;
         }
 
     }
 }