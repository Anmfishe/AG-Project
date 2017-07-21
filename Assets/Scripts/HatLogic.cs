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


    // Use this for initialization
    void Start () {
	}

	void Awake()
	{
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
