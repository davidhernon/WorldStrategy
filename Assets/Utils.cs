using System;
using UnityEngine;

public class Utils
{

	public static float distance(float x1, float y1, float x2, float y2){
		return Mathf.Sqrt(Mathf.Pow((float)x2-x1,2) + Mathf.Pow((float)y2-y1,2));
	}

}


