/*
Name: Projectile.cs
Author: Dylan Faust
Purpose: Fire projectiles on player input=
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{

Camera mainCam;
float triggerL;
float triggerR;

bool triggerUsed = false;

public GameObject fireBall;
	// Use this for initialization
	void Start () 
	{
		mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{

		triggerL = Input.GetAxis("TriggerL");
		triggerR = Input.GetAxis("TriggerR");
		if ((triggerL > .3f)  || (triggerR > .3f))//Input.GetKeyDown("joystick button 14"))// && myTime > nextFire)
        {
        	//print("swag");
        	//GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        	//sphere.transform.position = mainCam.transform.position;// + new Vector3(2,2,2);
        	if (triggerUsed == false)
        	{
        		triggerUsed = true;
        		Instantiate(fireBall,  mainCam.transform.position, mainCam.transform.rotation);
        	}

        }
        else
        {
        	triggerUsed = false;
        }
	}
}
