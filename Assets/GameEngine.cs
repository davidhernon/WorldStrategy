using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

	public static Map map;
	public static int num_row;
	public static int num_col;
	private static Mesh mesh;

	public static bool show_tile;

	Player[] players;
	int player = 0;
	bool show_culture = false;
	bool hoverUI = false;
	
	public static Hex selected_hex;
	public static Hex hover_hex;

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
		mesh = World7.mesh;

		Hex hex = map.terrain [Random.Range (0, num_row - 1), Random.Range (0, num_col - 1)];
		while (!GameUtils.isLand(hex)) {
			hex = map.terrain [Random.Range (0, num_row - 1), Random.Range (0, num_col - 1)];
		}
		players [1].setupUnits (map, hex);
		hover_hex = new Hex ();
	}
	
	// Update is called once per frame
	void Update () {
		bool hasMoves = false;
		for (int i=0; i<players[player].units.Length; i++) {
			if(players[player].units[i].moves > 0){
				hasMoves = true;
			}
		}

		if (hasMoves == false) {
			endTurn();
		}
		RaycastHit hoverhit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hoverhit, 10000f)) {

			if(hoverhit.collider.gameObject.CompareTag("Terrain")){
				Hex hover = GameUtils.getHexFromPoint(new Vector3(hoverhit.point.x,hoverhit.point.z,0), map, num_row, num_col);
				hover_hex = hover;
				hoverUI = true;
			}else{
				hoverUI = false;
			}
		}


		if (Input.GetMouseButtonUp (1)) {
			RaycastHit hit;
			Ray ray2 = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(Physics.Raycast(ray2, out hit, 10000f)){
				if(hit.collider.gameObject.CompareTag("Terrain")){
					Hex right_clicked_hex = GameUtils.getHexFromPoint(new Vector3(hit.point.x,hit.point.z,0), map, num_row, num_col);
					if(selected_hex.hasUnit())
					{
						selected_hex.unit.move (right_clicked_hex);
//						selected_hex = null;
//						selected_hex = right_clicked_hex;
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

		if (GUI.Button (new Rect (10, Screen.height - 200, 130, 30), "Culture")) {

			if(show_culture == true){
				show_culture = false;
			}else{
				show_culture = true;
			}
		}

		if (show_culture) {
			Rect temp = getRect(200,300);
			GUI.Box (temp,players[player].playerName+"\'s Culture\n\n\n" + players[player].culture.cultureToString());
		}

		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 40, 130, 30), "End Turn")) {
			endTurn();
		}

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

		if (hoverUI) {

			string hovertext = "" + hover_hex.getTileInfo();
			if(hover_hex.hasUnit()){
				hovertext += "\n*--UNIT---*\n" + hover_hex.unit.name + "\nHealth: " + hover_hex.unit.health + "\nStrength: " + hover_hex.unit.strength + "\nDefend: " + hover_hex.unit.defend;
			}
			GUI.Box (new Rect(Screen.width - 140, Screen.height - 400, 130, 200), hovertext);

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

	Rect getRect(int height, int width){
		return new Rect ((Screen.width / 2) - (width / 2), (Screen.height / 2) - (height / 2), width, height);
	}

	void endTurn(){
		players[player].endPlayersTurn();
		player = (player+1)%(players.Length);
		players[player].startPlayersTurn();
		if(players[player].playerName == "Nature"){
			players[player].endPlayersTurn();
			player = (player+1)%(players.Length);
			players[player].startPlayersTurn();
		}
	}



}
