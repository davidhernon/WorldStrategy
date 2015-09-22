﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;

public class World3 : MonoBehaviour {

	public Color[] plains;
	public Color[] sand; 
	public Color[] forest;
	public Color[] jungle;
	public Color[] rock;

	public List<Vector3> newVertices = new List<Vector3> ();
	public List<int> newTriangles = new List<int> ();
	public List<Vector2> newUV = new List<Vector2>();
	public List<Color> newColor = new List<Color> ();
	private Mesh mesh;

	private int hex_count = 0;
	private int vertex_count;
	private int x_shift, y_shift, this_x_shift, this_y_shift, row_index;
	public double[,] height_map;
	public double[,] moisture;
	public Hex[,] terrain;
	public static int border = 6;

	private static int x_off = 1;
	private static int y_off = 1;

	public static Vector2 shift = new Vector2(0,0);
	public static float zoom = 0.1f;

		//rows go sideways across
		//cols go down
	public static int num_row = 20;
	public static int num_col = (int)(num_row * 1.618);
	private static int scale_x = (int)num_row/2;
	private static int scale_y = (int)num_col/2;
	private static int scale = 1;
	private static float max_height = (5.0f) * (scale);

	private static float ef_max_height = 0.0f;

	//Raycast From Mouse to Terrain
	public RaycastHit hit;
    public static Collider collider1 = new Collider();
    private Ray ray;
    private Vector3 vec;
    private LayerMask layerMask;
    
    private GameObject mesh_terrain;

    private GameObject sphere1;

    public static GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

    private int cur_x = 0;
    private int cur_y = 0;

    void Awake () {
    	UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;
    	//UnityEngine.Random.seed = 1;
    	plains = new Color[1] {getColor(195,245,0)};
    	sand = new Color[1] {getColor(255,279,0)};
		forest = new Color[1] {getColor(0,133,0)};
		jungle = new Color[1] {getColor(43,117,33)};
		rock = new Color[1] {getColor(120,120,120)};
    }

	// Use this for initialization
	void Start () {

		GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.cyan;
		
		mesh_terrain = GameObject.Find("Terrain");
		mesh = GetComponent<MeshFilter> ().mesh;
		Vector2 texture = new Vector2 (0,0);
		float z = transform.position.z;
		terrain = new Hex[num_row, num_col];
		initHex();
		GenerateHeightMap ();
		//Generate the Hexes
		CreateHexes(z, texture);
		moisture = TerrainFunctions.createMoistureMap(num_row, num_col);
		TerrainFunctions.setCoast(terrain,num_row,num_col);
		terrain = TerrainFunctions.iterateOverWater(terrain, num_row, num_col);
		List<Vector3> rivers = TerrainFunctions.addRivers(terrain, num_row, num_col);
		TerrainFunctions.addRiversToBoard(rivers);
		for(int i=0; i<50; i++){
			rivers = TerrainFunctions.addRivers(terrain, num_row, num_col);
			TerrainFunctions.addRiversToBoard(rivers);
		}
		TerrainFunctions.initializeBiomes(terrain, moisture, num_row, num_col);

		//Add Hexes to the Board
		CreateMesh(z,texture);
		BuildMesh ();

		//GetComponent<MeshCollider>().mesh = mesh;
		MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshc.sharedMesh = mesh; // Give it your mesh here.
		sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);

	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
       Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray2.origin, ray2.direction * 10000, Color.yellow);
		if(Physics.Raycast(ray2, out hit, 10000f)){
			if(hit.collider.gameObject.CompareTag("Terrain")){
				Hex ret = getMouseHex(new Vector3(hit.point.x,hit.point.z,0));
				sphere1.transform.position = ret.center;
				Debug.Log(ret.pos);
			}
		}

	}

	void GenerateHeightMap()
	{

		Vector2 pos;
		float noise = 0.0f;
		int r = num_row*4;
		int c = num_col*3 + 1;
		height_map = new double[r+3,c+4];

		for (int i=0; i<r+3; i++) {
						for (int j=0; j<c+4; j++) {
								
								if(isBorder (i,j,r,c)){
									height_map[i,j] = 0;
								}else{
									double sc = 0.0;
									if(i > num_row)
										sc = num_row - (i % num_row);
									sc = sc / num_row;
									pos = zoom * (new Vector2(i,j)) + shift;
									noise = Mathf.PerlinNoise(pos.x, pos.y);
									height_map[i,j] = (double)(noise*max_height);
									if(height_map[i,j] > ef_max_height){
										ef_max_height = (float)height_map[i,j];
									}
								}
						}
				}
	}

	bool isBorder(int i, int j, int r, int c){

				if (i <= (0 + border+1) || i >= (r + 2 - border) || j <= (0 + border) || j >=(c-border)) {
						return true;
				} else {
						return false;
				}

		}

	void BuildMesh()
	{

		mesh.Clear ();
		mesh.vertices = newVertices.ToArray ();
		mesh.triangles = newTriangles.ToArray ();
		//mesh.uv = newUV.ToArray();
		mesh.colors = newColor.ToArray ();
		mesh.Optimize ();
		mesh.RecalculateNormals ();
		
		newVertices.Clear();
		newTriangles.Clear();
		//newUV.Clear();
		newColor.Clear ();

	}

	void fixZ(){

		for(int i = 0; i < num_row; i++){
			terrain[i,num_col-3].bottom.y = 0;
			terrain[i,num_col-3].leftBot.y = 0;
			terrain[i,num_col-3].rightBot.y = 0;
		}
	}

	void UpdateMesh()
	{



	}

	void CreateHexes(float z, Vector2 texture)
	{

		for (int i=0; i<num_col; i++) {
						for (int j=0; j<num_row; j++) {
							if(i % 2 == 0){
								BuildHex(i,j,z,texture, false);
							}else{
								BuildHex(i,j,z,texture, true);
							}
						}
				}

	}

	void CreateMesh(float z, Vector2 texture){
				for (int i=0; i<num_col; i++) {
						for (int j=0; j<num_row; j++) {
							if(i % 2 == 0){
								addHexToMesh(i,j,z,texture, false);
							}else{
								addHexToMesh(i,j,z,texture, true);
							}
						}
				}
	}

	

	int getElevation(int x, int y, int x_o, int y_o, int vertex){

				//print ("x: " + x + " " + y + " center at: " + ((2 * x + 1) * 2 + x_o) + " and " + (6 * (y / 2) + 2 + y_o));

				int x_hex = ((2 * x + 1) * 2 + x_o);
				int y_hex = (6 * (y / 2) + 2 + y_o);

				if(terrain[cur_x, cur_y].type == "water" || terrain[cur_x, cur_y].type == "shallow_water" || terrain[cur_x, cur_y].type == "deep_water"){
					return 0;
				}

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



	void addVertex(Vector3 vert, int x, int y, int pos){

		newVertices.Add (vert);
		terrain[x,y].id_list[pos] = vertex_count; 
		vertex_count++;

		}

	void addColor(float pos){

		print("pos: " + pos + " height: " + ef_max_height);
		pos = - pos;

		if(pos <= (float)(ef_max_height/4)){
			newColor.Add(Color.cyan);
		}else if(pos > (float)(ef_max_height/4) && pos <= (float)(ef_max_height/2)) {
			newColor.Add(Color.yellow);
		}else if(pos > (float)(ef_max_height/2) && pos <= (float)(3*(ef_max_height/4)) ){
			newColor.Add(Color.green);
		}else if(pos > (float)(3*(ef_max_height/4)) && pos <= (float)(7*(ef_max_height/8))){
			newColor.Add(Color.grey);
		} else{
			newColor.Add(Color.white);
		}

	}

	void addColor2(int x, int y, int count){

		string type = terrain[x,y].type;

		for(int i=0; i < count; i++){
			int r = UnityEngine.Random.Range(0,3);
			if(type == "water"){
				newColor.Add(Color.cyan);
			}else if(type=="land"){
				newColor.Add(Color.green);
			}else if(type == "ice"){
				newColor.Add(Color.white);
			}else if(type == "desert"){
				newColor.Add(getColor(255,221,115));
			}else if(type == "grass"){
				newColor.Add(Color.green);
			}else if(type == "shallow_water"){
				newColor.Add(new Color(0,1,1,1));
			}else if(type == "deep_water"){
				newColor.Add(getColor(0,177,198));
			}else if(type == "sand"){
				newColor.Add(sand[0]);
			}else if(type == "plains"){
				Debug.Log(r);
				newColor.Add(plains[0]);
			}else if(type == "forest"){
				newColor.Add(forest[0]);
			}else if(type == "jungle"){
				newColor.Add(jungle[0]);
			}else if(type =="rock"){
				newColor.Add(rock[0]);
			}else{
				Debug.Log(type);
				newColor.Add(Color.white);
			}
		}

	}

	public static Color getColor(int r, int g, int b){
		return new Color((1f*r)/255, (1f*g)/255, (1f*b)/255);
	}

	void addTriangles(int x, int y){

				newTriangles.Add (terrain [x, y].id_list [1]);
				newTriangles.Add (terrain [x, y].id_list [0]);
				newTriangles.Add (terrain [x, y].id_list [3]);
				
				newTriangles.Add (terrain [x, y].id_list [0]);
				newTriangles.Add (terrain [x, y].id_list [2]);
				newTriangles.Add (terrain [x, y].id_list [3]);
				
				newTriangles.Add (terrain [x, y].id_list [2]);
				newTriangles.Add (terrain [x, y].id_list [5]);
				newTriangles.Add (terrain [x, y].id_list [3]);
				
				newTriangles.Add (terrain [x, y].id_list [3]);
				newTriangles.Add (terrain [x, y].id_list [5]);
				newTriangles.Add (terrain [x, y].id_list [6]);
				
				newTriangles.Add (terrain [x, y].id_list [4]);
				newTriangles.Add (terrain [x, y].id_list [3]);
				newTriangles.Add (terrain [x, y].id_list [6]);
				
				newTriangles.Add (terrain [x, y].id_list [1]);
				newTriangles.Add (terrain [x, y].id_list [3]);
				newTriangles.Add (terrain [x, y].id_list [4]);

	}

	void worldToHex(Vector3 world){

				int x = (int)world.x;
				int y = (int)world.z;

				x = x / 4;
				y = (y-2)/6;

				sphere1.transform.position = terrain[x,y].center;
				Debug.Log("Sphere at Point: " + terrain[x,y].center);

				/*if (odd) {
						y--;
						x_off = 2;
						y_off = 3;
				}

								Vector3[] vertices = new Vector3[] {
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 0), ((((y / 2) * 6) + y_off) * scale)),
								new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, 1), (((y / 2) * 6) + y_off + 1) * scale),
								new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, 2), ((((y / 2) * 6) + y_off + 1) * scale)),
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
								new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, 4), (((y / 2) * 6) + y_off + 3) * scale),
								new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
						};*/

	}

	Hex getMouseHex(Vector3 world){
		for(int i=0; i < num_row; i++){
			for(int j=0; j < num_col; j++){
				if(terrain[i,j].inBoundingBox(world)){
					return terrain[i,j];
				}
			}
		}
		return null;
	}

	double scaleDistanceFromEdge(int x, int y){

		double xd = Mathf.Abs(x-num_row);
		double yd = Mathf.Abs(y-scale_y);
		Debug.Log("x: " + x + " y " + y + " " + xd/num_row);
		double ret = Math.Abs(((xd/num_row)%1.0)*((yd/num_col)%1.0) + 0.3);
		return ret;

	}

	void initHex(){

		int inx = 5;
		int iny = 5;

		for(int i=0; i<num_row; i++){
			for(int j=0; j < num_col; j++){
				terrain[i,j] = new Hex();
				if(i==0 || i==num_row-1 || j==0 || j==num_col-1){
					terrain[i,j].type = "water";
				}
				if((i < inx || i > num_row - inx || j < iny || j > num_col - iny) && UnityEngine.Random.Range(0,100) > 50 ){
					terrain[i,j].type = "water";
				}
			}
		}

		for(int i=1; i<num_row-1; i++){
			for(int j=1; j < num_col-1; j++){
				if(terrain[i,j].type == "water" && neighborHexTypes(i,j,"land") >= 4){
					terrain[i,j].type = "land";
				}
			}
		}

	}

	int neighborHexTypes(int i, int j, string type){
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

	void addHexToMesh(int y, int x, float depth, Vector2 texture, bool odd){

		x_off = 0;
				y_off = 0;

				int hy = y;
				int hx = x;

				cur_x = x;
				cur_y = y;
		
				if (odd) {
						y--;
						x_off = 2;
						y_off = 3;
				}

				//terrain[hx,hy] = new Hex();
				if (hy == 0 && hx == 0) {

					addVertex(terrain[hx,hy].vertices[0],hx,hy,0);
					addVertex(terrain[hx,hy].vertices[1],hx,hy,1);
					addVertex(terrain[hx,hy].vertices[2],hx,hy,2);
					addVertex(terrain[hx,hy].vertices[3],hx,hy,3);
					addVertex(terrain[hx,hy].vertices[4],hx,hy,4);
					addVertex(terrain[hx,hy].vertices[5],hx,hy,5);
					addVertex(terrain[hx,hy].vertices[6],hx,hy,6);

					addTriangles(hx,hy);

					addColor2(hx,hy,7);

				} else if (hy == 0 && hx != 0) {

					addVertex(terrain[hx,hy].vertices[0],hx,hy,0);
					terrain[hx,hy].id_list[1] = terrain [hx-1, hy].id_list[2];
					addVertex(terrain[hx,hy].vertices[2],hx,hy,2);
					addVertex(terrain[hx,hy].vertices[3],hx,hy,3);
					terrain [hx, hy].id_list [4] = terrain [hx - 1, hy].id_list [5];
					addVertex(terrain[hx,hy].vertices[5],hx,hy,5);
					addVertex(terrain[hx,hy].vertices[6],hx,hy,6);

					addTriangles(hx,hy);

					addColor2(hx,hy,5);
						

				} else if (!odd) {
						//even hex in y direc

						if (hy != 0 && hx == 0) {

							
							terrain [hx, hy].id_list [0] = terrain [hx, hy - 1].id_list [4];
							addVertex(terrain[hx,hy].vertices[1], hx,hy,1);
							terrain [hx, hy].id_list [2] = terrain [hx, hy - 1].id_list [6];
							addVertex(terrain[hx,hy].vertices[3],hx,hy,3);
							addVertex(terrain[hx,hy].vertices[4],hx,hy,4);
							addVertex(terrain[hx,hy].vertices[5],hx,hy,5);
							addVertex(terrain[hx,hy].vertices[6],hx,hy,6);
							addTriangles(hx,hy);
							addColor2(hx,hy,5);

						} else if (hy != 0 && hx != 0) {

							terrain [hx, hy].id_list [0] = terrain [hx-1, hy - 1].id_list [5];
							terrain [hx, hy].id_list [1] = terrain [hx - 1, hy].id_list [2];
							terrain [hx, hy].id_list [2] = terrain [hx, hy - 1].id_list [6];
							terrain [hx, hy].id_list [4] = terrain [hx - 1, hy].id_list [5];
							addVertex(terrain[hx,hy].vertices[3],hx,hy,3);
							addVertex(terrain[hx,hy].vertices[5],hx,hy,5);
							addVertex(terrain[hx,hy].vertices[6],hx,hy,6);

							addTriangles(hx,hy);

							addColor2(hx,hy,3);	

						} else if (hx != 0 && hy != num_col - 1) {
								print ("uncaught scenario in hex make1");
								return;

						} else {
								print ("uncaught scenario in hex make2");
								return;
						}

				} else {
						//odd hex in y direc

						if (hy != 0 && hx == 0 && hx != num_row - 1) {	

								terrain [hx, hy].id_list [0] = terrain [hx, hy - 1].id_list [5];
								terrain [hx, hy].id_list [1] = terrain [hx, hy - 1].id_list [6];
								terrain [hx, hy].id_list [2] = terrain [hx + 1, hy - 1].id_list [6];
								addVertex(terrain [hx, hy].vertices[3],hx,hy,3);
								addVertex(terrain [hx, hy].vertices[4],hx,hy,4);
								addVertex(terrain [hx, hy].vertices[5],hx,hy,5);
								addVertex(terrain [hx, hy].vertices[6],hx,hy,6);
								addTriangles(hx,hy);
								addColor2(hx,hy,4);				
				
						} else if (hy!=0 && hx==num_row-1 && hx==0){

						terrain[hx,hy].id_list[0] = terrain[hx,hy-1].id_list[5];
						terrain[hx,hy].id_list[1] = terrain[hx,hy-1].id_list[6];
						addVertex(terrain[hx,hy].vertices[2],hx,hy,2);
						addVertex(terrain[hx,hy].vertices[3],hx,hy,3);
						addVertex(terrain[hx,hy].vertices[4],hx,hy,4);
						addVertex(terrain[hx,hy].vertices[5],hx,hy,5);
						addVertex(terrain[hx,hy].vertices[6],hx,hy,6);
						addTriangles(hx,hy);	
						addColor2(hx,hy,5);

						} else if(hy!=0 && hx == num_row-1){
	
							terrain[hx,hy].id_list[0] = terrain[hx,hy-1].id_list[5];
							terrain[hx,hy].id_list[1] = terrain[hx,hy-1].id_list[6];
							addVertex(terrain[hx,hy].vertices[2],hx,hy,2);
							addVertex(terrain[hx,hy].vertices[3],hx,hy,3);
							terrain[hx,hy].id_list[4] = terrain[hx-1,hy].id_list[5];
							addVertex(terrain[hx,hy].vertices[5],hx,hy,5);
							addVertex(terrain[hx,hy].vertices[6],hx,hy,6);
							addTriangles(hx,hy);
							addColor2(hx,hy,4);
				
						} else if(hy!=0 && hx!=num_row-1){

							terrain[hx,hy].id_list[0] = terrain[hx,hy-1].id_list[5];
							terrain[hx,hy].id_list[1] = terrain[hx,hy-1].id_list[6];
							terrain [hx, hy].id_list [2] = terrain[hx+1,hy-1].id_list[6];
							addVertex(terrain[hx,hy].vertices[3],hx,hy,3);
							terrain[hx,hy].id_list[4] = terrain[hx-1,hy].id_list[5];
							addVertex(terrain[hx,hy].vertices[5],hx,hy,5);
							addVertex(terrain[hx,hy].vertices[6],hx,hy,6);
							addTriangles(hx,hy);
							addColor2(hx,hy,3);
				
						} else{
							print ("uncaught scenario in hex make");
							return;
						}

				}

	}

	void BuildHex(int y, int x, float depth, Vector2 texture, bool odd){

				x_off = 0;
				y_off = 0;

				int hy = y;
				int hx = x;

				cur_x = x;
				cur_y = y;
		
				if (odd) {
						y--;
						x_off = 2;
						y_off = 3;
				}

				//terrain[hx,hy] = new Hex();

				if (hy == 0 && hx == 0) {

						Vector3[] vertices = new Vector3[] {
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

				} else if (hy == 0 && hx != 0) {
						Vector3[] vertices = new Vector3[] {
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 0), ((((y / 2) * 6) + y_off) * scale)),
							terrain [hx - 1, hy].rightTop,
							new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, 2), ((((y / 2) * 6) + y_off + 1) * scale)),
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
							terrain [hx - 1, hy].rightBot,
							new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
						};

						//terrain [hx, hy] = new Hex (vertices, hx, hy);
						terrain[hx,hy].addParams(vertices, hx, hy);				
						hex_count++;

				} else if (!odd) {
						//even hex in y direc

						if (hy != 0 && hx == 0) {

								Vector3[] vertices = new Vector3[] {
											terrain [hx, hy - 1].leftBot,
											new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, 1), (((y / 2) * 6) + y_off + 1) * scale),
											terrain [hx, hy - 1].bottom,
											new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
											new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, 4), (((y / 2) * 6) + y_off + 3) * scale),
											new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
											new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
										};

								//terrain [hx, hy] = new Hex (vertices, hx, hy);
								terrain[hx,hy].addParams(vertices, hx, hy);
								hex_count++;

						} else if (hy != 0 && hx != 0) {

								Vector3[] vertices = new Vector3[] {
									terrain [hx-1, hy - 1].rightBot,
									terrain [hx - 1, hy].rightTop,
									terrain [hx, hy - 1].bottom,
									new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
									terrain [hx - 1, hy].rightBot,
									new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
									new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
								};
				
								//terrain [hx, hy] = new Hex (vertices, hx, hy);
								terrain[hx,hy].addParams(vertices, hx, hy);
								hex_count++;

						} else if (hx != 0 && hy != num_col - 1) {
								print ("uncaught scenario in hex make1");
								return;

						} else {
								print ("uncaught scenario in hex make2");
								return;
						}

				} else {
						//odd hex in y direc

						if (hy != 0 && hx == 0 && hx != num_row - 1) {

								Vector3[] vertices = new Vector3[]{
								terrain [hx, hy - 1].rightBot,
					            terrain [hx, hy - 1].bottom,
								terrain [hx + 1, hy - 1].bottom,
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
								new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, 4), (((y / 2) * 6) + y_off + 3) * scale),
								new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
								new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
							};

								//terrain [hx, hy] = new Hex (vertices, hx, hy);
								terrain[hx,hy].addParams(vertices, hx, hy);
								hex_count++;

				
						} else if (hy!=0 && hx==num_row-1 && hx==0){

						Vector3[] vertices = new Vector3[]{
							terrain[hx,hy-1].rightBot,
							terrain[hx,hy-1].bottom,
							new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, 2), ((((y / 2) * 6) + y_off + 1) * scale)),
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
							new Vector3 ((((4 * x) + x_off) * scale), getElevation (x, y, x_off, y_off, 4), (((y / 2) * 6) + y_off + 3) * scale),
							new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
							new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
						};
				
						//terrain[hx,hy] = new Hex(vertices, hx, hy);
						terrain[hx,hy].addParams(vertices, hx, hy);
						hex_count++;

				} else if(hy!=0 && hx == num_row-1){

				Vector3[] vertices = new Vector3[]{
					terrain[hx,hy-1].rightBot,
					terrain[hx,hy-1].bottom,
					new Vector3 ((((4 * (x + 1) + x_off) * scale)), getElevation (x, y, x_off, y_off, 2), ((((y / 2) * 6) + y_off + 1) * scale)),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
					terrain[hx-1,hy].rightBot,
					new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
				};

				//terrain[hx,hy] = new Hex(vertices, hx, hy);
				terrain[hx,hy].addParams(vertices, hx, hy);
				hex_count++;

				
						} else if(hy!=0 && hx!=num_row-1){

				Vector3[] vertices = new Vector3[]{
					terrain[hx,hy-1].rightBot,
					terrain[hx,hy-1].bottom,
					terrain[hx+1,hy-1].bottom,
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 3), (((y / 2) * 6) + y_off + 2) * scale),
					terrain[hx-1,hy].rightBot,
					new Vector3 (((4 * (x + 1) + x_off) * scale), getElevation (x, y, x_off, y_off, 5), (((y / 2) * 6) + y_off + 3) * scale),
					new Vector3 ((((((2 * x) + 1) * 2) + x_off) * scale), getElevation (x, y, x_off, y_off, 6), (((y / 2) * 6) + y_off + 4) * scale)
				};

				//terrain[hx,hy] = new Hex(vertices, hx, hy);
				terrain[hx,hy].addParams(vertices, hx, hy);
				hex_count++;
				
						} else{
							print ("uncaught scenario in hex make");
							return;
						}

				}

		}


}
