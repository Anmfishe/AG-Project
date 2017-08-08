using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformNeighbors : MonoBehaviour {
    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public bool hasPlayer = false;
    private PhotonView pv;
    // Use this for initialization
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start () {
		
	}
    
    // Update is called once per frame
    void Update () {
        
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hasPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            hasPlayer = false;
        }
    }
    public void HasPlayer(bool b)
    {
        pv.RPC("HasPlayer2", PhotonTargets.All, b);
    }
    [PunRPC]
    void HasPlayer2(bool b)
    {
        hasPlayer = b;
    }
}
