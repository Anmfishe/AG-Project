using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureRigEnabled : MonoBehaviour {
    private GameObject camRig;
	// Use this for initialization
	void Start () {
        camRig = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnEnable()
    {
       // camRig = transform.parent.gameObject;
        //camRig.GetComponent<Edwon.VR.VRGestureRig>().Init();
    }
}
