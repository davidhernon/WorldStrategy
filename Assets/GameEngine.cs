using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

	public static Map map;
	public static int num_row;
	public static int num_col;

	public GameObject smoke;

	Player[] players = new Player[1];
	int player = 0;

	//
	public static GameObject selected_cell_marker;
	public static Hex selected_hex;

	// Use this for initialization
	void Start () {

		players[0] = new Human("Player One");
//		players[1] = new AI("Player Two");

		selected_cell_marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		selected_cell_marker.GetComponent<Collider>().enabled = false;

		GameEngine.num_row = World7.num_row;
		GameEngine.num_col = World7.num_col;
		GameEngine.map = World7.map;
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (1)) {
			RaycastHit hit;
			Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (ray2.origin, ray2.direction * 10000, Color.yellow);
			if(Physics.Raycast(ray2, out hit, 10000f)){
				if(hit.collider.gameObject.CompareTag("Terrain")){
					Hex ret = GameUtils.getHexFromPoint(new Vector3(hit.point.x,hit.point.z,0), map, num_row, num_col);
					if(selected_hex.unit != null)
					{
						Debug.Log ("was not null");
						Unit unit = selected_hex.unit;
											Debug.Log ("Printing from Right Click Before If: " + unit.toString());
											unit.move (ret.center, ret);
											Debug.Log ("Printing from Right Click After If: " + unit.toString());
											selected_hex.unit = null;
											selected_hex = ret;

					}else{
						Debug.Log ("was null");
						selected_hex = ret;
					}


					//grab new hex
					//grab old hex
					//move unit from hex 1 to hex 2
					// remove references to old hex
					//set new hex to old

				}else{
				}
			}
		}
	
	}

	void OnMouseDown()
	{
		RaycastHit hit;
		Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (ray2.origin, ray2.direction * 10000, Color.yellow);
		if(Physics.Raycast(ray2, out hit, 10000f)){
			if(hit.collider.gameObject.CompareTag("Terrain")){
				selected_hex = null;
				//if(selected_hex != null){Debug.Log ("Printing from Left Click before: " + selected_hex.unit.toString()); };
				selected_hex = GameUtils.getHexFromPoint(new Vector3(hit.point.x,hit.point.z,0), map, num_row, num_col);
				selected_cell_marker.transform.position = selected_hex.center;
//				selected_hex = ret;
				//GameObject newSmoke = (GameObject)Instantiate (smoke, ret.center, Quaternion.identity);
				//Debug.Log ("Printing from Left Click after: " + selected_hex.unit.toString()); 
			}else{
			}
		}



	}

	void OnGUI(){
		
		if (GUI.Button (new Rect (0, Screen.height - 40, 130, 30), "End Turn")) {
			players[player].endPlayersTurn();
			player = (player+1)%(players.Length);
		}
		if(GUI.Button(new Rect(10,110,130,30), "Spawn Unit")) {
			Vector2 loc = selected_hex.pos;
			players[player].setupUnits(map, loc);
		}

	}

	void SetMap(Map new_map)
	{
		map = new_map;
	}

	void SetRow(int row)
	{
		num_row = row;
	}

	void SetCol(int col)
	{
		num_col = col;
	}


}
