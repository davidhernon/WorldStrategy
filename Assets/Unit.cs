// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using UnityEngine;

	public class Unit 
	{
		public int hex_coordinates_x;
		public int hex_coordinates_y;
		public Vector3 vector;
		public Hex on_hex;
		List<Hex> path = new List<Hex> ();
		public string name = "Tribesman";
		public int moves = 0;
		public int max_moves = 2;

		

		public GameObject unit_marker;

		public Unit ()
		{
			
			this.unit_marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.unit_marker.transform.position = new Vector3(-1000,-1000,0);
			this.unit_marker.GetComponent<Renderer>().material.color = ColorGenerator.getColorFromString ("unit_marker_basic");
			
		}

		public Unit (Hex hex)
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
	}

	public void move(Vector3 vector, Hex hex)
	{
		on_hex.unit = null;
		unit_marker.transform.position = vector;
		this.on_hex = hex;
		hex.unit = this;
	}

	public void move(Hex hex){


		int distance = (int)Vector2.Distance (on_hex.pos, hex.pos);
		Debug.Log ("distance: " + distance);
		int temp_move = Mathf.RoundToInt(moves -  distance);
		if (temp_move < 0) {
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
	}

	public void move (){}

	public string toString()
	{
		return "Unit on: " + this.hex_coordinates_x + " " + this.hex_coordinates_y + " with vector: " + this.vector;
	}

	public string getName(){
		return this.name;
	}

	public string getInfo(){
		return "" + this.getName () + "\nMoves: " + this.moves;
	}

}


