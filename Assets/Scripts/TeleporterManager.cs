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
            return true;
        }

        return false;
    }

    void TeleportPlayersToArena()
    {
//        Debug.Log("TeleporterManager.cs : TeleportPlayersToArena() : Inside");
        PlayerStatus ps;
        int i = 0;

        Debug.Log("blue : total = " + blue.players.Count);
        foreach (GameObject player in blue.players)
        {
            ps = player.GetComponent<PlayerStatus>();
            if (ps != null)
            {
                ps.GetComponent<PhotonView>().RPC("Teleport", PhotonTargets.AllBuffered, true, bluePlatforms[i].transform.position);
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
                ps.GetComponent<PhotonView>().RPC("Teleport", PhotonTargets.All, false, redPlatforms[i].transform.position);
                i++;
            }
            else
            {
                Debug.Log("TeleportManager.cs : Teleport() : [red] " + player.name + " does not have a PlayerStatus component!");
            }
        }

        rm = GameObject.Find("Round Manager(Clone)");
        rm.GetComponent<PhotonView>().RPC("Display_Countdown", PhotonTargets.All, null);
    }

}