using System.Collections.Generic;
using UnityEngine;

public class Astar {



	public static List<Hex> findPath(Hex start, Hex end){
		Debug.Log("help");
		List<Hex> path = new List<Hex>();
		Hex temp = end;
			//Always false
		Debug.Log ("vectorL: " + temp.center + " and " + end.center);
			while (temp.center != end.center) {
			Debug.Log ("do we get in here?");
			float currentDistance = Vector3.Distance(temp.center, end.center);
			Hex currentHex = temp;
				foreach(Hex hex in start.getNeighbors()){
					Debug.Log("inside processing neighbors");
					float distance = Vector3.Distance (hex.center, end.center);
					if(distance < currentDistance){
						distance = currentDistance;
						currentHex = hex;
					}
				}
			path.Add (currentHex);		
			}
		return path;
	}

}