using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class World7 : MonoBehaviour {

	public static Map map;
	private Mesh mesh;
	private GameObject mesh_terrain;
	public static int num_row = 63; //75, 63
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

	// Use this for initialization
	void Start () {
		
		num_col = (int)(num_row*col_stretch);
		mesh_terrain = GameObject.Find("Terrain");
		mesh = GetComponent<MeshFilter> ().mesh;

		map = new Map(num_row, num_col);
		map.initHexes();
		map.generateHeightMap();

		map.createHexBoard();
		map.setHexType();

		createMesh();
		buildMesh();

		MeshCollider meshc = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshc.sharedMesh = mesh; // Give it your mesh here.
		//sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		//sphere1.collider.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
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
		
		if (hy == 0 && hx == 0) {

			addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
			addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
			addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
			addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
			addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
			addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
			addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);

			addTriangles(hx,hy);
			addColor(hx,hy,7);

		} else if (hy == 0 && hx != 0) {

			
			map.terrain[hx,hy].vertices[1] = map.terrain[hx-1, hy].vertices[2];
			map.terrain[hx, hy].vertices[4] = map.terrain[hx - 1, hy].vertices[5];
			addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
			addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
			addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
			addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
			addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
			addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
			addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);

			addTriangles(hx,hy);
			addColor(hx,hy,7);
				

		} else if (!odd) {
				//even hex in y direc

			if (hy != 0 && hx == 0) {

				
				map.terrain[hx, hy].vertices [0] = map.terrain[hx, hy - 1].vertices [4];
				map.terrain[hx, hy].vertices [2] = map.terrain[hx, hy - 1].vertices [6];
				addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
				addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
				addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
				addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
				addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
				addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
				addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);

				addTriangles(hx,hy);
				addColor(hx,hy,7);

			} else if (hy != 0 && hx != 0) {

				map.terrain[hx, hy].vertices [0] = map.terrain[hx-1, hy - 1].vertices [5];
				map.terrain[hx, hy].vertices [1] = map.terrain[hx - 1, hy].vertices [2];
				map.terrain[hx, hy].vertices [2] = map.terrain[hx, hy - 1].vertices[6];
				map.terrain[hx, hy].vertices[4] = map.terrain[hx - 1, hy].vertices[5];
				addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
				addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
				addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
				addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
				addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
				addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
				addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);

				addTriangles(hx,hy);
				addColor(hx,hy,7);	

			} else if (hx != 0 && hy != num_col - 1) {
					print ("[ERROR] uncaught scenario in hex make: Code 1");
					return;

			} else {
					print ("[ERROR] uncaught scenario in hex make: Code 2");
					return;
			}

		} else {
				//odd hex in y direc

				if (hy != 0 && hx == 0 && hx != num_row - 1) {	

						map.terrain[hx, hy].vertices [0] = map.terrain[hx, hy - 1].vertices [5];
						map.terrain[hx, hy].vertices [1] = map.terrain[hx, hy - 1].vertices [6];
						map.terrain[hx, hy].vertices [2] = map.terrain[hx + 1, hy - 1].vertices [6];

						addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
						addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
						addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
						addVertex(map.terrain[hx, hy].vertices[3],hx,hy,3);
						addVertex(map.terrain[hx, hy].vertices[4],hx,hy,4);
						addVertex(map.terrain[hx, hy].vertices[5],hx,hy,5);
						addVertex(map.terrain[hx, hy].vertices[6],hx,hy,6);
						addTriangles(hx,hy);
						addColor(hx,hy,7);				
		
				} else if (hy!=0 && hx==num_row-1 && hx==0){

				map.terrain[hx,hy].vertices[0] = map.terrain[hx,hy-1].vertices[5];
				map.terrain[hx,hy].vertices[1] = map.terrain[hx,hy-1].vertices[6];
				addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
				addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
				addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
				addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
				addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
				addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
				addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
				addTriangles(hx,hy);	
				addColor(hx,hy,7);

				} else if(hy!=0 && hx == num_row-1){

					map.terrain[hx,hy].vertices[0] = map.terrain[hx,hy-1].vertices[5];
					map.terrain[hx,hy].vertices[1] = map.terrain[hx,hy-1].vertices[6];
					map.terrain[hx,hy].vertices[4] = map.terrain[hx-1,hy].vertices[5];
					addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
					addVertex(map.terrain[hx,hy].vertices[1], hx,hy,1);
					addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
					addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
					addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
					addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
					addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
					addTriangles(hx,hy);
					addColor(hx,hy,7);
		
				} else if(hy!=0 && hx!=num_row-1){

					map.terrain[hx,hy].vertices[0] = map.terrain[hx,hy-1].vertices[5];
					map.terrain[hx,hy].vertices[1] = map.terrain[hx,hy-1].vertices[6];
					map.terrain[hx, hy].vertices [2] = map.terrain[hx+1,hy-1].vertices[6];
					map.terrain[hx,hy].vertices[4] = map.terrain[hx-1,hy].vertices[5];
					addVertex(map.terrain[hx,hy].vertices[0],hx,hy,0);
					addVertex(map.terrain[hx,hy].vertices[1],hx,hy,1);
					addVertex(map.terrain[hx,hy].vertices[2],hx,hy,2);
					addVertex(map.terrain[hx,hy].vertices[3],hx,hy,3);
					addVertex(map.terrain[hx,hy].vertices[4],hx,hy,4);
					addVertex(map.terrain[hx,hy].vertices[5],hx,hy,5);
					addVertex(map.terrain[hx,hy].vertices[6],hx,hy,6);
					addTriangles(hx,hy);
					addColor(hx,hy,7);
		
				} else{
					print ("uncaught scenario in hex make");
					return;
				}

		}

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
		Color color = new Color();

		if(map.terrain[x,y].type == "land"){
			color = Color.green;
		}else if(map.terrain[x,y].type == "water"){
			color = Color.cyan;
		}

		for(int i=0; i < count; i++){
			newColor.Add(color);
		}
	}

	public static void addVertex(Vector3 vert, int x, int y, int pos){

		newVertices.Add (vert);
		map.terrain[x,y].id_list[pos] = vertex_count; 
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


// End of Class
}
