using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structures : MonoBehaviour {
	public static Structures instance;


	public GameObject government;
	public GameObject hunter;
	public GameObject smithy;
	public GameObject tavern;
	public GameObject trader;
	public GameObject walls;
	public GameObject walls_background;
	public GameObject warehouse;

	public void Awake () {

		if (instance == null) {
			instance = this;
		} else {			
			Debug.LogError ("Another instance of ::STRUCTURES.cs:: is already running!");
		}
		
	}
}
