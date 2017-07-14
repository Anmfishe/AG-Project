using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    public float delay = 5;
	// Use this for initialization
	void Start () {
        Destroy(this, delay);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
