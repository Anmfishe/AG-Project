using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour {

    public TeleporterPlatform blue;
    public TeleporterPlatform red;

    public GameObject[] bluePlatforms;
    public GameObject[] redPlatforms;
    
    private GameObject rm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient && IsReady())
        {
            TeleportPlayersToArena();
        }
	}

    bool IsReady()
    {
//        Debug.Log("blue : " + blue.numPlayersOnPlatform + " red : " + red.numPlayersOnPlatform + " == " + PhotonNetwork.playerList.Length);
        if (PhotonNetwork.playerList.Length != 0 && (blue.numPlayersOnPlatform + red.numPlayersOnPlatform) / 2 == PhotonNetwork.playerList.Length)          // have to divide by 2 because torso has 2 colliders which trigger twice per player
        {
            Debug.Log("TeleporterManager.cs : IsReady() : All players are on platforms");
            foreach (GameObject player in blue.players)
            {
                if (player.GetComponent<PlayerStatus>().playerClass == PlayerClass.none)
                {
                    Debug.Log("TeleporterManager.cs : IsReady() : Player does not have a hat");
                    return false;
                }
            }
            foreach (GameObject player in red.players)
            {
                if (player.GetComponent<PlayerStatus>().playerClass == PlayerClass.none)
                {
                    Debug.Log("TeleporterManager.cs : IsReady() : Player does not have a hat");
                    return false;
                }
            }
            return true;
        }

        return false;
    }

    void TeleportPlayersToArena()
    {
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