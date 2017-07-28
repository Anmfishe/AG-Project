﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *      !!!!!!!!!   IMPORTANT   !!!!!!!!!
 *      
 *      Player avatar torso must be tagged wtih "Player"
 *      There must be a GameObject called "PowerupManager" in the scene, and it must have a script component called "PowerupManager(Clone)"
 * 
 * */

public class Powerup : MonoBehaviour {

    PowerupManager pm;
    bool isBlue;
    int platformIndex;

	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("PowerupManager(Clone)");
        if (go == null)
        {
            Debug.Log("Powerup.cs : Start() : Not able to find GameObject called \"PowerupManager(Clone)\" in scene!");
            return;
        }
        
        pm = go.GetComponent<PowerupManager>();
        if (pm == null)
        {
            Debug.Log("Powerup.cs : Start() : Not able to find script component called \"PowerupManager(Clone)\" in scene!");
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(this.transform.up, 30 * Time.deltaTime);
	}

    public void SetPowerupProperties(bool isBlue_, int platformIndex_)
    {
        isBlue = isBlue_;
        platformIndex = platformIndex_;
    }

    void OnTriggerEnter(Collider other)
    {
       
    }

    [PunRPC]
    void UpdatePowerupManager()
    {
        pm.DecrementPowerUp(isBlue, platformIndex);
    }
}
