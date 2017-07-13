using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour {

    Transform targeted;
    [HideInInspector]
    public Transform target;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown("joystick button 15"))
        //{
        //    target = ;
        //}

        if (targeted)
        {
            targeted.Find("Head/TargetIndicator").gameObject.SetActive(false);
        }

        RaycastHit hit;
        target = null;
        Debug.DrawRay(this.transform.position, this.transform.forward * 100, Color.red, 0.01f);
        Physics.queriesHitBackfaces = false;
        if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, 100))
        {
           // Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.tag == "Player"/*&& hit.collider.gameObject != transform.parent.gameObject*/)
            {

                Debug.Log("testing");
                //GameObject[] targets;
                //targets = GameObject.FindGameObjectsWithTag("Target");
                //foreach (GameObject t in targets)
                //{
                //    t.SetActive(false);
                //}
                hit.transform.parent.Find("Head/TargetIndicator").gameObject.SetActive(true);
                
                targeted = hit.transform.parent;
                target = hit.transform.parent.Find("Avatar_torso").transform;
            }
        }

      
    }

    private void LateUpdate()
    {
        
    }

}
