using UnityEngine;
using System.Collections;

public abstract class Player
{
	public string playerName = "";
	public bool is_players_turn = false;
	public Unit[] units;
	public int available;

	public Player (string name) {
		this.playerName = name;
		available = 0;
		units = new Unit[0];
	}

	string getPlayerName ()
	{
		return this.playerName;
	}

	public void endPlayersTurn()
	{
		is_players_turn = false;
		move ();
		Debug.Log (this.playerName + " Turn was Ended");
	}

	public void startPlayersTurn()
	{
		is_players_turn = true;
		for (int i=0; i < units.Length; i++) {
			units[i].resetMove();
		}
		Debug.Log (this.playerName + " Turn was Started");
	}
	
	public abstract void move();
	public abstract void setupUnits(Map map, Hex hex);
	public abstract void setupUnits(Map map);
}