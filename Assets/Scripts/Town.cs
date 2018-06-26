using System.Collections;
using System.Collections.Generic;

public class TownHouse {

	public string name = "upgrade_placeholder";
	int _level = 0;
	public int maxlevel = 1;

	public enum Upgrade { none, government, hunter, smithy, tavern, trader, walls, warehouse }
	public Upgrade type = TownHouse.Upgrade.none;

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


	public void SetName ()
	{
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

		case Upgrade.hunter:
			switch (level) {
			case 0:
				name = "No hunter";
				break;
			case 1:
				name = "Hunter's hut";
				break;
			case 2:
				name = "Hunting lodge";
				break;
			default:
				Main.instance.logger.LogError ("SETNAME: Cannot set the name of Upgrade " + this.name + " to a proper level; the level is out of defined range!");
				break;
			}
			break;

		case Upgrade.smithy:
			switch (level) {
			case 0:
				name = "No blacksmith";
				break;
			case 1:
				name = "Blacksmith";
				break;
			case 2:
				name = "Armory";
				break;
			default:
				Main.instance.logger.LogError ("SETNAME: Cannot set the name of Upgrade " + this.name + " to a proper level; the level is out of defined range!");
				break;
			}
			break;

		case Upgrade.tavern:
			switch (level) {
			case 0:
				name = "No tavern";
				break;
			case 1:
				name = "Bar";
				break;
			case 2:
				name = "Tavern";
				break;
			default:
				Main.instance.logger.LogError ("SETNAME: Cannot set the name of Upgrade " + this.name + " to a proper level; the level is out of defined range!");
				break;
			}
			break;

		case Upgrade.trader:
			switch (level) {
			case 0:
				name = "No trader";
				break;
			case 1:
				name = "Trading post";
				break;
			default:
				Main.instance.logger.LogError ("SETNAME: Cannot set the name of Upgrade " + this.name + " to a proper level; the level is out of defined range!");
				break;
			}
			break;

		case Upgrade.walls:
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

		case Upgrade.warehouse:
			switch (level) {
			case 0:
				name = "No warehouse";
				break;
			case 1:
				name = "Warehouse";
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

	public TownHouse government;
	public TownHouse hunter;
	public TownHouse smithy;
	public TownHouse tavern;
	public TownHouse trader;
	public TownHouse walls;
	public TownHouse warehouse;

	public Settlement() {
		government = new TownHouse ();
		government.type = TownHouse.Upgrade.government;
		government.name = "Government";
		government.level = 0;
		government.maxlevel = 3;

		hunter = new TownHouse ();
		hunter.type = TownHouse.Upgrade.government;
		hunter.name = "Hunter";
		hunter.level = 0;
		hunter.maxlevel = 2;

		smithy = new TownHouse ();
		smithy.type = TownHouse.Upgrade.smithy;
		smithy.name = "Smithy";
		smithy.level = 0;
		smithy.maxlevel = 2;

		tavern = new TownHouse ();
		tavern.type = TownHouse.Upgrade.tavern;
		tavern.name = "Tavern";
		tavern.level = 0;
		tavern.maxlevel = 2;

		trader = new TownHouse ();
		trader.type = TownHouse.Upgrade.smithy;
		trader.name = "Trader";
		trader.level = 0;
		trader.maxlevel = 1;

		walls = new TownHouse ();
		walls.type = TownHouse.Upgrade.walls;
		walls.name = "Walls";
		walls.level = 0;
		walls.maxlevel = 3;

		warehouse = new TownHouse ();
		warehouse.type = TownHouse.Upgrade.smithy;
		warehouse.name = "Warehouse";
		warehouse.level = 0;
		warehouse.maxlevel = 1;


	}
}
