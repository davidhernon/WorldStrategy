using UnityEngine;
using System.Collections;

public class World7 : MonoBehaviour {

	public static Map map;
	private Mesh mesh;
	private GameObject mesh_terrain;

	// Use this for initialization
	void Start () {
	
		mesh_terrain = GameObject.Find("Terrain");
		mesh = GetComponent<MeshFilter> ().mesh;

		map = new Map();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
