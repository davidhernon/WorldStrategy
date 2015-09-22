using UnityEngine;
//using System.Collections;
//using System;
using System.Collections.Generic;

public class World7 : MonoBehaviour {

	public static Map map;
	private Mesh mesh;
	private GameObject mesh_terrain;
	public static int num_row = 63; //75, 63
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

		// cities = new GameObject[5];
		// for(int i = 0 ; i < 5; i++){
		// 	cities[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
		// 	cities[i].transform.position = new Vector3(-1000,-1000,0);
		// }
		
		MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshc.sharedMesh = mesh; // Give it your mesh here.
//		sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//		sphere1.GetComponent<Collider>().enabled = false;

		GameObject ocean = GameObject.CreatePrimitive(PrimitiveType.Cube);
		ocean.transform.position = new Vector3(128f,20.1f,217f);
		ocean.transform.localScale = new Vector3(256f,40f,440f);
		ocean.GetComponent<Renderer>().material.color = ColorGenerator.getColorFromString("deep_water");


	}
	
	// Update is called once per frame
	void Update () {

//		RaycastHit hit;
//		Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
//		Debug.DrawRay (ray2.origin, ray2.direction * 10000, Color.yellow);
//		if(Physics.Raycast(ray2, out hit, 10000f)){
//			if(hit.collider.gameObject.CompareTag("Terrain")){
//				Hex ret = getMouseHex(new Vector3(hit.point.x,hit.point.z,0));
//				showTile = true;
//				tileInfo = getTileInfo(map.terrain,(int)ret.pos.x, (int)ret.pos.y);
//			}else{
//				showTile = false;
//			}
//		}
	
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


		addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
		addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
		addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
		addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
		addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
		addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
		addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
		
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
		
//		if (hy == 0 && hx == 0) {
//
//			addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//			addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
//			addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//			addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//			addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//			addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//			addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//
//			addTriangles(hx,hy);
//			addColor(hx,hy,7);
//
//		} else if (hy == 0 && hx != 0) {
//
//			
//			map.terrain[hx,hy].vertices[1] = map.terrain[hx-1, hy].vertices[2];
//			map.terrain[hx, hy].vertices[4] = map.terrain[hx - 1, hy].vertices[5];
//			addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//			addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
//			addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//			addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//			addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//			addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//			addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//
//			addTriangles(hx,hy);
//			addColor(hx,hy,7);
//				
//
//		} else if (!odd) {
//				//even hex in y direc
//
//			if (hy != 0 && hx == 0) {
//
//				
//				map.terrain[hx, hy].vertices [0] = map.terrain[hx, hy - 1].vertices [4];
//				map.terrain[hx, hy].vertices [2] = map.terrain[hx, hy - 1].vertices [6];
//				addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//				addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
//				addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//				addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//				addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//				addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//				addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//
//				addTriangles(hx,hy);
//				addColor(hx,hy,7);
//
//			} else if (hy != 0 && hx != 0) {
//
//				map.terrain[hx, hy].vertices [0] = map.terrain[hx-1, hy - 1].vertices [5];
//				map.terrain[hx, hy].vertices [1] = map.terrain[hx - 1, hy].vertices [2];
//				map.terrain[hx, hy].vertices [2] = map.terrain[hx, hy - 1].vertices[6];
//				map.terrain[hx, hy].vertices[4] = map.terrain[hx - 1, hy].vertices[5];
//				addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//				addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
//				addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//				addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//				addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//				addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//				addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//
//				addTriangles(hx,hy);
//				addColor(hx,hy,7);	
//
//			} else if (hx != 0 && hy != num_col - 1) {
//					print ("[ERROR] uncaught scenario in hex make: Code 1");
//					return;
//
//			} else {
//					print ("[ERROR] uncaught scenario in hex make: Code 2");
//					return;
//			}
//
//		} else {
//				//odd hex in y direc
//
//				if (hy != 0 && hx == 0 && hx != num_row - 1) {	
//
//						map.terrain[hx, hy].vertices [0] = map.terrain[hx, hy - 1].vertices [5];
//						map.terrain[hx, hy].vertices [1] = map.terrain[hx, hy - 1].vertices [6];
//						map.terrain[hx, hy].vertices [2] = map.terrain[hx + 1, hy - 1].vertices [6];
//
//						addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//						addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
//						addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//						addVertex(map.terrain[hx, hy].vertices[3],hx,hy,3);
//						addVertex(map.terrain[hx, hy].vertices[4],hx,hy,4);
//						addVertex(map.terrain[hx, hy].vertices[5],hx,hy,5);
//						addVertex(map.terrain[hx, hy].vertices[6],hx,hy,6);
//						addTriangles(hx,hy);
//						addColor(hx,hy,7);				
//		
//				} else if (hy!=0 && hx==num_row-1 && hx==0){
//
//				map.terrain[hx,hy].vertices[0] = map.terrain[hx,hy-1].vertices[5];
//				map.terrain[hx,hy].vertices[1] = map.terrain[hx,hy-1].vertices[6];
//				addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//				addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
//				addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//				addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//				addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//				addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//				addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//				addTriangles(hx,hy);	
//				addColor(hx,hy,7);
//
//				} else if(hy!=0 && hx == num_row-1){
//
//					map.terrain[hx,hy].vertices[0] = map.terrain[hx,hy-1].vertices[5];
//					map.terrain[hx,hy].vertices[1] = map.terrain[hx,hy-1].vertices[6];
//					map.terrain[hx,hy].vertices[4] = map.terrain[hx-1,hy].vertices[5];
//					addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//					addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
//					addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//					addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//					addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//					addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//					addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//					addTriangles(hx,hy);
//					addColor(hx,hy,7);
//		
//				} else if(hy!=0 && hx!=num_row-1){
//
//					map.terrain[hx,hy].vertices[0] = map.terrain[hx,hy-1].vertices[5];
//					map.terrain[hx,hy].vertices[1] = map.terrain[hx,hy-1].vertices[6];
//					map.terrain[hx, hy].vertices [2] = map.terrain[hx+1,hy-1].vertices[6];
//					map.terrain[hx,hy].vertices[4] = map.terrain[hx-1,hy].vertices[5];
//					addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
//					addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
//					addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
//					addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
//					addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
//					addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
//					addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
//					addTriangles(hx,hy);
//					addColor(hx,hy,7);
//		
//				} else{
//					print ("uncaught scenario in hex make");
//					return;
//				}
//
//		}

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
		/*Color rand = new Color( UnityEngine.Random.Range(0.0f, 1.0f),
			UnityEngine.Random.Range(0.0f,1.0f),
			UnityEngine.Random.Range(0.0f,1.0f), 
			UnityEngine.Random.Range(0.0f,1.0f));*/
//		Color color = new Color();
//
//		if (map.terrain [x, y].type == "water") {
//			color = Color.cyan;
//		} else if (map.terrain [x, y].type == "sand") {
//			color = Color.yellow;
//		}else if (map.terrain [x, y].type == "grass") {
//			color = Color.green;
//		}else if (map.terrain [x, y].type == "rock") {
//			color = Color.gray;
//		}else if (map.terrain [x, y].type == "snow") {
//			color = Color.white;
//		} else {
//			color = Color.magenta;
//		}

		for (int i=0; i < count; i++) {
			Color tri = ColorGenerator.getColorFromString (map.terrain [x, y].type);
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
	
}
