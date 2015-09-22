using UnityEngine;
using System.Collections;

public class ColorGenerator : MonoBehaviour {

	public static Color[] deep_water = new Color[]{ new Color(0f/255f,152f/255f,186f/255f), new Color(29f/255f,169f/255f,248f/255f)};
	public static Color[] shallow_water = new Color[]{new Color(125f/255f,215f/255f,235f/255f)};
	public static Color[] sand = new Color[]{new Color(245f/255f,221f/255f,173f/255f)};
	public static Color[] desert = new Color[]{new Color(240f/255f,166f/255f,120f/255f)};
	public static Color[] grass = new Color[]{new Color(131f/255f,168f/255f,44f/255f)};
	public static Color[] plains = new Color[]{new Color(225f/255f,223f/255f,42f/255f)};
	public static Color[] forest = new Color[]{new Color(47f/255f,143f/255f,36f/255f)};
	public static Color[] tundra = new Color[]{new Color(204f/255f,165f/255f,149f/255f)};
	public static Color[] rock = new Color[]{new Color(110f/255f,106f/255f,105f/255f)};
	public static Color[] snow = new Color[]{new Color(255f/255f,255f/255f,255f/255f)};
	public static Color[] marsh = new Color[]{new Color(85f/255f,212f/255f,159f/255f)};
	public static Color[] jungle = new Color[]{new Color(33f/255f,115f/255f,13f/255f)};

	//Units
	public static Color[] unit_marker_basic = new Color[]{new Color(242f/255f,207f/255f,170f/255f)};

	public static int deep_water_it, shallow_water_it, sand_it, grass_it, plains_it, forest_it, tundra_it, rock_it, snow_it = -1;

	public static Color getColorFromString(string color_string){
		switch (color_string)
		{

		case "water":
			return getDeepWater ();
		case "deep_water":
			return getDeepWater ();
		case "shallow_water":
			return getShallowWater ();
		case "sand":
			return getSand ();
		case "grass":
			return getGrass ();
		case "plains":
			return getPlains ();
		case "forest":
			return getForest ();
		case "tundra":
			return getTundra ();
		case "rock":
			return getRock ();
		case "snow":
			return getSnow ();
		case "desert":
			return getDesert ();
		case "marsh":
			return getMarsh ();
		case "jungle":
			return getJungle();
		case "unit_marker_basic":
			return getUnitMarkerBasic();
		default:
			return Color.magenta;
		}
	}

	public static Color getDeepWater(){
		return deep_water[Random.Range(0, deep_water.Length)];
	}
	public static Color getShallowWater(){
		return shallow_water[Random.Range(0, shallow_water.Length)];
	}
	public static Color getSand(){
		return sand[Random.Range(0, deep_water.Length-1)];
	}
	public static Color getGrass(){
		return grass[Random.Range(0, grass.Length)];;
	}
	public static Color getPlains(){
		return plains[Random.Range(0, plains.Length)];
	}
	public static Color getForest(){
		return forest[Random.Range(0, forest.Length)];
	}
	public static Color getTundra(){
		return tundra[Random.Range(0, tundra.Length)];;
	}
	public static Color getRock(){
		return rock[Random.Range(0, rock.Length)];
	}
	public static Color getSnow(){
		return snow[Random.Range(0, snow.Length)];;
	}
	public static Color getDesert(){
		return desert[Random.Range(0, desert.Length)];;
	}
	public static Color getMarsh(){
		return marsh[Random.Range(0, marsh.Length)];;
	}
	public static Color getJungle(){
		return jungle[Random.Range(0, jungle.Length)];;
	}
	public static Color getUnitMarkerBasic(){
		return unit_marker_basic [Random.Range (0, unit_marker_basic.Length)];
	}

}
