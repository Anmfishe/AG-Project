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
    private List<GameObject> playerRigs = new List<GameObject>();
    private List<GameObject> players = new List<GameObject>();
    ScoreboardUpdater scoreboard;
    int score;

    //TODO score ssystem, if you want it to end the round
    // Use this for initialization
    void Start() {
        if (GameObject.FindGameObjectWithTag("Scoreboard")) {
            scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();
        } else {
            print("COULD NOT FIND SCOREBOARD");
        }


        hatRoom = GameObject.FindGameObjectWithTag("HatRoom").GetComponent<Transform>();
        ChooseHats();
    }

    // Update is called once per frame
    void Update() {
        //all this has to ce re-done according to whatever you want the round to be. Does the round start after the 1st player puts hat on? Does it start when certain ammount of people do that? 
        if (!hatsSelected)
        {
            foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("Player"))
            {

                if (playerRCP.GetComponent<PlayerStatus>().playerClass == PlayerClass.none)
                    break;
                hatsSelected = true;

            }
        }
        else if (hatsSelected && !inBattlefield)
        {
            //            StartRound();
        }
        else if (inBattlefield)
        {
          
            timeElapsed += Time.deltaTime;

            if (scoreboard == null)
            {
                scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();
            }
            if ((isScoreBased && (scoreboard.red_score >= maxScore || scoreboard.blue_score >= maxScore)) || (isTimeBased && timeElapsed >= roundTime))
            {
                print(scoreboard.red_score + " | " + scoreboard.blue_score + " | " + maxScore);
                print("ROUND HAS ENDED");
                EndRound();
            }
        }
    }
    /// <summary>
    /// Call from a player once it is killed
    /// </summary>
    void OnPlayerKilled()
    {
        UpdateScoreboard();
        score++;
    }
    void UpdateScoreboard() { }
  

    void EndRound()
    {
        Camera.main.transform.parent.GetComponent<PlatformController>().enabled = false;
        print("ROUND ENDED, SHOULD HAVE TURNED OFF PLATFORMCONTROLLER");
//        FindPlayers ();
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
        score = 0;
        timeElapsed = 0;
 //       FindPlayers();
        //TODO send players to the battlefield
        /*foreach (GameObject pl in playerPCP)
            pl.GetComponent<TeamManager>().Respawn();*/
        inBattlefield = true;

    }

    void ChooseHats()
    {
        Camera.main.transform.parent.GetComponent<PlatformController>().enabled = false;
//        FindPlayers();
        foreach (GameObject player in playerRigs)
        {
            Vector3 newPos = hatRoom.position;
            Transform camObj = player.GetComponentInChildren<Camera>().transform;
            newPos.x -= camObj.localPosition.x;
            newPos.z -= camObj.localPosition.z;
            player.GetComponent<Transform>().SetPositionAndRotation(newPos, player.GetComponent<Transform>().rotation);
            player.GetComponent<Edwon.VR.VRGestureRig>().enabled = false;
            player.GetComponent<SpellcastingGestureRecognition>().enabled = false;

        }
        //foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("Player"))
        //    //player.TakeOffHat();

            hatsSelected = false;

        if (GameObject.Find("RightController") == null)
        {
            return;
        }
        GameObject.Find("RightController").GetComponent<VRTK.VRTK_StraightPointerRenderer>().enabled = true;
    }

    void SendPlayerToHatRoom(GameObject player)
    {
        Vector3 newPos = hatRoom.position;
        Transform camObj = player.GetComponentInChildren<Camera>().transform;
        newPos.x -= camObj.localPosition.x;
        newPos.z -= camObj.localPosition.z;
        player.GetComponent<Transform>().SetPositionAndRotation(newPos, player.GetComponent<Transform>().rotation);

    }



    void ShowScoreboard()
    {
        //TODO show whatever AG like to show in the end of round 
    }
    public void Subscribe(GameObject go, GameObject rig)
    {
        Debug.Log(go.name + " subscribed");
        players.Add(go);
        playerRigs.Add(rig);
        //send 
        SendPlayerToHatRoom(rig);
    }

    public void Unsubscribe(GameObject go, GameObject rig)
    {
        players.Remove(go);
        players.Remove(rig);
        Debug.Log(go.name + " unsubscribed");
    }
}
