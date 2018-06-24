using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

	public static GUI instance;

	public GameObject backdrop;
	public GameObject seasonIcon;


	public GameObject walls;
	public GameObject walls_background;

	public GameObject enemyBase;
	public GameObject enemyBaseName;

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
	public void RedrawTown() {

		switch (Main.instance.town.walls.level) {
		case 0:
			walls_background.SetActive (false);
			walls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/empty");
			break;
		case 1:
			walls_background.SetActive (true);
			walls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/wall_1");
			break;
		case 2:
			walls_background.SetActive (true);
			walls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/wall_2");
			break;
		case 3:
			walls_background.SetActive (true);
			walls.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/town/wall_3");
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


		enemyBase.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("graphics/board/enemybase/nothing");
		enemyBaseName.GetComponent<Text> ().text = "";
		
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
