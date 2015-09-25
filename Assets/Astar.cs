using System.Collections.Generic;
using UnityEngine;

public class Astar {



	public static List<Hex> findPath(Hex start, Hex end){
		List<Hex> path = new List<Hex>();
		Hex temp = end;
			//Always false
			while (temp.center != end.center) {
			float currentDistance = Vector3.Distance(temp.center, end.center);
			Hex currentHex = temp;
				foreach(Hex hex in start.getNeighbors()){
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