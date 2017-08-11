using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *      !!!!    IMPORTANT   !!!!
 * 
 *      This script depends on the player torso to have a rigidbody and tag "Player".
 *      Also depends on the scene having a GameObject called "PowerupManager(Clone)" with script component "PowerupManager"
 * */

public class Powerup : MonoBehaviour {

    public bool isBlue;
    public int platformIndex;
    public GameObject powerup_success;
    GameObject pm;

	// Use this for initialization
	void Start () {
        pm = GameObject.Find("PowerupManager(Clone)");
        if (pm == null)
        {
            Debug.Log("Powerup.cs : Start() : Not able to find GameObject called \"PowerupManager(Clone)\" in scene!");
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (PhotonNetwork.isMasterClient)
        {
            this.transform.Rotate(this.transform.up, 30 * Time.deltaTime);
        }
	}

    public void SetPowerupProperties(bool isBlue_, int platformIndex_)
    {
        isBlue = isBlue_;
        platformIndex = platformIndex_;

        Debug.Log("Powerup.cs : SetPowerupProperties : isBlue=" + isBlue_ + " platformIndex=" + platformIndex_);
    }

    void OnTriggerEnter(Collider other)
    {
        if (GetComponent<PhotonView>().isMine)
        {
            if (other.tag == "Player")
            {
                this.GetComponent<Collider>().enabled = false;
                other.GetComponent<PhotonView>().RPC("SetRandomSpell", other.GetComponent<PhotonView>().owner, null);
                PowerupManager pm = GameObject.Find("PowerupManager(Clone)").GetComponent<PowerupManager>();
                pm.DecrementPowerUp(isBlue, platformIndex);
                PhotonNetwork.Instantiate(powerup_success.name, transform.position, Quaternion.identity, 0);
                PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
            }
        }
    }
}
