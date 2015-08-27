using UnityEngine;
using System.Collections;

public abstract class Player
{
	public string playerName = "";
	public bool is_players_turn = false;
	public Unit[] units;

	public Player (string name) {
		this.playerName = name;
	}

	string getPlayerName ()
	{
		return this.playerName;
	}

	public void endPlayersTurn()
	{
		is_players_turn = false;
		Debug.Log (this.playerName + " Turn was Ended");
	}

	public void startPlayersTurn()
	{
		is_players_turn = true;
		Debug.Log (this.playerName + " Turn was Started");
	}
	
	public abstract void move();
	public abstract void setupUnits(Map map, Vector2 vector);
}