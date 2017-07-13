using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {
    private GameObject[] redSquares;
    private GameObject[] blueSquares;
    public VRTK.VRTK_StraightPointerRenderer vrtk_spr;
    [HideInInspector]
    public bool blue = false;
	// Use this for initialization
	void Start () {
        redSquares = GameObject.FindGameObjectsWithTag("RedPlatform");
        blueSquares = GameObject.FindGameObjectsWithTag("BluePlatform");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetBlue()
    {
        blue = true;
        transform.position = blueSquares[Random.Range(0, blueSquares.Length - 1)].transform.position;
        vrtk_spr.blue = true;
    }
    public void SetRed()
    {
        blue = false;
        transform.position = redSquares[Random.Range(0, redSquares.Length - 1)].transform.position;
        vrtk_spr.blue = false;
    }
}
