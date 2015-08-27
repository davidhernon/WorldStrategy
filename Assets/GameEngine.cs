using UnityEngine;
using System.Collections;

public class GameEngine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	Debug.Log("here");	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		
			GUI.Box(new Rect(0, 10, 130, 90), "String");	
	}
}
