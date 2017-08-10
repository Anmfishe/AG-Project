using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum PlayerClass {none, attack, heal, support, all}; 

public class HatLogic : MonoBehaviour {


	public Material attackerMat;
	public Material healerMat;
	public Material supportMat;

	public PlayerClass playerClass = PlayerClass.none;
	private GameObject torso;
	public bool onHead = false;
	public bool touchingHead = false;
	public bool held = false;

	public bool resettable = false;
	public bool releaseHat = false;
	public float releaseTime = 0f;
	float timer;
	float resetTime = .5f;
	Vector3 startPosition;
	Quaternion startRotation;

	GameObject hatSpot;

	PickupParent wand;

	PhotonView photonView;

	private Renderer rend;

	// Use this for initialization
	public void Start()
	{
		timer = 0;

		startPosition = gameObject.transform.position;
		startRotation = gameObject.transform.rotation;
		// print("start position is: " + startPosition);
	}

    // Detect the collision of hat and head
    void OnCollisionEnter(Collision other)
    {
		if (other.gameObject.tag == "put")
		{
			if (held == true) 
			{
				//print ("touching head");
				bool touchingHead = true;
				hatSpot = other.gameObject;
				putOnHat ();
			}

		}
//	
//		{
//			touchingHead = false;
//		}

    }

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == "put") 
		{
			touchingHead = false;
			hatSpot = null;
		}
	}

	public void putOnHat()
	{
		//print ("putting on");
		this.transform.SetParent (hatSpot.transform);
		this.GetComponent<Rigidbody> ().isKinematic = true;
		torso = hatSpot.transform.parent.Find ("Torso").gameObject;
		torso.GetComponent<PlayerStatus> ().SetClass (playerClass);

		// Search for the child hat in player
		foreach (Transform child in hatSpot.transform)
			if (child.CompareTag ("findHat")) 
			{
				this.transform.position = child.transform.position;
				this.transform.rotation = child.transform.rotation;
				onHead = true;

                //Move player to battlefield.
//				torso.GetComponentInParent<TeamManager> ().Respawn ();                                                                          // uncomment for 8/8 build

                //GameObject.Find("RightController").GetComponent<VRTK.VRTK_StraightPointerRenderer>().enabled = false;
                //gameObject.transform.scale = child.transform.scale;
            }
	}

	public void takeOffHat()
	{
		onHead = false;
		//torso.GetComponent<PlayerStatus> ().setClass(PlayerClass.none);
	}
		
	public void callSetClass(PlayerClass pc)
	{
		photonView.RPC("setClass", PhotonTargets.AllBuffered, pc);
	}

	[PunRPC]
	public void setClass(PlayerClass pc)
	{
		playerClass = pc;

		if (playerClass == PlayerClass.heal) {
			if (rend != null)
				rend.material = healerMat;
		}
		else if (playerClass == PlayerClass.attack) {
			if (rend != null)
			rend.material = attackerMat;
		}
		else if (playerClass == PlayerClass.support) {
			if (rend != null)
			rend.material = supportMat;
		}

	}
		
	void Awake()
	{
		photonView = GetComponent<PhotonView> ();
		rend = GetComponent<Renderer> ();
	}
	

	public void resetHat()
	{
		this.GetComponent<Rigidbody>().isKinematic = false;
		//print(onHead +" " + wand.inHand);
		gameObject.transform.position = startPosition;
		gameObject.transform.rotation = startRotation;
		this.GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);

	}




	// Update is called once per frame
	void Update () {


		if (releaseHat == true) 
		{
			if ((Time.time - releaseTime) > .5f) 
			{
				held = false;
				releaseHat = false;
			}
		}

		if (GameObject.Find ("Controller (right)") == null)
			return;
		
		wand = GameObject.Find("Controller (right)").GetComponent<PickupParent>();
		resettable = !onHead && !wand.inHand;

		if (resettable && timer < 2)
		{
			timer += Time.deltaTime;
		}


		if (held == false && timer >= resetTime)
		{
			resetHat();
			resettable = false;
		}

		if (!resettable)
		{
			timer = 0;
		}
	}
}
