using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour {

    public TeleporterPlatform blue;
    public TeleporterPlatform red;

    public GameObject[] bluePlatforms;
    public GameObject[] redPlatforms;

    public GameObject countdown_prefab;
    private GameObject rm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient && IsReady())
//        if (IsReady())
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
                //                ps.Teleport(bluePlatforms[i++].transform.position);
                ps.GetComponent<PhotonView>().RPC("Teleport", PhotonTargets.AllBuffered, bluePlatforms[i].transform.position);
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
                //                ps.Teleport(bluePlatforms[i++].transform.position);
                ps.GetComponent<PhotonView>().RPC("Teleport", PhotonTargets.All, redPlatforms[i].transform.position);
                i++;
            }
            else
            {
                Debug.Log("TeleportManager.cs : Teleport() : [red] " + player.name + " does not have a PlayerStatus component!");
            }
        }

        rm = GameObject.Find("Round Manager(Clone)");
        //        rm.GetComponent<RoundManager>().Display_Countdown();
        rm.GetComponent<PhotonView>().RPC("Display_Countdown", PhotonTargets.All, null);

//        this.GetComponent<PhotonView>().RPC("Display_Countdown", PhotonTargets.All, null);
        
    }

/*
    [PunRPC]
    void Display_Countdown()
    {
        if (countdown_prefab == null)
        {
            countdown_prefab = PhotonNetwork.InstantiateSceneObject(countdown_prefab.name, new Vector3(0, 4, 0), Quaternion.identity, 0, null);
        }
        else
        {
            countdown_prefab.SetActive(true);
        }
    }
*/
}