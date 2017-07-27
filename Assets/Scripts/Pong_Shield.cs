using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *                                  !!!!!!!!!!!!    IMPORTANT   !!!!!!!!!!!!
 *                                  
 *      This script relies on the scene having a GameObject called "RightController". MAKE SURE THAT THE GAMEOBJECT EXISTS!  
 *          Also, this script relies on the assumption that the pong shield ONLY MOVES ALONG THE X-AXIS and CENTERED IN (0, 0, 0)!
 *              In addition, it requires a GameObject (in our use, we used 2 opposite facing planes) with specified layer pongLayer
 * 
 * */

public class Pong_Shield : MonoBehaviour {

    public Vector3 scale;
    public float clamp;
    public float duration;
    public string pongLayer;

    GameObject rightController;
    RaycastHit hit;

    // Use this for initialization
    void Start () {
        // check scale input
        if (scale == null)
        {
            scale = new Vector3(2, 1.5f, 0.25f);
        }
        else
        {
            if (scale.x == 0)
            {
                scale.x = 2;
            }
            if (scale.y == 0)
            {
                scale.y = 1.5f;
            }
            if (scale.z == 0)
            {
                scale.z = 0.25f;
            }
        }
        this.transform.localScale = scale;

        // check clamp input
        if (clamp <= 0)
        {
            clamp = 4;
        }

        // check duration input
        if (duration <= 0)
        {
            duration = 3;
        }

        // check pongLayer input
        if (LayerMask.GetMask(pongLayer) < 0)
        {
            Debug.Log("Pong_Shield.cs : INVALID pongLayer INPUT, DEFAULTING TO \"PongShieldPlane\"");
            pongLayer = "PongShieldPlane";
        }

        // retrieve "Right Controller" GameObject
        rightController = GameObject.Find("RightController");
    }
	
	// Update is called once per frame
	void LateUpdate () {
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }


        // in case the GameObject "Right Controller" was not found in Start(), look for it again
        //      if still not found, return and skip the RayCast
        if (rightController == null)
        {
            rightController = GameObject.Find("RightController");
            if (rightController == null)
            {
                return;
            }
        }        

		if (Physics.Raycast(rightController.transform.position, rightController.transform.forward, out hit, 100, LayerMask.GetMask(pongLayer)))
        {
            if (hit.transform.tag == "Pong_Shield_Plane")
            {
               this.transform.position = new Vector3(Mathf.Max(-clamp, Mathf.Min(clamp, hit.point.x)), this.transform.position.y, hit.point.z);
            }
        }
	}
}
