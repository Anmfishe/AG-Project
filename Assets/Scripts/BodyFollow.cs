﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyFollow : MonoBehaviour {
    public Transform head;
    public float speed;
    private float y_offset;
    private Transform myTrans;

    public Transform target_transform;

	// Use this for initialization
	void Start () {
        myTrans = GetComponent<Transform>();
    }
   
    void Update()
    {
        
        myTrans.position = Vector3.Lerp(myTrans.position, target_transform.position, speed * Time.deltaTime);
        myTrans.rotation = Quaternion.Lerp(myTrans.rotation, target_transform.rotation, speed * Time.deltaTime);
    }
}