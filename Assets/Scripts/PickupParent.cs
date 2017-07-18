using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;


public class PickupParent : MonoBehaviour
{

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;
    GameObject[] Grabbables;
    GameObject grabbables;
    GameObject grabbed;
    Holdable holdable;
    GameObject head;
    GameObject ears;
    GameObject origin;
    GameObject target;
    public GameObject button;

    float pickupTime;

    public Animator buttonAnim;
    Animator whiteout;
    List<Collider> TriggerList;


    float audio2Volume;
    float audio1Volume;

    bool fadeIn;
    bool fadeOut;

    AudioSource ambient;
    AudioSource foley;

    AudioClip[] sounds;
    

    public Transform ball;
    private bool endingPlayed;

    void Awake()
    {
    }


    void Update()
    {
        if (Input.GetKeyDown("joystick button 17") && ((Time.time - pickupTime)> .2f))
        {
            if (grabbed != null)
            {
                grabbed.GetComponent<Rigidbody>().isKinematic = false;
                grabbed.gameObject.transform.SetParent(null);
                holdable.held = false;
                holdable = null;
                grabbed = null;
            }

        }
    }
        void OnTriggerStay(Collider col)
    {

            if (Input.GetKeyDown("joystick button 17"))
            {
            if (col.tag == "Grabbable" && grabbed == null)
            {
                if (col.GetComponent<Holdable>())
                {
                    holdable = col.GetComponent<Holdable>();
                    print("hold");

                    if (holdable.held == false)
                    {
                        col.GetComponent<Rigidbody>().isKinematic = true;
                        col.gameObject.transform.SetParent(gameObject.transform);
                        grabbed = col.gameObject;
                        holdable = col.GetComponent<Holdable>();
                        holdable.held = true;
                        pickupTime = Time.time;
                    }
                }
            }
        }
    }

     void tossObject(Rigidbody rigidBody)
    {
        // If condition is true, first expression evaluated, result becomes return. If it evaluates as false, the second object is assigned.
        // Short form of if/else
        Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
        if (origin != null)
        {
            rigidBody.velocity = origin.TransformVector(device.velocity);
            rigidBody.angularVelocity = origin.TransformVector(device.angularVelocity);
        }

        else
        {
            rigidBody.velocity = device.velocity;
            rigidBody.angularVelocity = device.angularVelocity;
        }
    }


}