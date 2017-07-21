using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

    
    public Transform hatRoom;
    public float roundTime;
    private float timeElapsed;
    private bool inBattlefield;
    private bool hatsSelected = false;
    private GameObject[] playerRigs;
    private GameObject[] playerPCP;
    //TODO score ssystem, if you want it to end the round
    // Use this for initialization
    void Start () {
        hatRoom = GameObject.FindGameObjectWithTag("HatRoom").GetComponent<Transform>();
        FindPlayers();
    }
	
	// Update is called once per frame
	void Update () {
        //        if (Input.anyKeyDown)
        //           EndRound();
        if (inBattlefield)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= roundTime)
                EndRound();
        }
        if (!hatsSelected)
        {
            foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("PCP"))
            {
                /*
                if playerRCP.playerClass == none
                    break;
                hatsSelected = true;   
             */
            }
        }
        else if(hatsSelected && !inBattlefield)
        {
            StartRound();
        }
	}

    /// <summary>
    /// We are using this shit every time before any allPlayers action cause we didn't go with observer
    /// </summary>
    void FindPlayers()
    {
        playerRigs = GameObject.FindGameObjectsWithTag("CameraRig");
        playerPCP = GameObject.FindGameObjectsWithTag("PCP");
    }

    void EndRound()
    {
        ChooseHats();
        ShowScoreboard();
        inBattlefield = false;
    }

    void StartRound()
    {

        timeElapsed = 0;
        FindPlayers();
        //TODO send players to the battlefield
        foreach (GameObject pl in playerPCP)
            pl.GetComponent<TeamManager>().Respawn();
        inBattlefield = true;
    }

    void ChooseHats()
    {

        FindPlayers();
        foreach (GameObject player in playerRigs)
        {
            player.GetComponent<Transform>().SetPositionAndRotation(hatRoom.position, hatRoom.rotation);
         
        }
        foreach (GameObject playerRCP in GameObject.FindGameObjectsWithTag("PCP"))
            //player.TakeOffHat();

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
