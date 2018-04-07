using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDisabler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public PlayerEntityController playerController;

    // Use this for initialization
    void Start () {
        
    }
    
    public void OnPointerEnter(PointerEventData eventData) {
		playerController.takeMouseEvent = false;
    }

    public void OnPointerExit(PointerEventData eventData) {
		playerController.takeMouseEvent = true;
    }
}