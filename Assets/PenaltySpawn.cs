using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenaltySpawn : MonoBehaviour {

	bool vacant = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsVacant()
	{
		return vacant;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			vacant = false;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			vacant = true;
		}
	}
}
