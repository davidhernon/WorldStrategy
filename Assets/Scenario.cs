using UnityEngine;
using System.Collections;

public class Scenario : MonoBehaviour {

	private string question = "";
	private string answer_one = "";
	private string answer_two = "";
	private string type = "";

	public Scenario(string question, string answer_one, string answer_two, string type){
		this.question = question;
		this.answer_one = answer_one;
		this.answer_two = answer_two;
	}

	public string getQuestion(){
		return this.question;
	}

	public string getAnswerOne(){
		return this.answer_one;
	}

	public string getAnswerTwo(){
		return this.answer_two;
	}
}
