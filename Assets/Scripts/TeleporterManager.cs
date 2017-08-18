using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour {

    public TeleporterPlatform blue;
    public TeleporterPlatform red;

    public GameObject[] bluePlatforms;
    public GameObject[] redPlatforms;
    
    GameObject rm;
    NotificationManager nm;

	// Use this for initialization
	void Start () {
        if (Camera.main == null)
        {
            Debug.Log("TeleporterManager.cs : Start() : Could not find \"Camera.main\" GameObject");
            return;
        }
        if (Camera.main.GetComponent<NotificationManager>() == null)
        {
            Debug.Log("TeleporterManager.cs : Start() : Could not find \"NotificationManager\" component");
            return;
        }

        nm = Camera.main.GetComponent<NotificationManager>();
    }
	
	// Update is called once per frame
	void Update () {
		if (IsReady())
        {
            TeleportPlayersToArena();
        }
	}

    bool IsReady()
    {
        if (nm == null)
        {
            if (Camera.main == null)
            {
                Debug.Log("TeleporterManager.cs : Start() : Could not find \"Camera.main\" GameObject");
            }
            else if (Camera.main.GetComponent<NotificationManager>() == null)
            {
                Debug.Log("TeleporterManager.cs : Start() : Could not find \"NotificationManager\" component");
            }
            else
            {
                nm = Camera.main.GetComponent<NotificationManager>();
            }
        }

        //        Debug.Log("blue : " + blue.numPlayersOnPlatform + " red : " + red.numPlayersOnPlatform + " == " + PhotonNetwork.playerList.Length);
        if (PhotonNetwork.playerList.Length > 0 && (blue.numPlayersOnPlatform + red.numPlayersOnPlatform) == PhotonNetwork.playerList.Length)          // have to divide by 2 because torso has 2 colliders which trigger twice per player
        {
            bool ready = true;

            Debug.Log("TeleporterManager.cs : IsReady() : All players are on platforms");
            foreach (GameObject player in blue.players)
            {
                if (player.GetComponent<PlayerStatus>().playerClass == PlayerClass.none)
                {                    
                    Debug.Log("TeleporterManager.cs : IsReady() : Player does not have a hat");
                    ready = false;
                }
            }
            foreach (GameObject player in red.players)
            {
                if (player.GetComponent<PlayerStatus>().playerClass == PlayerClass.none)
                {
                    Debug.Log("TeleporterManager.cs : IsReady() : Player does not have a hat");
                    ready = false;
                }
            }

            if (ready)
            {
                return true;
            }
        }

        // Notify player how many people are not ready if the player is on the teleporter
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (go.GetComponent<PhotonView>().isMine && go.GetComponent<PlayerStatus>().onTeleporter && go.GetComponent<PlayerStatus>().playerClass != PlayerClass.none)
            {
                int numPlayersNotReady = PhotonNetwork.playerList.Length - blue.numPlayersOnPlatform - red.numPlayersOnPlatform;
                nm.SetNotification("Waiting on " + numPlayersNotReady + " player(s)..");
                break;
            }
        }
        return false;
    }

    void TeleportPlayersToArena()
    {
        nm.Clear();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            go.GetComponent<PlayerStatus>().onTeleporter = false;
        }

        if (! PhotonNetwork.isMasterClient)
        {
            return;
        }

//        Debug.Log("TeleporterManager.cs : TeleportPlayersToArena() : Inside");
        PlayerStatus ps;
        int i = 0;
        rm = GameObject.Find("Round Manager(Clone)");
        rm.GetComponent<PhotonView>().RPC("StartRound", PhotonTargets.All, null);
        Debug.Log("blue : total = " + blue.players.Count);
        foreach (GameObject player in blue.players)
        {
            ps = player.GetComponent<PlayerStatus>();
            if (ps != null)
            {
                ps.GetComponent<PhotonView>().RPC("Teleport", PhotonTargets.AllBuffered, true, Vector3.zero);
                i++;
            }
            else
            {
                Debug.Log("TeleportManager.cs : Teleport() : [blue] " + player.name + " does not have a PlayerStatus component!");
            }
        }

        i = 0;
        Debug.Log("blue : total = " + red.players.Count);
        foreach (GameObject player in red.players)
        {
            ps = player.GetComponent<PlayerStatus>();
            if (ps != null)
            {
                ps.GetComponent<PhotonView>().RPC("Teleport", PhotonTargets.All, false, Vector3.zero);
                i++;
            }
            else
            {
                Debug.Log("TeleportManager.cs : Teleport() : [red] " + player.name + " does not have a PlayerStatus component!");
            }
        }
        
    }

}