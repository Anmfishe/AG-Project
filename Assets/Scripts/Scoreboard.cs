using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour {

	public GameObject[] red_score_display;
	public GameObject[] blue_score_display;

	int red_score;
	int blue_score;

	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Reset()
	{
		red_score_display [0].GetComponent<TextMesh> ().text = "1";
		red_score_display [1].GetComponent<TextMesh> ().text = "1";
		blue_score_display [0].GetComponent<TextMesh> ().text = "1";
		blue_score_display [1].GetComponent<TextMesh> ().text = "1";

		red_score = 0;
		blue_score = 0;
	}

	public void IncrementRedScore()
	{
		++red_score;
		red_score_display [0].GetComponent<TextMesh> ().text = "" + red_score;
		red_score_display [1].GetComponent<TextMesh> ().text = "" + red_score;
	}

	public void IncrementBlueScore()
	{
		++blue_score;
		blue_score_display [0].GetComponent<TextMesh> ().text = "" + blue_score;
		blue_score_display [1].GetComponent<TextMesh> ().text = "" + blue_score;
	}
}
