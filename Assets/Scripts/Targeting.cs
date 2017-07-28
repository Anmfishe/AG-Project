using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour {

    [HideInInspector]
    public Transform result;
    private TargetablePlayer targetableScript;

    public Transform pointer;
    public float range;
    public LayerMask layers;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Target();
    }
    private void Target()
    {
        RaycastHit hit;

        //Disable back faces so it doesn't collide with itself.
        Physics.queriesHitBackfaces = false;

//Debug.DrawRay(pointer.position, pointer.forward * range, Color.red, 0.01f);
        //Get raycast results.
        if (Physics.Raycast(pointer.position, pointer.forward, out hit, range, layers))
        {
            //print(hit.collider);
            //Return if target is the same, and turn off the previous indicator if it's not.
            if (result != null)
            {
                if (result == hit.collider.transform)
                {
                    return;
                }
                else
                {
                    //Reset targetable script.
                    if (targetableScript != null) targetableScript.SetIndicator(false);
                    targetableScript = null;

                    //Reset result.
                    result = null;
                }
            }

            //Check if it has a Player tag.
            switch(hit.collider.tag)
            {
                case "Player":
                    //Assign resulting collider to target.
                    result = hit.collider.transform;

                    //Try to get the targetable script. Turn it on if it's valid.
                    targetableScript = result.GetComponent<TargetablePlayer>();
                    if (targetableScript != null) targetableScript.SetIndicator(true);
                    break;
                case "BluePlatform":
                case "RedPlatform":
                    //Assign resulting collider to target.
                    result = hit.collider.transform;
                    break;

            }
        }
    }
}
