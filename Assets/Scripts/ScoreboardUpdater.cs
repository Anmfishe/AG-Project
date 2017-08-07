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

    public int red_score = 0;
	public int blue_score = 0;

	public bool roundOver = false;

    private PhotonView pv;
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        red_score_for_red_view.GetComponent<TextMesh>().text = "0";
        blue_score_for_red_view.GetComponent<TextMesh>().text = "0";
        red_score_for_blue_view.GetComponent<TextMesh>().text = "0";
        blue_score_for_blue_view.GetComponent<TextMesh>().text = "0";

        
    
}
// Use this for initialization
void Start() {
}
		
	
	// Update is called once per frame
	void Update () {
       
	}
    
    public void ResetScoreboard()
    {
        pv.RPC("Reset2", PhotonTargets.AllBuffered, null);
    }
    public void IncrementRedScore()
    {
        pv.RPC("IncrementRedScore2", PhotonTargets.AllBuffered, null);
    }
    public void IncrementBlueScore()
    {
        pv.RPC("IncrementBlueScore2", PhotonTargets.AllBuffered, null);
    }
    [PunRPC]
    public void Reset2()
	{
        Debug.Log("RPC Scoreboard Reset");

        red_score_for_red_view.GetComponent<TextMesh>().text = "0";
            blue_score_for_red_view.GetComponent<TextMesh>().text = "0";
            red_score_for_blue_view.GetComponent<TextMesh>().text = "0";
            blue_score_for_blue_view.GetComponent<TextMesh>().text = "0";

            red_score = 0;
            blue_score = 0;
            roundOver = true;
        
    }
    [PunRPC]
    public void IncrementRedScore2()
	{

        
            ++red_score;
            red_score_for_red_view.GetComponent<TextMesh>().text = "" + red_score;
            red_score_for_blue_view.GetComponent<TextMesh>().text = "" + red_score;
        Debug.Log("RPC RED SCORED: " + red_score);
    }
    [PunRPC]
    public void IncrementBlueScore2()
	{
        


        ++blue_score;
            blue_score_for_red_view.GetComponent<TextMesh>().text = "" + blue_score;
            blue_score_for_blue_view.GetComponent<TextMesh>().text = "" + blue_score;
        Debug.Log("RPC BLUE SCORED: " + blue_score);

    }
}
