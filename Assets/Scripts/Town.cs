using System.Collections;
using System.Collections.Generic;

public class Town {

	public string name = "upgrade_placeholder";
	int _level = 0;
	public int maxlevel = 1;

	public enum Upgrade { none, government, walls }
	public Upgrade type = Town.Upgrade.none;

	public int level {
		get {
			return _level;
		}

		set {
			if (value > maxlevel) {
				Main.instance.logger.LogWarning ("UPGRADE: Tried to set the level of " + this.name + " higher than its maxlevel; setting to maxlevel.");
				_level = maxlevel;
				SetName ();
				GUI.instance.RedrawTown ();
			} else {
				_level = value;
				SetName ();
				if (GUI.instance != null) {
					GUI.instance.RedrawTown ();
				}
				Main.instance.logger.LogInfo ("UPGRADE: The level of " + this.name + " is now " + _level);
			}
		}
	}


	public void SetName () {
		switch (type) {

		case Upgrade.government:
			switch (level) {
			case 0:
				name = "No government";
				break;
			case 1:
				name = "Elder's hut";
				break;
			case 2:
				name = "Town hall";
				break;
			default:
				Main.instance.logger.LogError ("SETNAME: Cannot set the name of Upgrade " + this.name + " to a proper level; the level is out of defined range!");
				break;
			}
			break;

		case (Upgrade.walls):
			switch (level) {
			case 0:
				name = "No walls";
				break;
			case 1:
				name = "Earth rampart";
				break;
			case 2:
				name = "Palisade";
				break;
			case 3:
				name = "Stone walls";
				break;

			default:
				Main.instance.logger.LogError ("SETNAME: Cannot set the name of Upgrade " + this.name + " to a proper level; the level is out of defined range!");
				break;
			}
			break;

		default:
			Main.instance.logger.LogError ("SETNAME: Cannot set name of " + this.name + " to a proper level; the type is not defined.");
			break;

		}

		Main.instance.logger.LogInfo ("The upgrade is now named: " + this.name);
	}
}


public class Settlement {

	public Town government;
	public Town walls;

	public Settlement() {
		government = new Town ();
		government.type = Town.Upgrade.government;
		government.name = "Government";
		government.level = 0;
		government.maxlevel = 2;


		walls = new Town ();
		walls.type = Town.Upgrade.walls;
		walls.name = "Walls";
		walls.level = 0;
		walls.maxlevel = 3;

	}
}
