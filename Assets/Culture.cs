using UnityEngine;
using System.Collections;

public class Culture {

	public int charity_lust = 5;
	public int temperance_gluttony = 5;
	public int charity_greed = 5;
	public int diligence_sloth = 5;
	public int kindness_envy = 5;
	public int humility_pride = 5;
	public int patience_wrath = 5;



	public Culture(){

	}

	public string cultureToString(){
		return "charity/lust : " + charity_lust + "/10\n"
			+ "temperance/gluttonyt : " + temperance_gluttony + "/10\n"
				+ "charity/greed : " + charity_greed + "/10\n"
				+ "diligence/sloth : " + diligence_sloth + "/10\n"
				+ "kindness/envy : " + kindness_envy + "/10\n"
				+ "humility/pride : " + humility_pride + "/10\n"
				+ "patience/wrath : " + patience_wrath + "/10\n";
	}
}
