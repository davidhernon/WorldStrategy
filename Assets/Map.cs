/*
// A class for creating a playable map
//
//
//
*/
using UnityEngine;
using System.Collections;



public class Map {

	public Hex[,] terrain;
	public int num_row = 10;
	public int num_col = 10;
	private int x_off = 1;
	private int y_off = 1;
	private Vector2 shift = new Vector2(Random.Range (0.0f,1.0f),Random.Range (0.0f,1.0f));
	private float zoom = 0.02f;
	private double[,] height_map;
	private double[,] depth_map;
	private double[] histogram;
	private float noise = 0.0f;
	private static int scale = 1;
	private static float max_height = (40.0f) * (scale);
	private int cur_x = 0;
	private int cur_y = 0;
	private int hex_count = 0;
	private double max = -1.0;
	private double water_distribution = 0.13;
	private double min = 100.0;
	private double[,] moisture_map;
	private double effective_max_height = 0.0;
	private bool no_water = false;
	private double moisture_max_height = 0.0;
	private double diff = 0.0;
	private double inc = 0.0;
	private double water_height = 0.0;


	public Map(){
		terrain = new Hex[this.num_row, this.num_col];
	}

	public Map(int num_row, int num_col){
		this.terrain= new Hex[num_row, num_col];
		this.num_row = num_row;
		this.num_col = num_col;
	}

	public Map(int num_row, int num_col, Vector2 sh, float zm){
		this.terrain= new Hex[num_row, num_col];
		this.shift = sh;
		this.zoom = zm;
	}

// Set all hexes to either water or land
	public void initHexes(){
		int inx = 5;
		int iny = 5;

		// The commented out else section may not be necessary, added it to the code after I forgot how the code worked...
		for(int i=0; i<num_row; i++){
			for(int j=0; j < num_col; j++){
				this.terrain[i,j] = new Hex();
			}
		}
	}

	public double[,] generateNoiseArray(double[,] array, int rows, int cols, float[] zoom_array, float[] blend_array){

		for (int i=0; i < rows; i++) {
			for (int j=0; j < cols; j++) {
				Vector2 perlin1 = zoom_array[0] * (new Vector2(i,j)) + shift;
				Vector2 perlin2 = (zoom_array[1] * (new Vector2(i,j)) + shift);
				Vector2 perlin3 = (zoom_array[2] * (new Vector2(i,j)));
				array[i,j] = (double)((Mathf.PerlinNoise(perlin1.x, perlin1.y)*max_height*blend_array[0] +
				                           Mathf.PerlinNoise(perlin2.x, perlin2.y)*max_height*blend_array[1] +
				                           Mathf.PerlinNoise(perlin3.x, perlin3.y)*max_height*blend_array[2] ));

				if(array[i,j] < min){
					min = array[i,j];
				}
				if(array[i,j] > max){
					max = array[i,j];
				}
			}
		}

		return array;
	}

	public void generateMoistureMap(){
		moisture_map = new double[num_row, num_col];
		moisture_map = generateNoiseArray (new double[num_row, num_col], num_row, num_col, new float[]{0.01f,0.03f,0.05f}, new float[] {
			0.8f,
			0.3f,
			0.0f
		});
		moisture_max_height = max - min;
	}

// Generates a 2D array of doubles to be used as height values for tile points
// histogram is a sorted array of height values used to find appropriate water level
	public void generateHeightMap(){
		Vector2 pos;
		int r = num_row*4;
		int c = num_col*3 + 1;
		height_map = new double[r+3,c+4];
		moisture_map = new double[r+3,c+4];
		min = 100.0;
		max = -1.0;

		height_map = generateNoiseArray (height_map, r + 3, c + 4, new float[]{0.02f,0.01f,0.05f}, new float[]{0.5f,0.5f,0.3f});

		diff = max - min;
		inc = diff * water_distribution;
		inc += min;
		effective_max_height = diff - inc;
		depth_map = new double[r+3, c+4];

		//The following line will turn off the water
		if(no_water)
			inc = 0.0;

		for (int i=0; i<r+3; i++) {
			for(int j=0; j<c+4; j++){
				depth_map[i,j] = height_map[i,j] - (2*inc);
				if(i<=2 || i >= r-1 || j <= 2 || j >= c-1){
					height_map[i,j]=0.0;
					continue;
				}
				if(height_map[i,j] - 2*inc > 0.0){
					height_map[i,j] -= 2*inc;
				}else{
					height_map[i,j] = 0.0;
				}
			}
		}		

	}

// Create all the Hexes for the Map
	public void createHexBoard(){
		for (int i=0; i<num_col; i++) {
			for (int j=0; j<num_row; j++) {
				if(i % 2 == 0){
					buildHex(i,j, false);
				}else{
					buildHex(i,j, true);
				}
			}
		}
		//setWaterHeight();
	}


// Build a Hex vertices for all Hexes in terrain
	public 	void buildHex(int y, int x, bool odd){

			x_off = 0;
			y_off = 0;
			int hy = y;
			int hx = x;

			cur_x = hx;
			cur_y = hy;
		
			if (odd) {
					y--;
					x_off = 2;
					y_off = 3;
			}



			Vector3[] vertices = new Vector3[7];

				vertices = new Vector3[] {
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 0), ((((y / 2) * 6) + y_off) * scale)),
							new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, 1), (((y / 2) * 6) + y_off + 1) * scale),
							new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, 2), ((((y / 2) * 6) + y_off + 1) * scale)),
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
							new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, 4), (((y / 2) * 6) + y_off + 3) * scale),
							new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
				};


			
			//terrain [hx, hy] = new Hex (vertices, hx, hy);

			terrain[hx,hy].addParams(vertices, hx, hy);
			hex_count++;
	}

	public int getCurX(){
		return this.cur_x;
	}

	public int getCurY(){
		return this.cur_y;
	}

// Returns the elevation at a specific vertex
	public int getElevation(int x, int y, int x_o, int y_o, int vertex){

		//print ("x: " + x + " " + y + " center at: " + ((2 * x + 1) * 2 + x_o) + " and " + (6 * (y / 2) + 2 + y_o));
		int x_hex = ((2 * x + 1) * 2 + x_o);
		int y_hex = (6 * (y / 2) + 2 + y_o);

		if (vertex == 0) {
				return -((int)height_map[x_hex,y_hex-2]);
		} else if (vertex == 1) {
				return -((int)height_map[x_hex-2,y_hex-1]);
		} else if (vertex == 2) {
				//print ("x_hex+2 " + (x_hex+2) + " y_hex-1 " + (y_hex-1));
				return -((int)height_map[x_hex+2,y_hex-1]);
		} else if (vertex == 3) {
				return -((int)height_map[x_hex,y_hex]);
		} else if (vertex == 4) {
				return -((int)height_map[x_hex-2,y_hex+1]);
		} else if (vertex == 5) {
				return -((int)height_map[x_hex+2,y_hex+1]);
		} else if (vertex == 6) {
				return -((int)height_map[x_hex,y_hex+2]);
		} else {
				return 1;
		}
	}

	public void setWaterHeight(){
		Debug.Log("set water height");
		double local_max = -1.0;
		double local_min = double.MaxValue;
		double local_inc = 0.0;
		for(int i=0; i < num_row; i++){
			for(int j=0; j < num_col; j++){
				for(int k=0; k < 7; k++){
					if( (-(terrain[i,j].vertices[k].y)) > local_max)
						local_max = -(terrain[i,j].vertices[k].y);
					if( (-(terrain[i,j].vertices[k].y)) < local_min)
						local_min = -(terrain[i,j].vertices[k].y);
				}
			}
		}

		local_inc = ((max - min) * water_distribution) + min;

		for(int i=0; i < num_row; i++){
			for(int j=0; j < num_col; j++){
				for(int k=0; k < 7; k++){
					// we add inc since vertices are negative
					if(i == 0 || j == 0 || i == num_row - 1 || j == num_col - 1){
						terrain[i,j].vertices[k].y = 0f;
						//terrain[i,j].calcAvgHeight();
						//Debug.Log("edge set to zero");
					}
					else if ( -(terrain[i,j].vertices[k].y) + local_inc > 0.0 ){
						//Debug.Log("need to reduce height: " + terrain[i,j].vertices[k].y);
						//terrain[i,j].vertices[k].y += (float)((2f)*inc);
						//Debug.Log("x,y,vert: " + i + " " + j + " " + (terrain[i,j].vertices[k].y + (2f*local_inc)));
						terrain[i,j].vertices[k].y += (float)(2f*local_inc);
						//terrain[i,j].vertices[k].y -= 1000f;
						//Debug.Log(terrain[i,j].vertices[k].y);
						//terrain[i,j].calcAvgHeight();
					}
					else{
						//Debug.Log("height low enough i set it to zero");
						terrain[i,j].vertices[k].y = 0f;
						//terrain[i,j].calcAvgHeight();

					}




				}
			}
		}

		water_height = local_inc + local_min;
		//effective_max_height = diff - local_inc;
		//this.terrain = terrain;
	}

		// Returns the depth over a hex
	public int getDepth(int i, int j){

		int depth = 0;

		//	if(i!=0 && j != 0 && i <= num_row-1 && j <= num_col -1){

		int x = i;
		int y = j;

		int x_off = 0;
		int y_off = 0;

		if (j%2==0) {
				y--;
				x_off = 2;
				y_off = 3;
		}

		int x_hex = ((2 * x + 1) * 2 + x_off);
		int y_hex = (6 * (y / 2) + 2 + y_off);

		//Debug.Log("x,y,depth,water_height: " + " " + x_hex + " " + y_hex + " " + depth + " " + water_height);
		return ((-((int)depth_map[x_hex,y_hex-2])) + (-((int)depth_map[x_hex-2,y_hex-1])) + (-((int)depth_map[x_hex+2,y_hex-1])) + (-((int)depth_map[x_hex,y_hex])) + (-((int)depth_map[x_hex-2,y_hex+1])) + (-((int)depth_map[x_hex+2,y_hex+1])) + (-((int)depth_map[x_hex,y_hex+2])) / 7);
		

		}

	public void setHexType(){

		for(int i=0; i < num_row; i++){
			for(int j=0; j < num_col; j++){

				

				// }else{
				// 	depth = (int)inc;
				// }
				//Debug.Log(ret + " " + terrain[i,j].getHexAverageElevation());

				// if(i==0 || i == num_row-1 || j==0 || j == num_col-1){
				// 	terrain[i,j].type = "deep_water";
				// 	continue;
				// }

				if(terrain[i,j].getHexAverageElevation() <= 0.0){
					if(getDepth(i,j) > 10){
						terrain[i,j].type = "deep_water";
					}else{
						terrain[i,j].type = "shallow_water";
					}

					if(i==0 || i == num_row-1 || j==0 || j == num_col-1){
						//terrain[i,j].type = "deep_water";
						continue;
					}

					if(neighborHexTypes(i,j,"sand") > 1 && Random.Range(0,100) > 50 &&  i>1 && j>1 && i<=num_row-1 && j <= num_col-1){
						terrain[i,j].type = "sand";
					}
					//terrain[i,j].type = "water";
				}else if(terrain[i,j].getHexAverageElevation() <= effective_max_height * 0.1){
					terrain[i,j].type = "sand";
				}else if(terrain[i,j].getHexAverageElevation() <= effective_max_height * 0.41){
					terrain[i,j].type = "grass";
				}else if(terrain[i,j].getHexAverageElevation() <= effective_max_height * 0.65){
					terrain[i,j].type = "rock";
				}else if(terrain[i,j].getHexAverageElevation() <= effective_max_height){
					terrain[i,j].type = "snow";
				}else{
					//Debug.Log ("There has been a horrible error!");
					terrain[i,j].type = "error";
				}

				//TODO make sure this only happens once, no moisture map just set rainfall directly
				terrain[i,j].rainfall = moisture_map[i,j];
			}
		}

		for(int i=0; i<num_row; i++){
			for(int j=0; j < num_col; j++){
				//Change from Water to Deep Water and Shallow Water
				if(i != 0 && j != 0 && i != num_row-1 && j != num_col-1){
					// if(terrain[i,j].type == "water" && (neighborHexTypes(i,j,"sand") > 0)){
					// 	//terrain[i,j].type = "shallow_water";
					// 	continue;
					// }
					// else if (terrain[i,j].type == "water"){
					// 	//terrain[i,j].type = "deep_water";
					// 	if(neighborHexTypes(i,j,"shallow_water") > 0 && Random.Range(0,100) > 75){
					// 		//terrain[i,j].type = "shallow_water";
					// 	}
					// 	// else if(neighborHexTypes(i,j,"shallow_water") > 0 && Random.Range(0,100) > 75){
					// 	// 	terrain[i,j].type = "shallow_water";
					// 	// }
					// 	continue;
					// }

				// continue line should have stopped us from reaching this point as a water tile
				// this point forward all tiles are land
					//Debug.Log(terrain[i,j].rainfall);
					if(terrain[i,j].rainfall <= max * 0.43){
						if(terrain[i,j].type == "grass")
							terrain[i,j].type = "desert";
							continue;
					}else if(terrain[i,j].rainfall <= max * 0.5){
						if(terrain[i,j].type == "rock"){
							if(Random.Range(0,100) > 75)
								terrain[i,j].type = "snow";
							else if (Random.Range(0,100) > 50)
								terrain[i,j].type = "tundra";
						}else if(terrain[i,j].type == "grass" && Random.Range(0,100) > 50){
							//terrain[i,j].type = "plains";
						}
					}else if(terrain[i,j].rainfall <= max * 0.6){
						if(terrain[i,j].type == "sand" && Random.Range(0,100) > 75){
							terrain[i,j].type = "marsh";
						}else if(terrain[i,j].type == "grass"){
							terrain[i,j].type = "jungle";
						}
					}
				}

			}
				  
		}

	}

	public int distanceToOcean(int x, int y, int count, int[,] alreadyChecked){

		if (count > 3) {
			return count;
		}

		if(x <= 0 || y <= 0 || x >= num_row-1 || y >= num_col-1){
			return count;
		}

		int one = int.MaxValue, two = int.MaxValue, three = int.MaxValue, four = int.MaxValue, five = int.MaxValue, six = int.MaxValue;

		if (terrain [x, y].type == "water" || terrain [x, y].type == "shallow_water" || terrain [x, y].type == "deep_water" || terrain [x, y].type == "error" || terrain [x, y].type == null) 
			return count;
		else {
			if(y%2==0){
				if(alreadyChecked[x+1, y+1] != 1){
					alreadyChecked[x+1, y+1] = 1;
					one = distanceToOcean (x+1,y+1,count+1,alreadyChecked);
				}
				if(alreadyChecked[x, y+1] != 1){
					alreadyChecked[x, y+1] = 1;
					two = distanceToOcean (x,y+1,count+1,alreadyChecked);
				}
				if(alreadyChecked[x-1, y] != 1){
					alreadyChecked[x-1, y] = 1;
					three = distanceToOcean (x-1,y,count+1,alreadyChecked);
				}
				if(alreadyChecked[x, y-1] != 1){
					alreadyChecked[x, y-1] = 1;
					four = distanceToOcean (1,y-1,count+1,alreadyChecked);
				}
				if(alreadyChecked[x+1, y-1] != 1){
					alreadyChecked[x+1, y-1] = 1;
					five = distanceToOcean (x+1,y+1,count+1,alreadyChecked);
				}
				if(alreadyChecked[x+1, y] != 1){
					alreadyChecked[x+1, y] = 1;
					six = distanceToOcean (x+1,y+1,count+1,alreadyChecked);
				}
				return Mathf.Min (Mathf.Min (Mathf.Min (Mathf.Min (Mathf.Min (one,two), three), four), five), six);
			}else{
				if(alreadyChecked[x, y+1] != 1){
					alreadyChecked[x, y+1] = 1;
					one = distanceToOcean (x+1,y+1,count+1,alreadyChecked);
				}
				if(alreadyChecked[x+1, y] != 1){
					alreadyChecked[x+1, y] = 1;
					two = distanceToOcean (x,y+1,count+1,alreadyChecked);
				}
				if(alreadyChecked[x-1, y+1] != 1){
					alreadyChecked[x-1, y+1] = 1;
					three = distanceToOcean (x-1,y,count+1,alreadyChecked);
				}
				if(alreadyChecked[x-1, y] != 1){
					alreadyChecked[x-1, y] = 1;
					four = distanceToOcean (1,y-1,count+1,alreadyChecked);
				}
				if(alreadyChecked[x-1, y-1] != 1){
					alreadyChecked[x-1, y-1] = 1;
					five = distanceToOcean (x+1,y+1,count+1,alreadyChecked);
				}
				if(alreadyChecked[x, y+1] != 1){
					alreadyChecked[x, y+1] = 1;
					six = distanceToOcean (x+1,y+1,count+1,alreadyChecked);
				}
				return Mathf.Min (Mathf.Min (Mathf.Min (Mathf.Min (Mathf.Min (one,two), three), four), five), six);
			}
		}	
	}

	public int neighborHexTypes(int i, int j, string type){
		string s = terrain[0,0].type;
		//Debug.Log(i + " " + j);
		int count = 0;
		if(terrain[i+1,j].type == type)
			count++;
		if(terrain[i+1,j+1].type == type)
			count++;
		if(terrain[i,j+1].type == type)
			count++;
		if(terrain[i-1,j].type == type)
			count++;
		if(terrain[i-1,j-1].type == type)
			count++;
		if(terrain[i,j-1].type == type)
			count++;
		
		return count;
	}
	
}
