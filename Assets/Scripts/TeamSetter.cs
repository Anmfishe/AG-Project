using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetter : MonoBehaviour {
    public Material blue;
    public Material red;
    private PhotonView photonView;
	// Use this for initialization
	void Awake () {
        photonView = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetRed()
    {
        //Debug.Log("Test " + Time.time);
            photonView.RPC("SetRed2", PhotonTargets.AllBuffered, null);
        
    }
    public void SetBlue()
    {
        //Debug.Log("Test " + Time.time);
        photonView.RPC("SetBlue2", PhotonTargets.AllBuffered, null);
        
    }
    [PunRPC]
    public void SetRed2()
    {
        GetComponent<Renderer>().material = red;
    }
    [PunRPC]
    public void SetBlue2()
    {
        GetComponent<Renderer>().material = blue;
    }
}
