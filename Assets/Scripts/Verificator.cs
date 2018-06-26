using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Verificator {

	public void CheckBoardLayout(){

		// The board must have these elements:
		//	- Player draw pile
		//	- Player discard pile
		//	- Player hand location
		//	- Opponent draw pile
		//	- Opponent discard pile
		//	- Opponent hand location
		/*

		if (Main.instance.pDrawPile == null) {
			Main.instance.logger.LogError ("Player draw pile is missing from the scene or not attached to the Main gameObject! Quitting...");
			Application.Quit ();
		} else
		//	- Player discard pile
		if (Main.instance.pDiscardPile == null) {
			Main.instance.logger.LogError ("Player discard pile is missing from the scene or not attached to the Main gameObject! Quitting...");
			Application.Quit ();
		} else
		//	- Player hand location
		if (Main.instance.pHandPile == null) {
			Main.instance.logger.LogError ("Player hand pile is missing from the scene or not attached to the Main gameObject! Quitting...");
			Application.Quit ();
		} else
		//	- Opponent draw pile
		if (Main.instance.oDrawPile == null) {
			Main.instance.logger.LogError ("Opponent draw pile is missing from the scene or not attached to the Main gameObject! Quitting...");
			Application.Quit ();
		} else
		//	- Opponent discard pile
		if (Main.instance.oDiscardPile == null) {
			Main.instance.logger.LogError ("Opponent discard pile is missing from the scene or not attached to the Main gameObject! Quitting...");
			Application.Quit ();
		} else
		//	- Opponent hand location
		if (Main.instance.oHandPile == null) {
			Main.instance.logger.LogError ("Opponent hand pile is missing from the scene or not attached to the Main gameObject! Quitting...");
			Application.Quit ();
		} else {
		// All checked objects are present
			Main.instance.logger.LogInfo("Board elements are verified successfully (player hand, draw pile, discard pile; opponent hand, draw pile, discard pile).");
		}

*/
	}

	public void DebugListDeck() {
		if (Input.GetKeyDown(KeyCode.C)) {
			foreach (Deck _deck in Main.instance.all_decks) {
				if (_deck.cards.Count > 0) {
					string temp = _deck.deckName + ": ";
					for (int i = 0; i < _deck.cards.Count; i++) {
						temp += "[ " + i + " : " + _deck.cards [i].playcardName + " ] ";
					}
					Debug.Log (temp);
				} else
					Debug.Log (_deck.deckName + " is empty!");
			}
		}
	}

	public void DebugMoveCards() {
		if (Input.GetKeyDown (KeyCode.G)) {
			Deck.MoveCard (0, Main.instance.pDrawDeck, Main.instance.pDiscardDeck);
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			Deck.MoveCard (0, Main.instance.pDiscardDeck, Main.instance.pDrawDeck);
		}
	}

	public void DebugDraw() {
		if (Input.GetKeyDown (KeyCode.D)) {
			Main.instance.pDrawDeck.DrawCard ();
		}
	}

	public void DebugPlayerDeck () {
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.elder);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.raid);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.raid);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.raid);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.raid);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.raid);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.militia);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.militia);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.militia);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.militia);
		Main.instance.pDrawDeck.AddCard(Card.PlayCard.militia);
	}

	public void DebugEnemyDeck() {
		GUI.instance.enemyBase.GetComponent<Image> ().sprite = Resources.Load<Sprite>("graphics/board/enemybase/goblin_camp");
		GUI.instance.enemyBaseName.GetComponent<Text> ().text = "Goblin camp";


		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_assassin);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_assassin);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_druid);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_druid);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_scout);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_scout);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_scout);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_scout);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_archer);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_archer);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_archer);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_raider);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_raider);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_raider);
		Main.instance.oDrawDeck.AddCard (Card.PlayCard.goblin_warlock);
	}

	public void DebugNextSeason() {
		if (Input.GetKeyDown (KeyCode.L)) {
			Main.instance.NextSeason ();
		}
	}

	public void DebugIncreaseWalls() {
		if (Input.GetKeyDown (KeyCode.W)) {
			Main.instance.town.government.level += 1;
			Main.instance.town.hunter.level += 1;
			Main.instance.town.smithy.level += 1;
			Main.instance.town.tavern.level += 1;
			Main.instance.town.trader.level += 1;
			Main.instance.town.walls.level += 1;
			Main.instance.town.warehouse.level += 1;
		}
	}
}
