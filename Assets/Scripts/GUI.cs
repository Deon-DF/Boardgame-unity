using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public static GUI instance;

	public GameObject enemyBase;
	public GameObject enemyBaseName;

	public GameObject backdrop;
	public GameObject seasonIcon;

	public Canvas CardDisplayCanvas;
	public CardDisplay CardDisplay;

	public Canvas TextDisplayCanvas;
	public Text TextDisplay;
	public float textDisplayTime = 2f;

	public GameObject pHandPile;
	public GameObject oHandPile;
	public GameObject EventPile;

	public Text yearText;
	public Text seasonText;
	public GameObject pDrawDeckAmount;
	public GameObject pDiscardDeckAmount;

	public GameObject oDrawDeckAmount;
	public GameObject oDiscardDeckAmount;

	public GameObject HPAmount;
	public GameObject FortificationAmount;
	public GameObject EnergyAmount;

	public GameObject BoardControlDisabler;
	public GameObject ProgressionCanvas;
	public GameObject ProgressionPanel;

	#region Display text UI

	public void DisplayUIText(string _text, float _time) {
		Main.instance.boardPlayerControl = false;
		TextDisplay.text = _text;
		textDisplayTime = _time;
		StartCoroutine (DisplayUITextIenumerator ());
	}

	public IEnumerator DisplayUITextIenumerator() {
		GUI.instance.TextDisplayCanvas.gameObject.SetActive (true);
		yield return new WaitForSeconds (textDisplayTime);
		GUI.instance.TextDisplayCanvas.gameObject.SetActive (false);
		GUI.instance.TextDisplay.text = "";
		Main.instance.boardPlayerControl = true;
	}

	#endregion

	#region PlayerUI display

	public void RefreshFeedbackUI () {
		pDrawDeckAmount.GetComponent<Text> ().text = Main.instance.pDrawDeck.cards.Count.ToString();
		pDiscardDeckAmount.GetComponent<Text> ().text = Main.instance.pDiscardDeck.cards.Count.ToString();

		oDrawDeckAmount.GetComponent<Text> ().text = Main.instance.oDrawDeck.cards.Count.ToString();
		oDiscardDeckAmount.GetComponent<Text> ().text = Main.instance.oDiscardDeck.cards.Count.ToString();

		HPAmount.GetComponent<Text> ().text = Main.instance.hp + "/" + Main.instance.maxhp;
		FortificationAmount.GetComponent<Text> ().text = Main.instance.fortification.ToString();
		EnergyAmount.GetComponent<Text> ().text = Main.instance.energy + "/" + Main.instance.maxenergy;
	}

	#endregion

	#region TownGraphics
	public void RedrawTown ()
	{

		switch (Main.instance.town.government.level) {
		case 0:
			Structures.instance.government.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/empty");
			break;
		case 1:
			Structures.instance.government.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/government_1");
			break;
		case 2:
			Structures.instance.government.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/government_2");
			break;
		case 3:
			Structures.instance.government.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/government_3");
			break;

		default:
			break;
		}

		switch (Main.instance.town.hunter.level) {
		case 0:
			Structures.instance.hunter.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/empty");
			break;
		case 1:
			Structures.instance.hunter.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/hunter_1");
			break;
		case 2:
			Structures.instance.hunter.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/hunter_2");
			break;

		default:
			break;
		}

		switch (Main.instance.town.smithy.level) {
		case 0:
			Structures.instance.smithy.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/empty");
			break;
		case 1:
			Structures.instance.smithy.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/smithy_1");
			break;
		case 2:
			Structures.instance.smithy.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/smithy_2");
			break;

		default:
			break;
		}

		switch (Main.instance.town.tavern.level) {
		case 0:
			Structures.instance.tavern.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/empty");
			break;
		case 1:
			Structures.instance.tavern.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/tavern_1");
			break;
		case 2:
			Structures.instance.tavern.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/tavern_2");
			break;

		default:
			break;
		}

		switch (Main.instance.town.trader.level) {
		case 0:
			Structures.instance.trader.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/empty");
			break;
		case 1:
			Structures.instance.trader.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/trader_1");
			break;

		default:
			break;
		}

		switch (Main.instance.town.walls.level) {
		case 0:
			Structures.instance.walls_background.SetActive (false);
			Structures.instance.walls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/empty");
			break;
		case 1:
			Structures.instance.walls_background.SetActive (true);
			Structures.instance.walls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/wall_1");
			break;
		case 2:
			Structures.instance.walls_background.SetActive (true);
			Structures.instance.walls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/wall_2");
			break;
		case 3:
			Structures.instance.walls_background.SetActive (true);
			Structures.instance.walls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/wall_3");
			break;

		default:
			break;
		}

		switch (Main.instance.town.warehouse.level) {
		case 0:
			Structures.instance.warehouse.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/empty");
			break;
		case 1:
			Structures.instance.warehouse.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/warehouse");
			break;

		default:
			break;
		}
	}

	#endregion

	// Use this for initialization
	public void Awake () {

		if (instance == null) {
			instance = this;
		} else {			
			Debug.LogError ("Another instance of ::GUI.cs:: is already running!");
		}

		Cursor.SetCursor (Resources.Load<Texture2D> ("graphics/ui/cursor"), Vector2.zero, CursorMode.Auto);


		GUI.instance.enemyBase.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/enemybase/nothing");
		GUI.instance.enemyBaseName.GetComponent<Text> ().text = "";
		
	}

	public void Start () {
		yearText.text = "Year " + Main.instance.year.ToString();
		RedrawTown();
	}

	public void Update () {

		Main.instance.verificator.DebugListDeck ();
		Main.instance.verificator.DebugNextSeason ();
		Main.instance.verificator.DebugIncreaseWalls ();

		RefreshFeedbackUI ();

		if (Main.instance.boardPlayerControl) {
			if (BoardControlDisabler.activeInHierarchy) {
				BoardControlDisabler.SetActive (false);
			}
		} else {
			if (!BoardControlDisabler.activeInHierarchy) {
				BoardControlDisabler.SetActive (true);
			}
		}
	}
}
