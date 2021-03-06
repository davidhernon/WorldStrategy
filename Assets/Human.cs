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

public class Human : Player
{

	public Human (string name) : base(name)
	{
		this.available = 1;
	}

	override public void move()
	{

		while(this.is_players_turn)
		{

		}

	}

	override public void setupUnits(Map map, Hex hex)
	{
		this.units = new Unit[1];
		Unit new_unit = new Unit (hex);
		this.units [0] = new_unit;
		hex.setUnit((Unit)this.units [0]);
		available--;
		unit_counter++;
		this.units [0].id = unit_counter;
		this.units [0].player = this;
		this.discoveredHex (hex);
	}

	override public void setupUnits(Map map){

	}

	override public void removeUnit(Unit unit){
		this.units = new Unit[0];
		available++;
	}

	override public void discoveredHex(Hex hex){
		Color[] c = (Color[])World7.mesh.colors.Clone ();
		if (discovered [(int)hex.pos.x, (int)hex.pos.y] == null) {
			for (int i=0; i<7; i++) {
				//			c [i] = ColorGenerator.getColorFromString(hex.type);
				Color cl = ColorGenerator.getColorFromString (hex.type);
				c [hex.id_list [i]] = cl;
			}
			discovered [(int)hex.pos.x, (int)hex.pos.y] = hex;
		}

//		Hex[] neighb = hex.getNeighbors ();
//		for (int j=0; j<neighb.Length; j++) {
		foreach (Hex neighbor in hex.getNeighbors()) {
			if (discovered [(int)neighbor.pos.x, (int)neighbor.pos.y] == null) {
				discovered[(int)neighbor.pos.x, (int)neighbor.pos.y] = neighbor;
				for (int i=0; i<7; i++) {
					Color cl = ColorGenerator.getColorFromString(neighbor.type);
					c[neighbor.id_list[i]] = cl;
				}
			}
			foreach(Hex neighbor2 in neighbor.getNeighbors()){
				discovered[(int)neighbor2.pos.x, (int)neighbor2.pos.y] = neighbor;
				for (int i=0; i<7; i++) {
					Color cl = ColorGenerator.getColorFromString(neighbor2.type);
					c[neighbor2.id_list[i]] = cl;
				}
			}

		}
//		}?
		World7.mesh.colors = c;
	}

}