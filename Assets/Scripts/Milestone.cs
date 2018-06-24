using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Milestone : MonoBehaviour {

	public GameObject milestoneText;
	public GameObject milestoneDate;

	public string milestoneTextString = "";
	public string milestoneDateString = "";
	
	// Update is called once per frame
	void Update () {
		if (this.gameObject.activeInHierarchy) {
			milestoneText.GetComponent<Text> ().text = milestoneTextString;
			milestoneDate.GetComponent<Text> ().text = milestoneDateString;
		}
		
	}
}
