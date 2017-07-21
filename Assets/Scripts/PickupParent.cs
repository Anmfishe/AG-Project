using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;


public class PickupParent : MonoBehaviour
{
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

	public bool inHand = false;

	void Awake()
	{
	}


	void Update()
	{
		// Drop object
		if (Input.GetKeyDown("joystick button 15") && ((Time.time - pickupTime)> .2f))
		{
			if (grabbed != null) 
			{
				tossObject (grabbed.GetComponent<Rigidbody>());
				inHand = false;
			}
		}
	}


	// Pickup logic
	void OnTriggerStay(Collider col)
	{

		if (Input.GetKeyDown("joystick button 15"))
		{
			if (col.tag == "Grabbable" && grabbed == null)
			{
				if (col.GetComponent<HatLogic>())
				{
					if (col.GetComponent<HatLogic>().onHead == false)
					{
						if (col.GetComponent<Holdable>())
						{
							holdable = col.GetComponent<Holdable>();
							inHand = true;

							if (holdable.held == false)
							{
								col.GetComponent<Rigidbody>().isKinematic = true;
								col.gameObject.transform.SetParent(gameObject.transform);
								grabbed = col.gameObject;
								holdable = col.GetComponent<Holdable>();
								holdable.held = true;
								pickupTime = Time.time;						

								col.GetComponent<HatLogic> ().takeOffHat();
							}
						}
					}
				}
			}
		}
	}

	void tossObject(Rigidbody rigidBody)
	{
		grabbed.GetComponent<Rigidbody>().isKinematic = false;
		grabbed.gameObject.transform.SetParent(null);
		holdable.held = false;
		holdable = null;
		grabbed = null;
	}


}