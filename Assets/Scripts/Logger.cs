using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Logger {

	#region Logging methods

	public void LogDirExists() {
		if (!System.IO.Directory.Exists(Application.dataPath + System.IO.Path.GetDirectoryName(Info.gamelog_path))) {
			Directory.CreateDirectory(Info.gamelog_path);
		}
	}

	public void Log (string _text) {
		LogDirExists ();
		StreamWriter gamelog = new StreamWriter(Info.gamelog_path + "gamelog.txt", true);
		Debug.Log (_text);
		gamelog.WriteLine(_text);
		gamelog.Close();
	}

	public void LogText(string _text) {
		LogDirExists ();
		StreamWriter gamelog = new StreamWriter(Info.gamelog_path + "gamelog.txt", true);
		Debug.Log("[" + System.DateTime.Now + "]" + _text);
		gamelog.WriteLine("[" + System.DateTime.Now + "]" + _text);
		gamelog.Close();
	}

	public void LogError(string _error) {
		if (Main.instance.logErrors) {
			LogDirExists ();
			StreamWriter gamelog = new StreamWriter (Info.gamelog_path + "gamelog.txt", true);
			Debug.LogError ("[" + System.DateTime.Now + "]" + " Error> " + _error);
			gamelog.WriteLine ("[" + System.DateTime.Now + "]" + " Error> " + _error);
			gamelog.Close ();
		}
	}
	public void LogInfo(string _info) {
		if (Main.instance.logInfo) {
			LogDirExists ();
			StreamWriter gamelog = new StreamWriter (Info.gamelog_path + "gamelog.txt", true);
			Debug.Log ("[" + System.DateTime.Now + "]" + " Info> " + _info);
			gamelog.WriteLine ("[" + System.DateTime.Now + "]" + " Info> " + _info);
			gamelog.Close ();
		}
	}
	public void LogUI(string _info) {
		if (Main.instance.logUiActions) {
			LogDirExists ();
			StreamWriter gamelog = new StreamWriter (Info.gamelog_path + "gamelog.txt", true);
			Debug.Log ("[" + System.DateTime.Now + "]" + " UI> " + _info);
			gamelog.WriteLine ("[" + System.DateTime.Now + "]" + " UI> " + _info);
			gamelog.Close ();
		}
	}
	public void LogWarning(string _warning) {
		if (Main.instance.logWarnings) {
			LogDirExists ();
			StreamWriter gamelog = new StreamWriter (Info.gamelog_path + "gamelog.txt", true);
			Debug.LogWarning ("[" + System.DateTime.Now + "]" + " Warning> " + _warning);
			gamelog.WriteLine ("[" + System.DateTime.Now + "]" + " Warning> " + _warning);
			gamelog.Close ();
		}
	}

	#endregion

	#region Application start

	public void StartLog() {
		Log ("====================================================================================================================");
		Log (" Starting new instance of the game " + Info.projectname);
		Log (" Device: " + SystemInfo.deviceType + ", Model: " + SystemInfo.deviceModel + ", Hostname: " + SystemInfo.deviceName);
		Log ("====================================================================================================================");
	}

	#endregion

}
