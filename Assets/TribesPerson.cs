﻿using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using UnityEngine;

public class Tribesperson  : Unit 
{
	public int hex_coordinates_x;
	public int hex_coordinates_y;
	public Vector3 vector;
	public Hex on_hex;
	List<Hex> path = new List<Hex> ();
	public int id = -1;
	
	public GameObject unit_marker;
	
	public Player player;
	
	public List<Hex> hexes_unit_can_see = new List<Hex>();
	
	public Tribesperson ()
	{
		
		this.unit_marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.unit_marker.transform.position = new Vector3(-1000,-1000,0);
		this.unit_marker.GetComponent<Renderer>().material.color = ColorGenerator.getColorFromString ("unit_marker_basic");
		
	}
	
	public Tribesperson (Hex hex)
	{
		this.unit_marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.unit_marker.transform.position = hex.center;
		this.vector = hex.center;
		this.unit_marker.GetComponent<Renderer>().material.color = ColorGenerator.getColorFromString ("unit_marker_basic");
		
		//			Vector2 hex_coordinates = GameUtils.getHexMapCoordinatesFromPoint (GameEngine.map, 	new Vector3(vector.x, vector.y + 1.0f, vector.z), GameEngine.num_row, GameEngine.num_col);
		//			Hex hexAtPoint = GameUtils.getHexFromPoint (vector, GameEngine.map, GameEngine.num_row, GameEngine.num_col);
		//			Debug.Log ("pos: "  + " coords: " + hex_coordinates);
		this.hex_coordinates_x = Mathf.RoundToInt(hex.pos.x);
		this.hex_coordinates_y = Mathf.RoundToInt(hex.pos.y);
		
		GameEngine.map.terrain [hex_coordinates_x, hex_coordinates_y].setUnit (this);
		
		this.on_hex = hex;
		GameUtils.destroyCollider (this.unit_marker);

		// STATS
		name = "Tribesperson";
		moves = 0;
		max_moves = 2;
		
		
		health = 10;
		health_recovery_per_turn = 3;
		hunger = 0;
		hunger_per_turn = 1;
		max_health = 10;
		
		strength = 8;
		defend = 3;
	}
	
	public void move(Vector3 vector)
	{
		GameEngine.map.terrain [this.hex_coordinates_x, this.hex_coordinates_y].unit = null;
		this.unit_marker.transform.position = vector;
		Vector2 hex_coordinates = GameUtils.getHexMapCoordinatesFromPoint (GameEngine.map, vector, GameEngine.num_row, GameEngine.num_col);
		Debug.Log ("hex coords received" + hex_coordinates);
		this.hex_coordinates_x = Mathf.RoundToInt(hex_coordinates.x);
		this.hex_coordinates_y = Mathf.RoundToInt(hex_coordinates.y);
		GameEngine.map.terrain [this.hex_coordinates_x, this.hex_coordinates_y].setUnit (this);
		
		
	}
	
	public void resetMove(){
		this.moves = max_moves;
		this.hunger += hunger_per_turn;
		this.health += health_recovery_per_turn;
		if (this.health > this.max_health) {
			this.health = this.max_health;
		}
	}
	
	public void move(Vector3 vector, Hex hex)
	{
		on_hex.unit = null;
		unit_marker.transform.position = vector;
		this.on_hex = hex;
		hex.unit = this;
		
	}
	
	public void move(Hex hex){
		if (on_hex == hex) {
			return;
		}
		int distance = (int)Vector2.Distance (on_hex.pos, hex.pos);
		int temp_move = Mathf.RoundToInt(moves -  distance);
		if (temp_move < 0) {
			return;
		}
		
		if (hex.hasUnit ()) {
			this.attack(hex);
			return;
		}
		
		this.moves = temp_move;
		if (this.moves < 0)
			moves = 0;
		
		List<Hex> path = Astar.findPath (on_hex, hex);
		while (path==null) {
		}
		this.unit_marker.transform.position = hex.center;
		this.on_hex.unit = null;
		this.on_hex = hex;
		hex.unit = this;
		player.discoveredHex (hex);
		GameEngine.selected_hex = hex;
		
		canSeeHex (hex);
	}
	
	public void canSeeHex(Hex hex){
		
		foreach(Hex temp in hexes_unit_can_see){
			if(hex.hasUnit () && hex != temp){
				temp.unit.invisible();
			}
		}
		
		hexes_unit_can_see = new List<Hex>();
		//		if(hex.hasUnit ()){
		hex.unit.visible ();
		//		}
		hexes_unit_can_see.Add(hex);
		
		foreach (Hex neighbor in hex.getNeighbors()) {
			if(neighbor.hasUnit ()){
				Debug.Log ("unit!");
				neighbor.unit.invisible();
			}
			hexes_unit_can_see.Add(neighbor);
			foreach(Hex neighbor2 in neighbor.getNeighbors()){
				hexes_unit_can_see.Add(neighbor2);
				if(neighbor2.hasUnit () && neighbor2 != hex){
					Debug.Log ("unit2!");
					neighbor2.unit.invisible();
				}
			}
		}
		
	}
	
	public void attack(Hex hex){
		hex.unit.health -= (this.strength-hex.unit.defend);
		this.health -= hex.unit.strength - this.defend;
		
		Debug.Log ("" + this.name + " attacks " + hex.unit.name + " with Strength " + this.strength + " against " + hex.unit.strength);
		
		if (hex.unit.health < 0) {
			hex.unit.killed();
			this.move (hex);
		}
		if (this.health < 0) {
			this.killed();
		}
		
	}
	
	public void killed(){
		GameEngine.Destroy (unit_marker);
		player.removeUnit(this);
	}
	
	public string toString()
	{
		return "Unit on: " + this.hex_coordinates_x + " " + this.hex_coordinates_y + " with vector: " + this.vector;
	}
	
	public string getName(){
		return this.name;
	}
	
	public string getInfo(){
		return "" + this.getName () + "\nMoves: " + this.moves + "\nHealth: " + this.health;
	}
	
	public void invisible(){
		//				unit_marker.SetActive (false);
		unit_marker.GetComponent<MeshRenderer>().enabled = false;
	}
	
	public void visible(){
		//				unit_marker.SetActive (true);
		unit_marker.GetComponent<MeshRenderer>().enabled = true;
	}

}