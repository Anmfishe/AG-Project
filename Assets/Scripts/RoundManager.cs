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
    private int blueMemb;
    private int redMemb;


    //
    public GameObject countdown_display;
    public float countdown_timer_max = 5.0f;
    public float y_acceleration = 9.78f;
    private float y_vel = 0;
    private float countdown_timer = 0;
    private TextMesh countdown_red_text;
    private TextMesh countdown_blue_text;
    private bool countdown_flag = true;
    


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

        if (countdown_flag)
        {
            SetCountdown();
        }

            //}
    }

    private void SetCountdown()
    {
        // Retrieve countdown text meshes if not already found. Return null if could not be found
        if (countdown_red_text == null)
        {
            if (countdown_display.transform.Find("red_countdown") != null)
            {
                countdown_red_text = countdown_display.transform.Find("red_countdown").GetComponent<TextMesh>();
            }
            else
            {
                Debug.Log("RoundManager.cs : Countdown() : Could not find GameObject \"red_countdown\"");
            }
        }
        if (countdown_blue_text == null)
        {
            if (countdown_display.transform.Find("blue_countdown") != null)
            {
                countdown_blue_text = countdown_display.transform.Find("blue_countdown").GetComponent<TextMesh>();
            }
            else
            {
                Debug.Log("RoundManager.cs : Countdown() : Could not find GameObject \"blue_countdown\"");
            }
        }
        if (countdown_red_text == null || countdown_blue_text == null)
        {
            return;
        }

        // Set countdown
        countdown_timer += Time.deltaTime;
        if (countdown_timer > 1.25*countdown_timer_max)
        {
            countdown_flag = false;
            countdown_timer = 0;
        }
        else if (countdown_timer > countdown_timer_max)
        {
            y_vel += y_acceleration * Time.deltaTime;
            countdown_display.transform.position += new Vector3(0, y_vel, 0);
        }
        else
        {
            int temp = Mathf.CeilToInt(countdown_timer_max - countdown_timer);
            float scale = 1.5f * (Mathf.Ceil(countdown_timer) - countdown_timer);
            Debug.Log(scale);
            countdown_red_text.text = "" + temp;
            countdown_red_text.characterSize = scale;
            countdown_blue_text.text = "" + temp;
            countdown_blue_text.characterSize = scale;
        }
        countdown_display.SetActive(countdown_flag);
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
        scoreboard.ResetScoreboard();
        //Camera.main.transform.parent.GetComponent<PlatformController>().enabled = false;
        print("ROUND ENDED, SHOULD HAVE TURNED OFF PLATFORMCONTROLLER");
//        FindPlayers ();
		//foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("Player"))                                //TODO
		//	playerRCP.GetComponentInChildren<PlayerStatus> ().RestartRound();
        ChooseHats();
        //ShowFinalScoreboard();
 //       inBattlefield = false;
		
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
//        Camera.main.transform.parent.GetComponent<PlatformController>().enabled = false;
        //        FindPlayers();
        foreach (GameObject player in playerRigs)
        {
            SendPlayerToHatRoom(player);

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
            hatRoom = GameObject.FindGameObjectWithTag("HatRoom").transform;
            Vector3 newPos = hatRoom.position;
            Transform camObj = player.GetComponentInChildren<Camera>().transform;
            newPos.x -= camObj.localPosition.x;
            newPos.z -= camObj.localPosition.z;
            player.GetComponent<Transform>().SetPositionAndRotation(newPos, player.GetComponent<Transform>().rotation);

        }
        
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
        SendPlayerToHatRoom(rig);
       
        
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
