using System;
using UnityEngine;
public class GameUtils
{

	public static Vector2 getRandomStartLocation(Hex[,] map, int rows, int cols)
	{
		int i, j = 0;
		
		i = UnityEngine.Random.Range(0,rows-1);
		j = UnityEngine.Random.Range(0,cols-1);

		while(map[i,j].type == "shallow_water" || map[i,j].type == "deep_water"){
			i = UnityEngine.Random.Range(0,rows-1);
			j = UnityEngine.Random.Range(0,cols-1);
		}

		return new Vector2(i,j);
	}

	public static Hex getHexFromPoint(Vector3 world, Map map, int num_row, int num_col){
		for(int i=0; i < num_row; i++){
			for(int j=0; j < num_col; j++){
				if(map.terrain[i,j].inBoundingBox(world)){
					return map.terrain[i,j];
				}
			}
		}
		return null;
	}

	public static Vector2 getHexMapCoordinatesFromPoint(Map map, Vector3 world, int num_row, int num_col){
		for(int i=0; i < num_row; i++){
			for(int j=0; j < num_col; j++){
				if(map.terrain[i,j].inBoundingBox(world)){
					return new Vector2(i,j);
				}
			}
		}
		return new Vector2();
	}
}
