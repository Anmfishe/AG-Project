using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformNeighbors : MonoBehaviour {
    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public bool hasPlayer = false;
    //public BoxCollider bc;
    private PhotonView pv;
    [HideInInspector]
    public LayerMask layerSave;
    // Use this for initialization
    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        layerSave = gameObject.layer;

    }
    void Start () {
		
	}
    
    // Update is called once per frame
    void Update () {
        
	}
    private void FixedUpdate()
    {
        //pv.RPC("HasPlayer2", PhotonTargets.All, hasPlayer, gameObject.layer);
        if(pv.isMine && gameObject.layer != LayerMask.NameToLayer("Default"))
            layerSave = gameObject.layer;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            hasPlayer = true;
            gameObject.layer = LayerMask.NameToLayer("Default");
            pv.RPC("HasPlayer2", PhotonTargets.All, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //hasPlayer = false;
            //gameObject.layer = layerSave;
            pv.RPC("HasPlayer2", PhotonTargets.All, false);
        }
    }
    public void SetLayer(LayerMask l)
    {

    }
    public void HasPlayer(bool b)
    {
        pv.RPC("HasPlayer2", PhotonTargets.All, b);
    }
    [PunRPC]
    void HasPlayer2(bool b)
    {
        hasPlayer = b;
        if(b)
            gameObject.layer = LayerMask.NameToLayer("Default");
        else
            gameObject.layer = layerSave;
    }
}
