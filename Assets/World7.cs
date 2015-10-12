using UnityEngine;
//using System.Collections;
//using System;
using System.Collections.Generic;

public class World7 : MonoBehaviour {

	public static Map map;
	public static Mesh mesh;
	private GameObject mesh_terrain;
	public static int num_row = 30; //75, 63
//	public static int c_lengthx = 63*4;
//	public static int c_lengthy = 63*3 + 1;
//	public static int c_lengthz = 0;
	private static float col_stretch = 2.3f; //1.618, 2.3
	public static int num_col = 0;
	private static int x_off = 0;
	private static int y_off = 0;
	private static int cur_x = 0;
	private static int cur_y = 0;
	private static int vertex_count = 0;
	public static List<Vector3> newVertices = new List<Vector3> ();
	public  static List<int> newTriangles = new List<int> ();
	public static List<Color> newColor = new List<Color> ();
	public static bool showTile = false;
	public static GameObject sphere1;
	public static string tileInfo;
	private static bool locations_on = false;
		
	//cities
	public static GameObject[] cities;
	public static double avg_dist = 0.0;
	public static int  d_count = 0;
	public static int times = 0;
	
	
	// Use this for initialization
	void Start () {

		UnityEngine.Random.seed = (int)System.DateTime.Now.Ticks;
		num_col = (int)(num_row*col_stretch);
		mesh_terrain = GameObject.Find("Terrain");
		mesh = GetComponent<MeshFilter> ().mesh;
		map = new Map(num_row, num_col);
		map.initHexes();
		map.generateHeightMap();
		map.createHexBoard();
		map.generateMoistureMap();
		map.setHexType();

		createMesh();
		buildMesh();
		
		MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshc.sharedMesh = mesh; // Give it your mesh here.

//		Create an Ocean to make world look better
//		GameObject ocean = GameObject.CreatePrimitive(PrimitiveType.Cube);
//		ocean.transform.position = new Vector3(128f,20.1f,217f);
//		ocean.transform.localScale = new Vector3(256f,40f,440f);
//		ocean.GetComponent<Renderer>().material.color = ColorGenerator.getColorFromString("deep_water");


	}

	public static void createMesh(){
			for (int i=0; i<num_col; i++) {
					for (int j=0; j<num_row; j++) {
						if(i % 2 == 0){
							addHexToMesh(i,j, false);
						}else{
							addHexToMesh(i,j, true);
						}
					}
			}
	}

	public static void addHexToMesh(int y, int x, bool odd){

		int hy = y;
		int hx = x;
		// Add Hexes by Triangles
//		addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
//		newTriangles.Add (times * 18 + 0);
//		addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//		newTriangles.Add (times * 18 + 1);
//		addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//		newTriangles.Add (times * 18 + 2);
//
//		addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//		newTriangles.Add (times * 18 + 3);
//		addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//		newTriangles.Add (times * 18 + 4);
//		addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//		newTriangles.Add (times * 18 + 5);
//
//
//		addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//		newTriangles.Add (times * 18 + 6);
//		addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//		newTriangles.Add (times * 18 + 7);
//		addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//		newTriangles.Add (times * 18 + 8);
//
//
//		addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//		newTriangles.Add (times * 18 + 9);
//		addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//		newTriangles.Add (times * 18 + 10);
//		addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//		newTriangles.Add (times * 18 + 11);
//
//
//		addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//		newTriangles.Add (times * 18 + 12);
//		addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//		newTriangles.Add (times * 18 + 13);
//		addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//		newTriangles.Add (times * 18 + 14);
//
//		addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
//		newTriangles.Add (times * 18 + 15);
//		addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//		newTriangles.Add (times * 18 + 16);
//		addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//		newTriangles.Add (times * 18 + 17);
//
//		addColor(hx,hy,18);


		addVertex(map.terrain[hx,hy].vertices[0],hx,hy,(times*7)+0);
		addVertex(map.terrain[hx,hy].vertices[1],hx,hy,(times*7)+1);
		addVertex(map.terrain[hx,hy].vertices[2],hx,hy,(times*7)+2);
		addVertex(map.terrain[hx,hy].vertices[3],hx,hy,(times*7)+3);
		addVertex(map.terrain[hx,hy].vertices[4],hx,hy,(times*7)+4);
		addVertex(map.terrain[hx,hy].vertices[5],hx,hy,(times*7)+5);
		addVertex(map.terrain[hx,hy].vertices[6],hx,hy,(times*7)+6);

		for (int i=0; i<7; i++) {
			map.terrain[hx,hy].id_list[i] = (times*7)+i;
		}

		newTriangles.Add (times*7 + 1);
		newTriangles.Add (times*7 + 0);
		newTriangles.Add (times*7 + 3);
		
		newTriangles.Add (times*7 + 0);
		newTriangles.Add (times*7 + 2);
		newTriangles.Add (times*7 + 3);
		
		newTriangles.Add (times*7 + 2);
		newTriangles.Add (times*7 + 5);
		newTriangles.Add (times*7 + 3);
		
		newTriangles.Add (times*7 + 3);
		newTriangles.Add (times*7 + 5);
		newTriangles.Add (times*7 + 6);
		
		newTriangles.Add (times*7 + 4);
		newTriangles.Add (times*7 + 3);
		newTriangles.Add (times*7 + 6);
		
		newTriangles.Add (times*7 + 1);
		newTriangles.Add (times*7 + 3);
		newTriangles.Add (times*7 + 4);

//		addTriangles(hx,hy);
		addColor(hx,hy,7);
		times ++;

	}

	public static void addTriangles(int x, int y){

		newTriangles.Add (map.terrain[x, y].id_list [1]);
		newTriangles.Add (map.terrain[x, y].id_list [0]);
		newTriangles.Add (map.terrain[x, y].id_list [3]);
		
		newTriangles.Add (map.terrain[x, y].id_list [0]);
		newTriangles.Add (map.terrain[x, y].id_list [2]);
		newTriangles.Add (map.terrain[x, y].id_list [3]);
		
		newTriangles.Add (map.terrain[x, y].id_list [2]);
		newTriangles.Add (map.terrain[x, y].id_list [5]);
		newTriangles.Add (map.terrain[x, y].id_list [3]);
		
		newTriangles.Add (map.terrain[x, y].id_list [3]);
		newTriangles.Add (map.terrain[x, y].id_list [5]);
		newTriangles.Add (map.terrain[x, y].id_list [6]);
		
		newTriangles.Add (map.terrain[x, y].id_list [4]);
		newTriangles.Add (map.terrain[x, y].id_list [3]);
		newTriangles.Add (map.terrain[x, y].id_list [6]);
		
		newTriangles.Add (map.terrain[x, y].id_list [1]);
		newTriangles.Add (map.terrain[x, y].id_list [3]);
		newTriangles.Add (map.terrain[x, y].id_list [4]);

	}

	public static void addColor(int x, int y, int count){

		for (int i=0; i < count; i++) {
//			Color tri = ColorGenerator.getColorFromString (map.terrain [x, y].type);
			Color tri = Color.clear;
//			for (int j=0; j < 3; j++){
				newColor.Add (tri);
//			}
		}
	}

	public static void addVertex(Vector3 vert, int x, int y, int pos){

		newVertices.Add (vert);
//		map.terrain[x,y].id_list[pos] = vertex_count; 
		vertex_count++;

	}

	public void buildMesh()
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

	// Should be Used to change colors of a hex or something in the map
	// 
	public static void changeColors(){
		Color[] c = (Color[])mesh.colors.Clone ();
		Vector3[] v = (Vector3[])mesh.vertices.Clone ();
		for(int i=0; i<300; i++){
			mesh.colors[i] = new Color(0f,99f,1f);
			v[i] = v[i] + new Vector3(0f,-1f,0f);
		}
//		mesh.colors = c;
		mesh.vertices = v;

	}

	public static void setColorAtIndex(string type, int id){
		Color[] c = (Color[])mesh.colors.Clone ();
		Vector3[] v = (Vector3[])mesh.vertices.Clone ();
		for(int i=0; i<300; i++){
			c[i] = new Color(0f,99f,1f);
			v[i] = v[i] + new Vector3(0f,-1f,0f);
		}
		mesh.colors = c;
	}
	
}
