using UnityEngine;
using System;

public class UIManager : MonoBehaviour {

	UIState currentState;

	void Start(){
		currentState = UIState.NONE;
	}

	void setUIState(UIState newState){
		currentState = newState;
	}

	void OnGUI()
	{
		if(currentState == UIState.NONE){

		}else if(currentState == UIState.UNIT){

		}else if(currentState == UIState.CITY){

		}
	}
}