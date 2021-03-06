﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {

	#region PlayCard types
	public enum PlayCard { none, raid, militia, elder,
							// goblins
							goblin_scout, goblin_archer, goblin_raider, goblin_druid, goblin_warlock,
							// wildlife
							animal_boar,  animal_wolf, animal_wolf_pack,
							// demons
							demon_fire,
							// agents
							animal_crows, goblin_assassin
	}
	#endregion
	#region Effects
	public enum Effect { none, physical_attack, magic_attack, summon, physical_defense, magic_defense, action }
	#endregion
	#region Stats
	public PlayCard playCard = PlayCard.none;
	public Effect playcardEffect = Effect.none;
	public PlayCard playcardSummon = PlayCard.none;

	public string playcardName = "";
	public string playcardArt = "";
	public string playcardClass = "";
	public string playcardShortDescription = "";
	public string playcardDescription = "";
	public int playcardValue = 0;
	public int playcardCost = 0; // Energy to play for the player; health for the opponent cards
	public AudioClip deathsound;

	public bool permanent = false;
	public bool player_owned = false;
	public bool trooper = true;
	#endregion

	#region Functions to describe attacking the opponent

	public void AttackPlayer(int value) {
		int damage = (value - Main.instance.fortification);
		if (damage > 0) {
			Main.instance.fortification = 0;
			Main.instance.hp -= damage;
			Main.instance.damageDealtLastTurn += damage;
		} else {
			Main.instance.fortification -= value;
		}
	}

	public void FortifyPlayer(int value) {
		Main.instance.logger.LogInfo("Adding fortification: " + value.ToString ());
		Main.instance.fortification += value;
	}

	public int DamageOpponent (int damage) {

		int enemyHP = 0;

		foreach (Card monster in Main.instance.oHandDeck.cards) {
			enemyHP += monster.playcardCost;
		}

		if (damage >= enemyHP) {
			foreach (Card monster in Main.instance.oHandDeck.cards) {
				monster.playcardCost = 0;
			}
			return (damage - enemyHP);
		} else
			// TODO Implement reduction of opponent HP at the same time
			foreach (Card monster in Main.instance.oHandDeck.cards) {
				if (damage > 0) {
					if (damage > monster.playcardCost) {
						damage -= monster.playcardCost;
						monster.playcardCost = 0;
					} else {
						monster.playcardCost -= damage;
						damage = 0;
					}
				}
			}

			return 0;
	}


	void DamageDeck(int damage) {

		if (Main.instance.oDrawDeck.cards.Count > 0 && damage > 0) {
			if (damage > Main.instance.oDrawDeck.cards.Count) {
				Main.instance.logger.LogInfo ("DAMAGEOPPONENT: Correcting damage to match the max amount of cards that the opponent has in the draw pile.");
				damage = Main.instance.oDrawDeck.cards.Count;
			}
			Main.instance.logger.LogInfo ("DAMAGEOPPONENT: After hitting the enemy line, the damage leftover is: " + damage.ToString ());
			for (int i = 0; i < damage; i++) {
				Main.instance.logger.LogInfo ("DAMAGEOPPONENT: Your attack caused the enemy to discard the card: " + Main.instance.oDrawDeck.cards [0].playcardName);
				Deck.MoveCard (0, Main.instance.oDrawDeck, Main.instance.oDiscardDeck);
			}
		} else
			Main.instance.logger.LogInfo ("DAMAGEOPPONENT: No more damage left, all is soaked by the opponent defenders!");
	}

	#endregion Functions to describe attacking the opponent

	#region Deal/Play/Discard/Death effects

	public void OnDeal () {
		Main.instance.logger.LogInfo ("ONDEAL: The card " + playcardName + " is dealt in the hand.");

		if (this.trooper == false) {
			Main.instance.EventDeck.cards.Add (this);
			Main.instance.oHandDeck.cards.Remove (this);
		}
	}

	public void Play() {
		Main.instance.logger.LogInfo ("Play effect is being resolved. Card played: " + playCard);

		// Player cards play effects
		if (player_owned) {
			switch (playCard) {
			case PlayCard.raid:
				AudioPlayer.instance.soundplayer.PlayOneShot (AudioPlayer.instance.sword_slash);
				DamageDeck(DamageOpponent(this.playcardValue));
				break;
			case PlayCard.militia:
				AudioPlayer.instance.soundplayer.PlayOneShot (AudioPlayer.instance.sword_slash);
				FortifyPlayer(this.playcardValue);
				break;
			case PlayCard.elder:
				Main.instance.energy += this.playcardValue;
				break;

			default:
				Main.instance.logger.LogError(this.playcardName + " has been played, but it has no effect assigned in Play() function in Card.cs! The current PlayCard class is " + playCard);
				break;
			}
		}

		// Opponent cards play effects
		if (!player_owned) {
			if (playcardEffect == Effect.physical_attack) {
				AttackPlayer(playcardValue);
			} else if (playcardEffect == Effect.summon) {
				for (int i = 0; i < playcardValue; i++) {
					Main.instance.logger.LogInfo("Adding a new card " + playcardSummon + " to the summon pile");
					Main.instance.SummonDeck.AddCard(playcardSummon);
				}
			}
		}
	}

	public void OnDiscard() {
		Main.instance.logger.LogInfo ("ONDISCARD: The card " + playcardName + " is discarded.");

		switch (playCard) {
				
		default:
			break;
		}
	}

	public void OnDeath() {
		switch (playCard) {

		default:
			break;
		}
	}

	#endregion

	public Card (PlayCard _cardClass) {
		
		switch (_cardClass) {

		#region Player cards
		case Card.PlayCard.elder:
			player_owned = true;
			playcardName = "Elder";
			playCard = PlayCard.elder;
			playcardArt = "village_elder";
			playcardClass = "Village";
			playcardCost = 0;
			playcardValue = 1;
			playcardEffect = Effect.action;
			playcardShortDescription = "+" + playcardValue + " action(s).";
			playcardDescription = "Gives " + playcardValue + " extra action(s) this turn.";
			break;

		case Card.PlayCard.raid:
			player_owned = true;
			playcardName = "Raid";
			playCard = PlayCard.raid;
			playcardArt = "village_raid";
			playcardClass = "Village";
			playcardCost = 1;
			playcardValue = 1;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = "Deals " + playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to the closest enemy. If no enemies left, burn the opponent draw deck.";
			break;

		case Card.PlayCard.militia:
			player_owned = true;
			playcardName = "Militia";
			playCard = PlayCard.militia;
			playcardArt = "village_militia";
			playcardClass = "Village";
			playcardCost = 1;
			playcardValue = 1;
			playcardEffect = Effect.physical_defense;
			playcardShortDescription = "+" + playcardValue + " to fortifications.";
			playcardDescription = "Adds " + playcardValue+ " to fortifications.";
			break;
		#endregion
		
		#region Opponent cards

		// WILDLIFE

		case Card.PlayCard.animal_boar:
			playcardName = "Boar";
			playCard = PlayCard.animal_boar;
			playcardArt = "animal_boar";
			playcardClass = "Beast";
			playcardValue = 1;
			playcardCost = 2;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to your settlement";
			deathsound = AudioPlayer.instance.goblin_groan;
			break;

		case Card.PlayCard.animal_crows:
			playcardName = "Crows";
			playCard = PlayCard.animal_crows;
			playcardArt = "animal_crows";
			playcardClass = "Beast";
			trooper = false;
			playcardValue = 1;
			playcardCost = 1;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to your settlement.\nCannot be directly attacked.";
			deathsound = AudioPlayer.instance.goblin_groan;
			break;

		case Card.PlayCard.animal_wolf:
			playcardName = "Wolf";
			playCard = PlayCard.animal_wolf;
			playcardArt = "animal_wolf";
			playcardClass = "Beast";
			playcardValue = 1;
			playcardCost = 1;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to your settlement";
			deathsound = AudioPlayer.instance.goblin_groan;
			break;
		
		case Card.PlayCard.animal_wolf_pack:
			playcardName = "Wolf pack";
			playCard = PlayCard.animal_wolf_pack;
			playcardArt = "animal_wolf_pack";
			playcardClass = "Beast";
			playcardValue = 2;
			playcardCost = 2;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to your settlement";
			deathsound = AudioPlayer.instance.goblin_groan;
			break;

		// GOBLINS 
		// Creatures
		case Card.PlayCard.goblin_archer:
			playcardName = "Archer";
			playCard = PlayCard.goblin_archer;
			playcardArt = "goblin_archer";
			playcardClass = "Goblin";
			playcardValue = 2;
			playcardCost = 1;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to your settlement";
			deathsound = AudioPlayer.instance.goblin_groan;
			break;

		case Card.PlayCard.goblin_druid:
			playcardName = "Druid";
			playCard = PlayCard.goblin_druid;
			playcardArt = "goblin_druid";
			playcardClass = "Goblin";
			playcardValue = 2;
			playcardCost = 1;
			playcardEffect = Effect.summon;
			playcardSummon = PlayCard.animal_wolf;
			playcardShortDescription = "Summon Wolf: " + playcardValue;
			if (playcardValue < 2) {
				playcardDescription = "Summons " + playcardValue + " wolf";
			} else {
				playcardDescription = "Summons " + playcardValue + " wolves";
			}
			deathsound = AudioPlayer.instance.goblin_groan;
			break;

		case Card.PlayCard.goblin_raider:
			playcardName = "Raider";
			playCard = PlayCard.goblin_raider;
			playcardArt = "goblin_raider";
			playcardClass = "Goblin";
			playcardValue = 1;
			playcardCost = 2;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to your settlement";
			deathsound = AudioPlayer.instance.goblin_groan;
			break;
		
		case Card.PlayCard.goblin_scout:
			playcardName = "Scout";
			playCard = PlayCard.goblin_scout;
			playcardArt = "goblin_scout";
			playcardClass = "Goblin";
			playcardValue = 1;
			playcardCost = 1;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to your settlement";
			deathsound = AudioPlayer.instance.goblin_groan;
			break;

		case Card.PlayCard.goblin_warlock:
			playcardName = "Warlock";
			playCard = PlayCard.goblin_warlock;
			playcardArt = "goblin_warlock";
			playcardClass = "Goblin";
			playcardValue = 1;
			playcardCost = 2;
			playcardEffect = Effect.summon;
			playcardSummon = PlayCard.demon_fire;
			playcardShortDescription = "Summon fire demon: " + playcardValue;
			if (playcardValue < 2) {
				playcardDescription = "Summons " + playcardValue + " fire demon";
			} else {
				playcardDescription = "Summons " + playcardValue + " fire demons";
			}
			deathsound = AudioPlayer.instance.goblin_groan;
			break;

		// Events
		case PlayCard.goblin_assassin:
				playcardName = "Assassin";
				playCard = PlayCard.goblin_assassin;
				playcardArt = "goblin_assassin";
				playcardClass = "Goblin";
				trooper = false;
				playcardValue = 1;
				playcardCost = 1;
				playcardEffect = Effect.physical_attack;
				playcardShortDescription = playcardValue + " damage.";
				playcardDescription = "Deals " + playcardValue + " damage to your settlement.\nCannot be directly attacked.";
				deathsound = AudioPlayer.instance.goblin_groan;
				break;

		// DEMONS
		// creatures

		case PlayCard.demon_fire:
			playcardName = "Fire demon";
			playCard = PlayCard.demon_fire;
			playcardArt = "demon_fire";
			playcardClass = "Demon";
			playcardValue = 3;
			playcardCost = 5;
			playcardEffect = Effect.physical_attack;
			playcardShortDescription = playcardValue + " damage.";
			playcardDescription = "Deals " + playcardValue + " damage to your settlement.\nDoes not go away until killed.";
			deathsound = AudioPlayer.instance.goblin_groan;
			permanent = true;
			break;

		default:
			Main.instance.logger.LogError("Incorrect card assignment! No case for the _class!");
			break;
		
		#endregion

		}
	}
}

public class BonusCard {

	#region BonusCard types
	public enum Type { none, government, hunter, wall }
	public Type _type = BonusCard.Type.none;
	#endregion

	#region Stats
	public string bonuscardName;
	public string bonuscardArt = "";
	public string bonuscardClass = "";


	#endregion
	public BonusCard (Type _type) {

		switch (_type) {

		case Type.government:
			break;

		case Type.hunter:
			break;

		case Type.wall:
			break;

		default:
			break;
		}
	}
}