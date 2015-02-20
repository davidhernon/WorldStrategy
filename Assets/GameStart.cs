using UnityEngine;
using System.Collections;

public class GameStart {

	public static double avg_dist = 0.0;
	public static int d_count = 0;
	public static bool locations_on = false;

	public static GameObject[] addStartLocation(Hex[,] terrain, int num_row, int num_col, GameObject[] new_cities){
		//GameObject[] new_cities = new GameObject[5];
		
		int i=0;
		int j=0;
		
		double[] distances = new double[5];
		
		for(int k=0; k < 5; k++){
			i = UnityEngine.Random.Range(0,num_row-1);
			j = UnityEngine.Random.Range(0,num_row-1);
			bool satisfied = false;
			int count = 0;
			int half = num_row/2;
			int up = 0;
			while(!satisfied){

				if(count%2==0)
					up = half;
				else
					up = 0;
				
				i = UnityEngine.Random.Range(up,num_row);
				j = UnityEngine.Random.Range(0+k*(num_col/new_cities.Length), (k+1)*(num_col/new_cities.Length));
				while(terrain[i,j].type == "shallow_water" || terrain[i,j].type == "deep_water"){
					i = UnityEngine.Random.Range(up,num_row);
					j = UnityEngine.Random.Range(0+k*(num_col/5), (k+1)*(num_col/5));
				}
				double d = TerrainFunctions.avgValSurroundingTerrain(terrain,i,j,num_row,num_col);
				distances[k] = d;
				avg_dist += d;
				d_count++;
				distances[k] = avg_dist/(1.0*d_count);
				Debug.Log( "value   " +k+" "+ avg_dist/((1.0)*d_count) );
				if(((farFromCities(k, i, j, 0, 70f,new_cities) && valueMetricBelowThreshold(distances,d,k,0.05)) || count > 1000 ) && (terrain[i,j].type != "shallow_water" || terrain[i,j].type != "deep_water")){
					satisfied = true;
				}
				count++;
				
			}
			new_cities[k].transform.position = terrain[i,j].center;
			/*if(locations_on){
					Destroy(cities[k]);
					cities[k] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cities[k].transform.position = terrain[i,j].center;
				}else{
					cities[k] = GameObject.CreatePrimitive(PrimitiveType.Cube);
					cities[k].transform.position = terrain[i,j].center;
				}*/
		}
		locations_on = true;

		return new_cities;
		
	}

	public static bool farFromCities(int count, int i, int j, int round, float dist, GameObject[] new_cities){
		bool ret = true;
		for(int l = 0; l < count; l++){
			ret = ret && (Utils.distance(new_cities[l].transform.position.x, new_cities[round].transform.position.z, i, j) >= dist);
		}
		return ret;
	}

	public static bool valueMetricBelowThreshold(double[] values, double v, int num_values, double threshold){
		bool ret = false;
		for(int i=0; i < num_values; i++){
			if((Mathf.Abs((float)(values[i] - v)) < threshold)){
				ret = true;
			}
		}
		return ret;
	}

}
