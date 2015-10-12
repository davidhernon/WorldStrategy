using UnityEngine;
using System.Collections.Generic;

public abstract class Player
{
	public string playerName = "";
	public bool is_players_turn = false;
	public Unit[] units;
	public int available;
	public int unit_counter = 0;
	public Culture culture;
	public Hex[,] discovered;
	public Color[] colors;

	public Player (string name) {
		this.playerName = name;
		available = 0;
		units = new Unit[0];
		discovered = new Hex[GameEngine.num_row, GameEngine.num_col];
		colors = (Color[])World7.mesh.colors.Clone ();
		culture = new Culture ();
	}

	string getPlayerName ()
	{
		return this.playerName;
	}

	public void endPlayersTurn()
	{
		is_players_turn = false;
		move ();
	}

	public void startPlayersTurn()
	{
		is_players_turn = true;
		for (int i=0; i < units.Length; i++) {
			units[i].resetMove();
		}
	}
	public abstract void discoveredHex(Hex hex);
	public abstract void removeUnit(Unit unit);
	public abstract void move();
	public abstract void setupUnits(Map map, Hex hex);
	public abstract void setupUnits(Map map);

}