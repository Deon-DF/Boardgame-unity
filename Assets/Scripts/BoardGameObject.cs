using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardGameObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

	public enum Type { card = 0, zone = 1 }
	public Type type = Type.card;
	public Card assignedCard;


	Transform savedParent = null;
	GameObject placeholder;
//	Vector3 offset = Vector3.zero;
	bool mousedOver = false;


	#region Inspector-set elements

	public bool playerobject = false;

	public GameObject goName;
	public GameObject goCost;
	public GameObject goArtwork;
	public GameObject goFrame;
	public GameObject goCardClass;
	public GameObject goDescription;

	#endregion

	#region UI interaction

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (type == Type.card && !(GUI.instance.CardDisplayCanvas.isActiveAndEnabled) && playerobject)
		{
			GUI.instance.oHandPile.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			GUI.instance.EventPile.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			// Placeholder to keep the position of the card free until we drop it.
			placeholder = new GameObject ();
			placeholder.transform.parent = this.transform.parent;
			placeholder.AddComponent<RectTransform> ();
			placeholder.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, this.GetComponent <RectTransform> ().rect.width);
			placeholder.GetComponent<RectTransform> ().SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, this.GetComponent <RectTransform> ().rect.height);
			placeholder.transform.SetSiblingIndex (this.transform.GetSiblingIndex ());

			// Moving the card out of the current parent (zone), but remembering the zone to return to it, if the object is dropped incorrectly.
			savedParent = this.transform.parent;
			this.transform.SetParent (transform.root);
			this.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			Main.instance.logger.LogUI ("Player picks up " + eventData.pointerDrag.GetComponent<BoardGameObject>().assignedCard.playcardName);
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		if (type == Type.card  && !(GUI.instance.CardDisplayCanvas.isActiveAndEnabled) && playerobject ) {
			this.transform.SetParent (savedParent);
			this.transform.SetSiblingIndex (placeholder.transform.GetSiblingIndex ());
			Destroy (placeholder);
			this.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		}

		GUI.instance.EventPile.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		GUI.instance.oHandPile.GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

	public void OnDrag (PointerEventData eventData) {
		if (type == Type.card  && !(GUI.instance.CardDisplayCanvas.isActiveAndEnabled) && playerobject) {
			this.transform.position = eventData.position;

			// Relocating the placeholder position as the card is moved on X axis, to make a manual rearrangement of cards possible.

			int index = savedParent.childCount; // we start the cycle assuming that we are at the very right.
			// Then we check each object to see if it's to the left or not, and arrange the position of the placeholder accordingly.

			for (int i = 0; i < savedParent.childCount; i++) {
				if ((this.transform.position.x < savedParent.GetChild (i).position.x) && savedParent.GetChild (i) != placeholder) {
					index = i;

					if (placeholder.transform.GetSiblingIndex () < index) {
						index--;
					}
					break;
				}
			}

			placeholder.transform.SetSiblingIndex (index);
		}
	}


	public void OnDrop (PointerEventData eventData) {
		BoardGameObject card = null;

		if (eventData.pointerDrag.GetComponent<BoardGameObject>().playerobject) {
			card = eventData.pointerDrag.gameObject.GetComponent<BoardGameObject> ();
			if (this.gameObject.name == "PlayArea") {
//				Main.instance.logger.LogUI ("Player has dropped the card " + card.assignedCard.playcardName + " on " + this.name + ". Hiding the highlighting.");
				this.GetComponent<Image> ().color = new Color (0, 0, 0, 0f);
				// Implement PLAY CARD mechanic here
				if (Main.instance.energy >= eventData.pointerDrag.GetComponent<BoardGameObject> ().assignedCard.playcardCost) {
					Main.instance.energy -= eventData.pointerDrag.GetComponent<BoardGameObject> ().assignedCard.playcardCost;
					eventData.pointerDrag.GetComponent<BoardGameObject> ().assignedCard.Play ();
					Destroy (eventData.pointerDrag.GetComponent<BoardGameObject> ().placeholder.gameObject);
					Destroy (eventData.pointerDrag.gameObject);		
				} else {
					GUI.instance.DisplayUIText ("Not enough actions to issue the order.", 2f);
				}
			} else {
				Main.instance.logger.LogUI ("The card " + card.assignedCard.playcardName + " was dropped but not above a playable area, it will return to its hand.");
			}
		}

		GUI.instance.EventPile.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		GUI.instance.oHandPile.GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

	public void OnPointerEnter (PointerEventData eventData) {

		if (this.type == Type.zone) {
			BoardGameObject card = null;
			if (eventData.pointerDrag != null) {
				card = eventData.pointerDrag.GetComponent<BoardGameObject> ();
			}
			if ((card != null) && (card.type == Type.card) && (card.playerobject)) {
				//card.placeholder.transform.SetParent(this.gameObject.transform);
				//card.savedParent = this.gameObject.transform;

				if (this.gameObject.name == "PlayArea") {
					Main.instance.logger.LogUI ("Player is hovering a card " + card.assignedCard.playcardName + " over " + this.name + ". Highlighting.");
					this.GetComponent<Image> ().color = new Color(0, 1, 0, 0.3f);
				}
			}
		}

		if (this.type == Type.card) {
			Main.instance.logger.LogUI ("Player has moved the mouse pointer over " + this.name + ". MousedOver variable of it is now true.");
			this.mousedOver = true;
		}
	}

	public void OnPointerExit (PointerEventData eventData) {
		if (this.type == Type.card) {
			Main.instance.logger.LogUI ("Player's mouse pointer is no longer over " + this.name + ". MousedOver variable of it is now false.");
			this.mousedOver = false;
		}

		if (this.type == Type.zone && this.gameObject.name == "PlayArea") {
			Main.instance.logger.LogUI ("Player is no longer hovering over " + this.name + ". Hiding the highlighting.");
			this.GetComponent<Image> ().color = new Color(0, 0, 0, 0f);
		}
	}

	#endregion

	public void RefreshCard() {	// Setting gameobject visualization based on the assigned play card
		if (assignedCard != null) {
			goName.GetComponent<Text> ().text = assignedCard.playcardName;
			goCost.GetComponent<Text> ().text = assignedCard.playcardCost.ToString ();
			goCardClass.GetComponent<Text> ().text = assignedCard.playcardClass;
			goDescription.GetComponent<Text> ().text = assignedCard.playcardShortDescription;
			goArtwork.GetComponent<Image> ().sprite = Resources.Load<Sprite> (Info.cards_path + assignedCard.playcardArt);
		}
	}

	void DisplayDetails() {
		if (Input.GetMouseButtonDown (1) && this.mousedOver == true) {
			Main.instance.logger.LogUI ("Player has right clicked " + this.name + ". Displaying the DisplayUI graphical element with details. Assigned card: " + this.assignedCard.playCard);
			GUI.instance.CardDisplayCanvas.gameObject.SetActive (true);

			Main.instance.logger.LogUI ("Setting the display card details elements" +
				". Name:" + this.goName.GetComponent<Text> ().text +
				". Type: " + this.goCardClass.GetComponent<Text> ().text +
				". Artwork: " + Info.cards_path + this.assignedCard.playcardArt +
				". Description: '" + this.goDescription.GetComponent<Text> ().text + "'.");

			GUI.instance.CardDisplay.cardFrame.GetComponent<Image> ().sprite = this.goFrame.GetComponent<Image> ().sprite;
			GUI.instance.CardDisplay.cardName.GetComponent<Text>().text = this.assignedCard.playcardName;
			GUI.instance.CardDisplay.cardType.GetComponent<Text> ().text = this.assignedCard.playcardClass;
			GUI.instance.CardDisplay.cardArtwork.GetComponent<Image> ().sprite = Resources.Load<Sprite> (Info.cards_path + this.assignedCard.playcardArt);
			GUI.instance.CardDisplay.cardDescription.GetComponent<Text> ().text = this.assignedCard.playcardDescription;
		}
	}

	public IEnumerator Die() {
		Main.instance.boardPlayerControl = false;
		yield return new WaitForSeconds (0.5f);
		this.assignedCard.OnDeath ();
		AudioPlayer.instance.soundplayer.PlayOneShot (this.assignedCard.deathsound);
		Destroy (this.gameObject);
		Main.instance.boardPlayerControl = true;
	}

	public void Update () {
		DisplayDetails ();


		if (this.type == Type.card) {
			RefreshCard ();

			if (this.playerobject == false) {
				if (this.assignedCard.playcardCost <= 0) {
					StartCoroutine (Die ());
				}

				if (this.assignedCard.trooper == false) {
					this.transform.SetParent(GUI.instance.EventPile.transform);
				}
			}
		}
	}

	public void Start () {
		//Card.InitializeGameobject (this);
	}

	public void OnDisable () {
		if (this.type == Type.card) {
			this.assignedCard.OnDiscard ();
			if (this.playerobject) {
				Main.instance.pDiscardDeck.cards.Add (this.assignedCard); Main.instance.logger.LogInfo ("OnDisable: Adding " + this.assignedCard.playcardName + " to " + Main.instance.pDiscardDeck.deckName);
				Main.instance.pHandDeck.cards.Remove (this.assignedCard); Main.instance.logger.LogInfo ("OnDisable: Removing " + this.assignedCard.playcardName + " from " + Main.instance.pHandDeck.deckName);
			} else {
				if (this.assignedCard.trooper) {
					Main.instance.oDiscardDeck.cards.Add (this.assignedCard);
					Main.instance.logger.LogInfo ("OnDisable: Adding " + this.assignedCard.playcardName + " to " + Main.instance.oDiscardDeck.deckName);
					Main.instance.oHandDeck.cards.Remove (this.assignedCard);
					Main.instance.logger.LogInfo ("OnDisable: Removing " + this.assignedCard.playcardName + " from " + Main.instance.oHandDeck.deckName);
				} else {
					Main.instance.oDiscardDeck.cards.Add (this.assignedCard);
					Main.instance.logger.LogInfo ("OnDisable: Adding " + this.assignedCard.playcardName + " to " + Main.instance.oDiscardDeck.deckName);
					Main.instance.EventDeck.cards.Remove (this.assignedCard);
					Main.instance.logger.LogInfo ("OnDisable: Removing " + this.assignedCard.playcardName + " from " + Main.instance.EventDeck.deckName);
				}
			}
		}
	}
		

}
