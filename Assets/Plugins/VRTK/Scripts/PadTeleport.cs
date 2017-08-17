using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadTeleport : MonoBehaviour
{
    public VRTK.VRTK_BasicTeleport basicTeleport;

    public LayerMask blueLayersToIgnore;
    public LayerMask redLayersToIgnore;

    [HideInInspector]
    public bool blue;

    bool active;
    [HideInInspector]
    public Transform origin;
    Transform padHit;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 fwd = origin.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (active == true)
        {
            if (blue)
            {
                if (Physics.Raycast(origin.transform.position, fwd, out hit, 1000, blueLayersToIgnore))
                {
                    padHit = hit.transform;
                }
            }
            else
            {
                if (Physics.Raycast(origin.transform.position, fwd, out hit, 1000, redLayersToIgnore))
                {
                    padHit = hit.transform;
                }
            }
        }

        if (Input.GetKeyDown("joystick button 9"))
        {
            active = true;
        }
        if (Input.GetKeyUp("joystick button 9"))
        {
            active = false;
            if (padHit != null)
            {
                basicTeleport.ForceTeleport(padHit.position);
            }

        }

    }
}
