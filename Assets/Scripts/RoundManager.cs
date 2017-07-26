using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    
    public Transform hatRoom;
    public float roundTime;
	public bool isTimeBased = false;
	public int maxScore = 3;
	public bool isScoreBased = true;
    private float timeElapsed;
    private bool inBattlefield = true;
    private bool hatsSelected = false;
    private GameObject[] playerRigs;
    private GameObject[] players;
	ScoreboardUpdater scoreboard;

    //TODO score ssystem, if you want it to end the round
    // Use this for initialization
    void Start () {
		if (GameObject.FindGameObjectWithTag ("Scoreboard")) {
			scoreboard = GameObject.FindGameObjectWithTag ("Scoreboard").GetComponent<ScoreboardUpdater>();
		} else {
			print ("COULD NOT FIND SCOREBOARD");
		}


        hatRoom = GameObject.FindGameObjectWithTag("HatRoom").GetComponent<Transform>();
        FindPlayers();
		ChooseHats ();
    }
	
	// Update is called once per frame
	void Update () {
        //        if (Input.anyKeyDown)
        //           EndRound();
        if (inBattlefield)
        {
			timeElapsed += Time.deltaTime;

			if (scoreboard == null) {
				scoreboard = GameObject.FindGameObjectWithTag ("Scoreboard").GetComponent<ScoreboardUpdater>();
			}
			if ((isScoreBased && (scoreboard.red_score >= maxScore || scoreboard.blue_score >= maxScore)) || (isTimeBased && timeElapsed >= roundTime))
			{
				print (scoreboard.red_score + " | " + scoreboard.blue_score + " | " + maxScore);
				print ("ROUND HAS ENDED");
                EndRound();
			}
        }
        if (!hatsSelected)
        {
            foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("PCP"))
            {
                
				if (playerRCP.GetComponentInChildren<PlayerStatus>().playerClass == PlayerClass.none)
                    break;
                hatsSelected = true;   
             
            }
        }
        else if(hatsSelected && !inBattlefield)
        {
//            StartRound();
        }
	}

    /// <summary>
    /// We are using this shit every time before any allPlayers action cause we didn't go with observer
    /// </summary>
    void FindPlayers()
    {
        playerRigs = GameObject.FindGameObjectsWithTag("CameraRig");
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void EndRound()
    {
		FindPlayers ();
//		foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("PCP"))                                //TODO
//			playerRCP.GetComponentInChildren<PlayerStatus> ().takeOffHat ();
        ChooseHats();
        ShowScoreboard();
 //       inBattlefield = false;
		scoreboard.Reset ();
		timeElapsed = 0;
		print ("END OF ENDROUND");
    }

    void StartRound()
    {

        timeElapsed = 0;
        FindPlayers();
        //TODO send players to the battlefield
        /*foreach (GameObject pl in playerPCP)
            pl.GetComponent<TeamManager>().Respawn();*/
        inBattlefield = true;
    }

    void ChooseHats()
    {
        FindPlayers();
        foreach (GameObject player in playerRigs)
        {
            Vector3 newPos = hatRoom.position;
            Transform camObj = player.GetComponentInChildren<Camera>().transform;
            newPos.x -= camObj.localPosition.x;
            newPos.z -= camObj.localPosition.z;
            player.GetComponent<Transform>().SetPositionAndRotation(newPos, player.GetComponent<Transform>().rotation);

        }
        //foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("Player"))
        //    //player.TakeOffHat();

            hatsSelected = false;
        

    }

    void ShowScoreboard()
    {
        //TODO show whatever AG like to show in the end of round 
    }
    /*
    public void Subscribe(GameObject go)
    {
        players.Add(go);
        Debug.Log(go.name + " subscribed");
    }

    public void Unsubscribe(GameObject go)
    {
        players.Remove(go);
        Debug.Log(go.name + " unsubscribed");
    }
    */
}
