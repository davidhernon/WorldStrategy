/*
// A class for creating a playable map
//
//
//
*/
using UnityEngine;
using System.Collections;



public class Map : MonoBehaviour {

	public Hex[,] terrain;
	private int num_row = 10;
	private int num_col = 10;
	private int x_off = 1;
	private int y_off = 1;
	private Vector2 shift = new Vector2(0,0);
	private float zoom = 0.02f;
	private double[,] height_map;
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
				this.terrain[i,j].type = "land";
				/*if(i==0 || i==num_row-1 || j==0 || j==num_col-1){
					this.terrain[i,j].type = "land";
				}
				if((i < inx || i > num_row - inx || j < iny || j > num_col - iny) && UnityEngine.Random.Range(0,100) > 50){
					this.terrain[i,j].type = "water";
				}else{
				//	this.terrain[i,j].type = "land";
				}*/
			}
		}
		// Reiterate through the array and do "Game of Life"-esque ops
		// Experimental right now, uncomment to test
		/*for(int i=1; i<num_row-1; i++){
			for(int j=1; j < num_col-1; j++){
				if(this.terrain[i,j].type == "water" && neighborHexTypes(i,j,"land") >= 4){
					this.terrain[i,j].type = "land";
				}
			}
		}*/
	}

	public double[,] generateNoiseArray(double[,] array, int rows, int cols){

		for (int i=0; i < rows; i++) {
			for (int j=0; j < cols; j++) {
				Vector2 perlin1 = 0.02f * (new Vector2(i,j)) + shift;
				Vector2 perlin2 = (0.01f * (new Vector2(i,j)) + shift);
				Vector2 perlin3 = (0.05f * (new Vector2(i,j)));
				array[i,j] = (double)(Mathf.PerlinNoise(perlin1.x, perlin1.y)*max_height*0.5 +
				                           Mathf.PerlinNoise(perlin2.x, perlin2.y)*max_height*0.5 +
				                           Mathf.PerlinNoise(perlin3.x, perlin3.y)*max_height*0.3);
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

// Generates a 2D array of doubles to be used as height values for tile points
// histogram is a sorted array of height values used to find appropriate water level
	public void generateHeightMap(){
		Vector2 pos;
		int r = num_row*4;
		int c = num_col*3 + 1;
		height_map = new double[r+3,c+4];
		min = 100.0;
		max = -1.0;

		height_map = generateNoiseArray (height_map, r + 3, c + 4);

		/*for (int i=0; i<r+3; i++) {
			for(int j=0; j<c+4; j++){
				Vector2 perlin1 = 0.02f * (new Vector2(i,j)) + shift;
				//Vector2 perlin2 = (0.009f * (new Vector2(i,j)) + new Vector2(1,1)); //"other config" just incomment lines
				//Vector2 perlin3 = (0.05f * (new Vector2(i,j)) + new Vector2(1,0));
				Vector2 perlin2 = (0.01f * (new Vector2(i,j)) + shift);
				Vector2 perlin3 = (0.05f * (new Vector2(i,j)));
				height_map[i,j] = (double)(Mathf.PerlinNoise(perlin1.x, perlin1.y)*max_height*0.5 +
											Mathf.PerlinNoise(perlin2.x, perlin2.y)*max_height*0.5 +
											Mathf.PerlinNoise(perlin3.x, perlin3.y)*max_height*0.3);
				if(height_map[i,j] < min){
					min = height_map[i,j];
				}
				if(height_map[i,j] > max){
					max = height_map[i,j];
				}
			}
		}
		*/
		// Other method of generating noise, not configured
	/*	SimplexNoiseGenerator simplex = new SimplexNoiseGenerator();
		for (int i=0; i<r+3; i++) {
			for(int j=0; j<c+4; j++){
				pos = zoom * (new Vector2(i,j)) + shift;
				height_map[i,j] = (double)simplex.coherentNoise(i*1f,j*1f,0f,5,50,20f,0.5f,0.5f);
				if(height_map[i,j] < min){
					min = height_map[i,j];
				}
				if(height_map[i,j] > max){
					max = height_map[i,j];
				}
			}
		}*/

		double diff = max - min;
		double inc = diff * water_distribution;
		inc += min;

		//The following line will turn off the water
		//inc = 0.0;

		for (int i=0; i<r+3; i++) {
			for(int j=0; j<c+4; j++){
				height_map[i,j] -= inc;
				if(height_map[i,j] - inc > 0.0){
					height_map[i,j] -= inc;
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

	public void setHexType(){

		for(int i=0; i<num_row; i++){
			for(int j=0; j < num_col; j++){
				if(terrain[i,j].getHexAverageElevation() <= 0.0){
					terrain[i,j].type = "water";
				}else{
					terrain[i,j].type = "land";
				}
			}
		}

	}
	
}
