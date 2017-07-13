using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public float shieldDuration = 10f;
    private float shieldTimer;
	
    // Use this for initialization
	void Start () {
        shieldTimer = shieldDuration;	
	}
	
	// Update is called once per frame
	void Update () {
        shieldTimer -= Time.deltaTime;
        if (shieldTimer <= 0)
            Destroy(gameObject);

	}
}
