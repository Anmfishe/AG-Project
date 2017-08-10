﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterPlatform : MonoBehaviour {
    [HideInInspector]
    public int numPlayersOnPlatform = 0;
    [HideInInspector]
    public List<GameObject> players = new List<GameObject>();

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log("TeleporterPlatform.cs : OnTriggerEnter() : Collided with " + other.name + " with tag " + other.tag);
        if (other.tag == "Player")
        {
            numPlayersOnPlatform++;
//            Debug.Log("TeleporterPlatform.cs : OnTriggerEnter() : numPlayersOnPlatform : " + numPlayersOnPlatform);
            players.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
//        Debug.Log("TeleporterPlatform.cs : OnTriggerExit() : Collided with " + other.tag + " with tag " + other.tag);
        if (other.tag == "Player")
        {
            numPlayersOnPlatform--;
//            Debug.Log("TeleporterPlatform.cs : OnTriggerEnter() : numPlayersOnPlatform : " + numPlayersOnPlatform);
            players.Remove(other.gameObject);
        }
    }
}
