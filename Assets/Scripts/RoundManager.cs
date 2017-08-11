using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {


    public Transform hatRoom;
    public float roundTime;
    public bool isTimeBased = false;
    public int maxScore;
    public bool isScoreBased = true;
    private float timeElapsed;
    private bool inBattlefield = true;
    private bool hatsSelected = false;
    private List<GameObject> playerRigs = new List<GameObject>();
    private List<GameObject> players = new List<GameObject>();
    ScoreboardUpdater scoreboard;
    int score;
    private int blueMemb;
    private int redMemb;

    public GameObject practiceRoom;
    public GameObject arena;
    //
//    public GameObject countdown_display;
    public GameObject restart_display;
    public GameObject countdown_display;
    
    //TODO score ssystem, if you want it to end the round
    // Use this for initialization
    void Start() {
        if (GameObject.FindGameObjectWithTag("Scoreboard")) {
            scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();
        } else {
            print("COULD NOT FIND SCOREBOARD");
        }


        hatRoom = GameObject.FindGameObjectWithTag("HatRoom").GetComponent<Transform>();

        if (GameObject.FindGameObjectWithTag("Pregame"))
        {
            practiceRoom = GameObject.FindGameObjectWithTag("Pregame");
            practiceRoom.SetActive(true);
        }
        if (GameObject.FindGameObjectWithTag("Arena"))
        {
            arena = GameObject.FindGameObjectWithTag("Arena");
            arena.SetActive(false);
        }


        //        ChooseHats();
    }

    // Update is called once per frame
    void Update() {
        //all this has to ce re-done according to whatever you want the round to be. Does the round start after the 1st player puts hat on? Does it start when certain ammount of people do that? 
        if (!hatsSelected)
        {
            //         foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("Player"))
            foreach(GameObject p in players)
            {
                
 //               if (p.GetComponent<PlayerStatus>().playerClass == PlayerClass.none)
 //                   break;
 //               hatsSelected = true;

            }
        }
        else if (hatsSelected && !inBattlefield)
        {
            //            StartRound();
        }
        //else if (inBattlefield)
        //{
/*          
        timeElapsed += Time.deltaTime;

        if (scoreboard == null)
        {
            scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();
        }

        Debug.Log("red = " + scoreboard.red_score + ", blue = " + scoreboard.blue_score + ", maxScore = " + scoreboard.maxScore);

        if ((isScoreBased && (scoreboard.red_score >= scoreboard.maxScore || scoreboard.blue_score >= scoreboard.maxScore)) || (isTimeBased && timeElapsed >= roundTime))
        {
            print(scoreboard.red_score + " | " + scoreboard.blue_score + " | " + maxScore);
            print("ROUND HAS ENDED");
            EndRound();
        }
            //}
*/
    }

    [PunRPC]
    public void Display_Countdown()
    {
        Debug.Log("RoundManager.cs : Display_Countdown() : Inside");
        countdown_display.SetActive(true);
    }

    [PunRPC]
    public void Display_Restart(bool blueWon, int blue_score, int red_score)
    {
        Debug.Log("RoundManager.cs : Display_Restart() : blueWon = " + blueWon + ", red_score = " + red_score + ", blue_score = " + blue_score);

        restart_display.SetActive(true);
        Restart_Display restart = restart_display.GetComponent<Restart_Display>();
        restart.SetWinner(blueWon);
        restart.SetScore(red_score, blue_score);
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
  
    [PunRPC]
    public void EndRound()
    {
        practiceRoom.SetActive(true);
        arena.SetActive(false);

        Camera.main.transform.parent.position = GameObject.FindGameObjectWithTag("HatRoom").transform.position;

        scoreboard.ResetScoreboard();
        //Camera.main.transform.parent.GetComponent<PlatformController>().enabled = false;
        print("ROUND ENDED, SHOULD HAVE TURNED OFF PLATFORMCONTROLLER");
        //        FindPlayers();
        foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("Player"))
        {//TODO
            playerRCP.GetComponentInChildren<PlayerStatus>().RestartRound();
            playerRCP.GetComponentInChildren<PlayerStatus>().dead = true;
        }
        ChooseHats();
        //ShowFinalScoreboard();
 //       inBattlefield = false;
        
		
		timeElapsed = 0;
        
        print ("END OF ENDROUND");
    }

    [PunRPC]
    void StartRound()
    {
        score = 0;
        timeElapsed = 0;
 //       FindPlayers();
        //TODO send players to the battlefield
        /*foreach (GameObject pl in playerPCP)
            pl.GetComponent<TeamManager>().Respawn();*/
        inBattlefield = true;
        if (practiceRoom != null)
        {
            practiceRoom.SetActive(false);
            arena.SetActive(true);
        }
        print("starting round");
        Display_Countdown();
        foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("Player"))
        {//TODO
            playerRCP.GetComponentInChildren<PlayerStatus>().dead = false;
        }

    }

    void ChooseHats()
    {
//        Camera.main.transform.parent.GetComponent<PlatformController>().enabled = false;
        //        FindPlayers();
        foreach (GameObject player in playerRigs)
        {
            SendPlayerToHatRoom(player);                                                                  // UNCOMMENT

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
        Debug.Log("SENDPLAYERTOHATROOM CALLED");
        if (hatRoom)
        {
            Vector3 newPos = hatRoom.position;
            Transform camObj = player.GetComponentInChildren<Camera>().transform;
            newPos.x -= camObj.localPosition.x;
            newPos.z -= camObj.localPosition.z;
            player.GetComponent<Transform>().SetPositionAndRotation(newPos, player.GetComponent<Transform>().rotation);
            
        }
        else
        {
            Vector3 newPos = hatRoom.position;
            Transform camObj = player.GetComponentInChildren<Camera>().transform;
            newPos.x -= camObj.localPosition.x;
            newPos.z -= camObj.localPosition.z;
            player.GetComponent<Transform>().SetPositionAndRotation(newPos, player.GetComponent<Transform>().rotation);

        }
        print("hat time");
        practiceRoom.SetActive(true);
        arena.SetActive(false);

    }
 //   [PunRPC]
 //   void UpdateScoreboard(bool blueScored)
 //   {
 //       //Why are we assigning this on runtime? It could be assigned through the NetworkManager.
 //       ScoreboardUpdater scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();

 //       Debug.Log(GameObject.FindGameObjectWithTag("Scoreboard").name);

 //       if (scoreboard == null)
 //       {
 //           Debug.Log("SCOREBOARD UPDATER IS NULL!");
 //       }

 ////       Debug.Log("INSIDE RPC: BLUE SCORED " + blueScored);

 //       if (blueScored)
 //       {
 //           scoreboard.IncrementBlueScore();
 //       }
 //       else
 //       {
 //           scoreboard.IncrementRedScore();
 //       }
 //   }


    void ShowFinalScoreboard()
    {
        //TODO show whatever AG like to show in the end of round 
    }
    public void Subscribe(GameObject avatar, GameObject rig)
    {
        Debug.Log(avatar.name + " subscribed");
        players.Add(avatar);
        playerRigs.Add(rig);
        //send 
//        SendPlayerToHatRoom(rig);
       
        
    }
    //public void AssignTeam(GameObject avatar)
    //{
    //    avatar.GetComponent<TeamManager>().SetAvatar(avatar.transform);
    //    setMembers();
    //    if (blueMemb >= redMemb)
    //    {
    //        avatar.GetComponent<TeamManager>().SetRed();

    //    }
    //    else
    //    {
    //        avatar.GetComponent<TeamManager>().SetBlue();
    //    }
    //}

    public void Unsubscribe(GameObject avatar, GameObject rig)
    {
        players.Remove(avatar);
        players.Remove(rig);
        Debug.Log(avatar.name + " unsubscribed");
        if (avatar.GetComponent<TeamManager>().blue)
            blueMemb--;
        else
            redMemb--;
    }
    //private void setMembers()
    //{
    //    blueMemb = 0;
    //    redMemb = 0;
    //    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    //    Debug.Log("Num players found: " + players.Length);
    //    foreach (GameObject p in players)
    //    {
    //        if(p.GetComponentInParent<TeamManager>().blue)
    //        {
    //            blueMemb++;   
    //        }
    //        else
    //        {
    //            redMemb++;
    //        }
    //    }
    //    Debug.Log("Red: " + redMemb + " Blue: " + blueMemb);
    //}
}
