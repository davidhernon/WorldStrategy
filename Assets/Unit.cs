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
using UnityEngine;

	public class Unit
	{
		int hex_coordinates_x;
		int hex_coordinates_y;
		Vector3 vector;
		Hex on_hex;

		public GameObject unit_marker;
		public Unit ()
		{
			this.unit_marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.unit_marker.transform.position = new Vector3(-1000,-1000,0);
			this.unit_marker.GetComponent<Renderer>().material.color = ColorGenerator.getColorFromString ("unit_marker_basic");
		}

		public Unit (Vector3 vector)
		{
			this.unit_marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
			this.unit_marker.transform.position = vector;
			this.vector = vector;
			this.unit_marker.GetComponent<Renderer>().material.color = ColorGenerator.getColorFromString ("unit_marker_basic");

			Vector2 hex_coordinates = GameUtils.getHexMapCoordinatesFromPoint (GameEngine.map, 	new Vector3(vector.x, vector.y + 1.0f, vector.z), GameEngine.num_row, GameEngine.num_col);
			this.hex_coordinates_x = Mathf.RoundToInt(hex_coordinates.x);
			this.hex_coordinates_y = Mathf.RoundToInt(hex_coordinates.y);
			Debug.Log ("created unit at: "+this.hex_coordinates_x + " " + this.hex_coordinates_y);
			this.vector = vector;


			GameEngine.map.terrain [hex_coordinates_x, hex_coordinates_y].setUnit (this);

			on_hex = GameEngine.map.terrain [this.hex_coordinates_x, this.hex_coordinates_y];

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

	public void move(Vector3 vector, Hex hex)
	{
		on_hex.unit = null;
		unit_marker.transform.position = vector;
		this.on_hex = hex;
		hex.unit = this;


	}

	public string toString()
	{
		return "Unit: " + this.hex_coordinates_x + " " + this.hex_coordinates_y + " with vector: " + this.vector;
	}

}


