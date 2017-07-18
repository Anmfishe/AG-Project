using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *                                      ***     !!!!!!     IMPORTANT   !!!!!!   ***
 *                                      
 *                                       Make sure GameObject Scoreboard has tag 'Scoreboard' 
 *                  
 *                                       AND NOTHING ELSE SHOULD BE TAGGED WITH SCOREBOARD
 *                                                          
 *                                      because script PlayerStatus looks for it!
 *  
 *                                      ***     !!!!!!     IMPORTANT   !!!!!!     ***
 *  
 *  
 *  Used by NetworkManager (to instantiate once in scene [in masterclient]) and PlayerStatus (to update scoreboard when a player is defeated)
 */

public class ScoreboardUpdater : MonoBehaviour {

	public GameObject red_score_for_red_view;
	public GameObject blue_score_for_red_view;
    public GameObject red_score_for_blue_view;
    public GameObject blue_score_for_blue_view;

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
        red_score_for_red_view.GetComponent<TextMesh> ().text = "0";
        blue_score_for_red_view.GetComponent<TextMesh> ().text = "0";
        red_score_for_blue_view.GetComponent<TextMesh> ().text = "0";
        blue_score_for_blue_view.GetComponent<TextMesh> ().text = "0";

		red_score = 0;
		blue_score = 0;
	}

	public void IncrementRedScore()
	{
		++red_score;
        red_score_for_red_view.GetComponent<TextMesh> ().text = "" + red_score;
        red_score_for_blue_view.GetComponent<TextMesh> ().text = "" + red_score;
	}

	public void IncrementBlueScore()
	{
		++blue_score;
        blue_score_for_red_view.GetComponent<TextMesh> ().text = "" + blue_score;
        blue_score_for_blue_view.GetComponent<TextMesh> ().text = "" + blue_score;
	}
}
