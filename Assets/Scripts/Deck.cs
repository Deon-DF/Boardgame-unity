using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck {

	public List<Card> cards = new List<Card>();
	public string deckName = "";

	public void AddCard (Card.PlayCard _playcard) {
		Card _card = new Card(_playcard);
		cards.Add (_card);
		Main.instance.logger.LogInfo ("ADDCARD: " + _card.playcardName + " to deck: " + deckName + ".");
	}

	public void DrawCard() {
		if (Main.instance.pDrawDeck.cards.Count > 0) {
			if (Main.instance.pHandDeck.cards.Count < Main.instance.pHandSize) {
				Main.instance.logger.LogInfo ("DRAWCARD: " + this.cards [0].playcardName + " into " + Main.instance.pHandDeck.deckName + ".");
				Deck.MoveCard (0, this, Main.instance.pHandDeck);
			} else {
				Main.instance.logger.LogWarning ("DRAWCARD: hand is full, burning: " + this.cards [0].playcardName);
				GUI.instance.DisplayUIText ("Your hand is full! The card " + this.cards [0].playcardName + " is discarded.", 2f);
				this.cards [0].OnDiscard ();
				Deck.MoveCard (0, this, Main.instance.pDiscardDeck);
			}

		} else {
			if (Main.instance.pDiscardDeck.cards.Count > 0) {
				Main.instance.logger.LogInfo ("DRAWCARD: DrawDeck is empty. DiscardDeck is moved into DrawDeck.");
				int discardsize = Main.instance.pDiscardDeck.cards.Count;
				for (int i = 0; i < discardsize; i++) {
					Main.instance.logger.LogInfo ("DRAWCARD: Moving card " + Main.instance.pDiscardDeck.cards[0].playcardName + " to the draw pile.");
					MoveCard (0, Main.instance.pDiscardDeck, Main.instance.pDrawDeck);					
				}
				// shuffle
				Main.instance.pDrawDeck.Shuffle ();
				Main.instance.logger.LogInfo ("DRAWCARD: Draw deck size now is: " + Main.instance.pDrawDeck.cards.Count);
				DrawCard ();
			} else {
				Main.instance.logger.LogInfo ("DRAWCARD: DrawDeck is empty, DiscardDeck is empty, cannot draw a card.");
			}
		}

	}


	public void OpponentDrawCard() {
			if (this.cards.Count > 0) {
				if (Main.instance.oHandDeck.cards.Count < Main.instance.oHandSize) {
					Main.instance.logger.LogInfo ("OPPONENTDRAWCARD: Opponent draws " + this.cards [0].playcardName + ".");
					Deck.MoveCard (0, this, Main.instance.oHandDeck);
				} else {
					Main.instance.logger.LogWarning ("OPPONENTDRAWCARD: opponent hand is full, burning: " + this.cards [0].playcardName);
					this.cards [0].OnDiscard ();
					Deck.MoveCard (0, this, Main.instance.oDiscardDeck);
				}
			}
		}

	public static void MoveCard(int index, Deck _deck1, Deck _deck2) {
		if (_deck1.cards.Count > 0) {
			if (_deck2 == Main.instance.pHandDeck) {
				Main.instance.logger.LogInfo ("MOVECARD: Add " + _deck1.cards[index].playcardName + " to " + _deck2.deckName);
				_deck2.cards.Add (_deck1.cards [index]);
				Main.instance.logger.LogInfo ("MOVECARD: Spawn " + _deck2.cards [_deck2.cards.Count - 1].playcardName + " as GameObject for player.");
				Main.instance.SpawnCard (_deck2.cards[_deck2.cards.Count - 1], GUI.instance.pHandPile);
				Main.instance.logger.LogInfo ("MOVECARD: Remove " + _deck1.cards[index].playcardName + " from " + _deck1.deckName);
				_deck1.cards [index].OnDeal ();
				_deck1.cards.Remove (_deck1.cards [index]);
			} else if (_deck2 == Main.instance.oHandDeck) {
				Main.instance.logger.LogInfo ("MOVECARD: Add " + _deck1.cards [index].playcardName + " to " + _deck2.deckName);
				_deck2.cards.Add (_deck1.cards [index]);
				Main.instance.logger.LogInfo ("MOVECARD: Spawn " + _deck2.cards [_deck2.cards.Count - 1].playcardName + " as GameObject for opponent.");
				Main.instance.SpawnOpponentCard (_deck2.cards [_deck2.cards.Count - 1], GUI.instance.oHandPile);
				_deck1.cards [index].OnDeal ();
				Main.instance.logger.LogInfo ("MOVECARD: Remove " + _deck1.cards [index].playcardName + " from " + _deck1.deckName);
				_deck1.cards.Remove (_deck1.cards [index]);
			} else {
				Main.instance.logger.LogInfo ("MOVECARD: Add " + _deck1.cards [index].playcardName + " to " + _deck2.deckName);
				_deck2.cards.Add (_deck1.cards [index]);
				Main.instance.logger.LogInfo ("MOVECARD: Remove " + _deck1.cards [index].playcardName + " from " + _deck1.deckName);
				_deck1.cards.Remove (_deck1.cards [index]);
			}

		} else {
			Main.instance.logger.LogWarning ("MOVECARD: Cannot move element0 from " + _deck1.deckName + " to " + _deck2.deckName + ", because " + _deck1.deckName + " is empty!");
		}
	}

	public void Shuffle() {
		Main.instance.logger.LogInfo ("SHUFFLE: " + this.deckName);
		int deckSize = cards.Count;

		for (int i = deckSize - 1; i >= 0; i--) {
			Card temp = cards [i];
			int random = Random.Range (0, deckSize);
			cards [i] = cards [random];
			cards [random] = temp;
		}
	}

}
