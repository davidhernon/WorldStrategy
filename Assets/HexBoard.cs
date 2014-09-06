using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;

public class HexBoard {

	public List<Vector3> newVertices = new List<Vector3> ();
	public List<int> newTriangles = new List<int> ();
	public List<Color> newColor = new List<Color> ();
	public  Map map;

	public  int num_row = 64;
	public  int num_col = 0;
	private  int x_off = 0;
	private  int y_off = 0;
	private  int cur_x = 0;
	private  int cur_y = 0;
	private  int vertex_count = 0;

	public HexBoard(){

	}

	public void createMesh(){
			for (int i=0; i<num_col; i++) {
					for (int j=0; j<num_row; j++) {
						if(i % 2 == 0){
							this.addHexToMesh(i,j, false);
						}else{
							this.addHexToMesh(i,j, true);
						}
					}
			}
		}

	private void addHexToMesh(int y, int x, bool odd){

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
					Debug.Log ("[ERROR] uncaught scenario in hex make: Code 1");

			} else {
					Debug.Log ("[ERROR] uncaught scenario in hex make: Code 2");
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
					Debug.Log ("[ERROR] uncaught scenario in hex make");
				}

			}

		}

		public void addTriangles(int x, int y){

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

	public void addColor(int x, int y, int count){
		/*Color rand = new Color( UnityEngine.Random.Range(0.0f, 1.0f),
			UnityEngine.Random.Range(0.0f,1.0f),
			UnityEngine.Random.Range(0.0f,1.0f), 
			UnityEngine.Random.Range(0.0f,1.0f));*/
		Color rand = Color.green;
		for(int i=0; i < count; i++){
			newColor.Add(rand);
		}
	}

	public void addVertex(Vector3 vert, int x, int y, int pos){

		newVertices.Add (vert);
		map.terrain[x,y].id_list[pos] = vertex_count; 
		vertex_count++;

	}

}