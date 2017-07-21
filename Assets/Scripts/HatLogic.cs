using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass {none, attack, heal, support}; 

public class HatLogic : MonoBehaviour {


	public Material attackerMat;
	public Material healerMat;
	public Material supportMat;

	private Renderer rend;

	public PlayerClass playerClass = PlayerClass.none;
	private GameObject torso;
	public bool onHead = false;

	public bool resettable = false;
	float timer;
	Vector3 startPosition;
	Quaternion startRotation;

	PickupParent wand;

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
          gameObject.transform.SetParent(other.gameObject.transform);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
			torso = other.transform.parent.Find ("Torso").gameObject;
			torso.GetComponent<PlayerStatus>().playerClass = playerClass;


            // Search for the child hat in player
            foreach (Transform child in other.transform) if (child.CompareTag("findHat"))
            {
                    gameObject.transform.position = child.transform.position;
                    gameObject.transform.rotation = child.transform.rotation;
					onHead = true;
                    //gameObject.transform.scale = child.transform.scale;
                }

        }
    }

	public void takeOffHat()
	{
		onHead = false;
		torso.GetComponent<PlayerStatus> ().playerClass = PlayerClass.none;
	}

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
		rend = GetComponent<Renderer> ();
	}
	

	public void resetHat()
	{
		//print(onHead +" " + wand.inHand);
		print("reset");
		gameObject.transform.position = startPosition;
		gameObject.transform.rotation = startRotation;

		//print("recent position is: " + gameObject.transform.position);
		//gameObject.transform.rotation = hatStart.transform.rotation;

	}




	// Update is called once per frame
	void Update () {
		wand = GameObject.Find("Controller (right)").GetComponent<PickupParent>();
		resettable = !onHead && !wand.inHand;
		print("timer is " + timer + " " + resettable);

		if (resettable && timer < 2)
		{
			timer += Time.deltaTime;
		}


		if (resettable && timer >= .5f)
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
