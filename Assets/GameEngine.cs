using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

	public static Map map;
	public static int num_row;
	public static int num_col;

	public static bool show_tile;

	Player[] players;
	int player = 0;
	
	public static Hex selected_hex;

	// Use this for initialization
	void Start () {
		Screen.SetResolution(1920, 1080, true, 60);
		GameEngine.num_row = World7.num_row;
		GameEngine.num_col = World7.num_col;
		GameEngine.map = World7.map;
		players = new Player[2];
		players [0] = new Nature ("Nature");
		players [1] = new Human ("Human");
		players [0].setupUnits (map);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonUp (1)) {
			RaycastHit hit;
			Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast(ray2, out hit, 10000f)){
				if(hit.collider.gameObject.CompareTag("Terrain")){
					Hex right_clicked_hex = GameUtils.getHexFromPoint(new Vector3(hit.point.x,hit.point.z,0), map, num_row, num_col);
					if(selected_hex.hasUnit())
					{
						selected_hex.unit.move (right_clicked_hex);
						selected_hex = null;
						selected_hex = right_clicked_hex;
					}else{
//
					}

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
		if (Physics.Raycast (ray2, out hit, 10000f)) {
			if (hit.collider.gameObject.CompareTag ("Terrain")) {
				selected_hex = GameUtils.getHexFromPoint (new Vector3 (hit.point.x, hit.point.z, 0), map, num_row, num_col);
				show_tile = true;
			} else {
				//clicked but didnt connect with terrain
			}
		} else {
		}

	}

	void OnGUI(){
		
		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 40, 130, 30), "End Turn")) {
			players[player].endPlayersTurn();
			player = (player+1)%(players.Length);
			players[player].startPlayersTurn();
		}
//		if(GUI.Button(new Rect(Screen.width-130,Screen.height - 90,130,30), "Spawn Unit")) {
//			Vector2 loc = selected_hex.pos;
//			players[player].setupUnits(map, loc);
//		}
		if (show_tile) {
			GUI.Box(new Rect(10, 10, 130, 90), selected_hex.getTileInfo());
		}

		string bottom_left = "";
		if (player == 0) {
		bottom_left	= "Player: " + players [player].playerName + "\nUnits: " + ((Nature)players[0]).total_units + " " + "\nAvailable: 0";

		} else {
			bottom_left	= "Player: " + players [player].playerName + "\nUnits: " + ((Human)players[1]).units.Length + "\nAvailable: " + ((Human)players[1]).available;
		}
		if (selected_hex != null && selected_hex.unit != null) {
			bottom_left += "\n*---Unit---*\n" + selected_hex.unit.getInfo ();
		}
//		string final = bottom_left + "\n*---Unit---*\n" + selected_hex.getTileInfo ();

		GUI.Box (new Rect (10, Screen.height - 160, 130, 150), bottom_left);

		if (selected_hex !=null && player == 1 && players[1].available > 0) {
			if(GUI.Button (new Rect(10, 110, 125, 30), "Spawn Unit")){
				setupUnitsOnHex();
			}
		}

	}

	void setupUnitsOnHex(){
		players[player].setupUnits(map, selected_hex);
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
