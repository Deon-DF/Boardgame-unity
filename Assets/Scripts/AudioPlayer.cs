using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

	public static AudioPlayer instance;

	public AudioSource soundplayer;

	public AudioClip crowd_scream;
	public AudioClip goblin_groan;
	public AudioClip sword_slash;


	public void Awake () {

		if (instance == null) {
			instance = this;
		} else {			
			Debug.LogError ("Another instance of ::AudioPlayer.cs:: is already running!");
		}

	}
}
