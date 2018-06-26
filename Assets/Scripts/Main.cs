using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Main : MonoBehaviour {

	public static Main instance;
	//private string projectname = "Boardgame";

	#region Comments for the project

	//	Naming conventions:
	//	- preceeded by "p" - player-related, example pDrawPile
	//	- preceeded by "o" - opponent-related, example oDrawPile
	// card - logical unit with stats
	// deck - logical list of cards
	// playcard - gameobject representing a card
	// pile - gameobject representing a deck

	#endregion

	#region Logging settings

	public Logger logger;
	public bool logInfo = true;
	public bool logErrors = true;
	public bool logUiActions = false;
	public bool logWarnings = true;

	#endregion

	#region Service objects and functions

	public Verificator verificator;
	public bool boardPlayerControl = true;
	public int damageDealtLastTurn = 0;

	public Deck pDrawDeck = new Deck();
	public Deck pDiscardDeck = new Deck();
	public Deck pHandDeck = new Deck();

	public Deck oDrawDeck = new Deck();
	public Deck oDiscardDeck = new Deck();
	public Deck oHandDeck = new Deck();

	public Deck EventDeck = new Deck();
	public Deck SummonDeck = new Deck();


	public List<Deck> all_decks = new List<Deck> ();

	/* Uncomment this if in need of reversing list elements
	 
	public List<Card> ReverseList(List<Card> _list) {
		List<Card> reversedList = new List<Card>();

		for (int i = 0; i < _list.Count; i++) {
			reversedList.Add(_list[_list.Count - i - 1]);
		}

		return reversedList;
	}
	*/

	#endregion

	#region engine

	public void ExitGame() {
		Application.Quit();
	}

	#endregion

	#region Seasons

	public enum Season { dawn, ember, dusk, frost }
	Season _season = Season.dawn;
	int _year = 1;

	public int year {
		get {
			return _year;
		}
		set {
			_year = value;
			GUI.instance.yearText.text = "Year " + _year;
		}
	}

	public Season season {
		get {
			return _season;
		}
		set {
			_season = value;
			switch (value) {
			case Season.dawn:
				GUI.instance.seasonText.text = "Dawn";
				GUI.instance.backdrop.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/backdrops/dawn");
				GUI.instance.seasonIcon.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/ui/season_dawn");
				break;
			case Season.ember:
				GUI.instance.seasonText.text = "Ember";
				GUI.instance.backdrop.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/backdrops/ember");
				GUI.instance.seasonIcon.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/ui/season_ember");
				break;
			case Season.dusk:
				GUI.instance.seasonText.text = "Dusk";
				GUI.instance.backdrop.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/backdrops/dusk");
				GUI.instance.seasonIcon.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/ui/season_dusk");
				break;
			case Season.frost:
				GUI.instance.seasonText.text = "Frost";
				GUI.instance.backdrop.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/backdrops/frost");
				GUI.instance.seasonIcon.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/ui/season_frost");
				break;
			}
		}
	}

	public void NextSeason() {
		Main.instance.logger.LogUI("MAIN::NEXTSEASON: DISABLING PLAYER CONTROL!");
		boardPlayerControl = false;
		GUI.instance.enemyBase.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/enemybase/nothing");
		GUI.instance.enemyBaseName.GetComponent<Text> ().text = "";
		int x = (int)season;
		if (x < 3) {
			x += 1;
		} else {
			x = 0;
			year += 1;
		}
		season = (Season)x;
		StartCoroutine (FinishSeason());
	}

	public IEnumerator FinishSeason() {
		if (damageDealtLastTurn > 0) {
			AudioPlayer.instance.soundplayer.PlayOneShot (AudioPlayer.instance.crowd_scream);

			GUI.instance.DisplayUIText("You have lost " + damageDealtLastTurn + " families.\n\n You survived the season. Next season: " + season, 5f);
			Main.instance.logger.LogUI("MAIN::FINISHSEASON: DISABLING PLAYER CONTROL!");
			boardPlayerControl = false;
		} else {
			GUI.instance.DisplayUIText("You survived the season. Next season: " + season, 5f);
			Main.instance.logger.LogUI("MAIN::FINISHSEASON: DISABLING PLAYER CONTROL!");
			boardPlayerControl = false;
		}

		if (oHandDeck.cards.Count > 0) {
			oHandDeck.cards.Clear ();
		}
		if (oDrawDeck.cards.Count > 0) {
			oDrawDeck.cards.Clear ();
		}
		if (oDiscardDeck.cards.Count > 0) {
			oDiscardDeck.cards.Clear ();
		}
		if (EventDeck.cards.Count > 0) {
			EventDeck.cards.Clear ();
		}
		FinishTurn ();

		yield return new WaitForSeconds (5f);

		// TODO GENERATE REWARD AND A CHOICE OF NEW PROGRESSION
		verificator.DebugEnemyDeck ();
		Main.instance.oDrawDeck.Shuffle();

		StartTurn ();
	}

	#endregion Seasons

	#region Settlement stats

	public int hp = 10;
	public int maxhp = 10;
	public int fortification = 0;
	public int energy = 0;
	public int maxenergy = 3;

	public int pDrawAmount = 5;
	public int pHandSize = 7;

	public Settlement town;

	#endregion

	#region Opponent stats

	public int oDrawAmount = 2;
	public int oHandSize = 7;

	#endregion

	#region Inspector-attached gameobjects

	/*
	//	- Player draw pile
	public GameObject pDrawPile;
	//	- Player discard pile
	public GameObject pDiscardPile;
	//	- Player hand location
	public GameObject pHandPile;
	//	- Opponent draw pile
	public GameObject oDrawPile;
	//	- Opponent discard pile
	public GameObject oDiscardPile;
	//	- Opponent hand location
	public GameObject oHandPile;
	*/


	public GameObject card_entity;
	public GameObject card_opponent_entity;

	#endregion

	#region Spawn card/icon  graphic

	public void SpawnCard (Card _card, GameObject pile) {
		GameObject cardObject = Instantiate (card_entity);
		cardObject.transform.SetParent(pile.transform);
		cardObject.GetComponent<BoardGameObject> ().assignedCard = _card;
		cardObject.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		cardObject.GetComponent<BoardGameObject> ().playerobject = true;
	}

	public void SpawnOpponentCard (Card _card, GameObject pile) {
		GameObject cardObject = Instantiate (card_opponent_entity);
		cardObject.transform.SetParent(pile.transform);
		cardObject.GetComponent<BoardGameObject> ().assignedCard = _card;
		cardObject.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
		cardObject.GetComponent<BoardGameObject> ().playerobject = false;
		cardObject.transform.SetAsFirstSibling ();
	}

	#endregion


	#region Initialization

	private void Initialize() {
		InitializeMain ();
		InitializeDecks ();
	}

	private void InitializeMain() {
		logger = new Logger ();
		logger.StartLog ();
				
		if (instance == null) {
			instance = this;
			logger.LogInfo ("Main gameObject is initialized.");
		} else {			
			logger.LogError ("Another instance of ::Main.cs:: is already running!");
		}

		verificator = new Verificator ();
		if (verificator == null) {
			logger.LogError ("Verificator is not initialized!");
		} else {
			logger.LogInfo ("Verification system is started.");
		}

		_year = 1;
		_season = Season.dawn;
		town = new Settlement ();


	}

	private void InitializeDecks() {
		pDrawDeck.deckName = "Player draw pile";
		pDiscardDeck.deckName = "Player discard pile";
		pHandDeck.deckName = "Player hand";

		oDrawDeck.deckName = "Opponent Draw pile";
		oDiscardDeck.deckName = "Opponent Discard pile";
		oHandDeck.deckName = "Opponent Hand";
		EventDeck.deckName = "Event hand";
		SummonDeck.deckName = "Summon deck";

		all_decks.Add (pDrawDeck);
		all_decks.Add (pDiscardDeck);
		all_decks.Add (pHandDeck);

		all_decks.Add (oDrawDeck);
		all_decks.Add (oDiscardDeck);
		all_decks.Add (oHandDeck);
		all_decks.Add (EventDeck);
	}

	public void Awake () {
		Initialize ();

	}

	public void Start () {
		// Check that the board layout is correct
		//verificator.CheckBoardLayout();

		// Set season
		season = Season.dawn;

		// Generate a choice of starting opponent decks
		//TODO

		// Create the starting deck for opponent
		verificator.DebugEnemyDeck();
		// Create the starting deck for player
		verificator.DebugPlayerDeck();

		// Shuffle decks
		oDrawDeck.Shuffle ();
		pDrawDeck.Shuffle ();
		RefreshStats ();
		DealCards ();

	}

	#endregion

	#region Game turn mechanics

	void ActivateMonsterCards () {
		int monstercards = oHandDeck.cards.Count;
		int eventcards = EventDeck.cards.Count;
		damageDealtLastTurn = 0;

		for (int i = 0; i < monstercards; i++) {
			GUI.instance.oHandPile.transform.GetChild(monstercards - i - 1).GetComponent<BoardGameObject>().assignedCard.Play();
			if (!GUI.instance.oHandPile.transform.GetChild (monstercards - i - 1).GetComponent<BoardGameObject> ().assignedCard.permanent) {
				Main.instance.logger.LogInfo ("ACTIVATEMONSTERCARDS: Destroying GameObject (monster) " + GUI.instance.oHandPile.transform.GetChild (monstercards - i - 1).GetComponent<BoardGameObject> ().assignedCard.playcardName);
				Destroy (GUI.instance.oHandPile.transform.GetChild (monstercards - i - 1).gameObject);
			}
		}

		for (int i = 0; i < eventcards; i++) {
			//Debug.LogWarning (eventcards + " " + GUI.instance.EventPile.transform.GetChild (eventcards - i - 1).GetComponent<BoardGameObject> ().assignedCard.playcardName);
			GUI.instance.EventPile.transform.GetChild(eventcards - i - 1).GetComponent<BoardGameObject>().assignedCard.Play();
			Main.instance.logger.LogInfo ("ACTIVATEMONSTERCARDS: Destroying GameObject (event) " + GUI.instance.EventPile.transform.GetChild (eventcards - i - 1).GetComponent<BoardGameObject> ().assignedCard.playcardName);
			Destroy(GUI.instance.EventPile.transform.GetChild (eventcards - i - 1).gameObject);
		}
	}

	void DiscardPlayerHand() {
		int iterations = pHandDeck.cards.Count;

		for (int i = 0; i < iterations; i++) {
			Main.instance.logger.LogInfo ("DISCARDPLAYERHAND: Discarding " + GUI.instance.pHandPile.transform.GetChild (iterations - i - 1).GetComponent<BoardGameObject> ().assignedCard.playcardName);
			Destroy(GUI.instance.pHandPile.transform.GetChild(iterations - i - 1).gameObject);
		}
	}

	void HandleLosses() {
		if (damageDealtLastTurn > 0) {
			AudioPlayer.instance.soundplayer.PlayOneShot (AudioPlayer.instance.crowd_scream);
			GUI.instance.DisplayUIText ("You have lost " + damageDealtLastTurn + " families.", 2f);
		}
	}

	void RefreshStats() {
		Main.instance.logger.LogInfo ("REFRESHSTATS: Resetting energy to 0.");
		energy = maxenergy;
		Main.instance.logger.LogInfo ("REFRESHSTATS: Resetting fortification to 0.");
		fortification = 0;

		if (Main.instance.town.walls.level > 0) {
			Main.instance.logger.LogInfo ("REFRESHSTATS: Wall level is: " + Main.instance.town.walls.level + ", adding it to fortification bonus.");
			fortification += Main.instance.town.walls.level;
		}
	}

	void DealCards() {
		// Draw new player cards
		StartCoroutine (DealCardsWithDelay());
	}

	IEnumerator DealCardsWithDelay() {
		Main.instance.logger.LogUI("MAIN::DEALCARDSWITHDELAY: DISABLING PLAYER CONTROL!");
		boardPlayerControl = false;

		if (SummonDeck.cards.Count > 0) {
			int index = SummonDeck.cards.Count;
			for (int i = 0; i < index; i++) {
				yield return new WaitForSeconds (0.1f);
				SummonDeck.OpponentDrawCard ();
			}
		}

		if (oDrawAmount > 0) {
			for (int i = 0; i < oDrawAmount; i++) {
				yield return new WaitForSeconds (0.1f);
				oDrawDeck.OpponentDrawCard ();
			}
		}

		if (pDrawAmount > 0) {
			for (int i = 0; i < pDrawAmount; i++) {
				yield return new WaitForSeconds (0.1f);
				pDrawDeck.DrawCard ();
			}
		}

		Main.instance.logger.LogUI("MAIN::DEALCARDSWITHDELAY: ENABLING PLAYER CONTROL!");
		boardPlayerControl = true;
	}

	#endregion Game turn mechanics

	#region End turn button actions

	void StartTurn() {
		RefreshStats ();
		DealCards ();
	}

	void FinishTurn() {
		Main.instance.logger.LogUI("MAIN::FINISHTURN: DISABLING PLAYER CONTROL!");
		boardPlayerControl = false;
		DiscardPlayerHand ();
		ActivateMonsterCards ();
		HandleLosses ();
	}

	public void EndTurnAction() {


		/// FINISH TURN
		/// 
		FinishTurn();
		// Only win if there's no cards in the opponent's draw deck, hand, event deck or in summon deck
		if (oHandDeck.cards.Count > 0 || oDrawDeck.cards.Count > 0 || EventDeck.cards.Count > 0 || SummonDeck.cards.Count > 0) {			

			StartTurn();

		} else {
			NextSeason ();
		}

	}

	#endregion

}
