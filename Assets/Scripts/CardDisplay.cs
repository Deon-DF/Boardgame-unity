using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerClickHandler {

	public GameObject cardName;
	public GameObject cardFrame;
	public GameObject cardArtwork;
	public GameObject cardType;
	public GameObject cardDescription;

	public void OnPointerClick(PointerEventData eventData) {
		this.transform.root.gameObject.SetActive (false);
	}
}
