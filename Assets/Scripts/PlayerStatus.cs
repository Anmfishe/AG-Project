using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IPunObservable
{
	public PlayerClass playerClass;

	private GameObject[] hats;
	private GameObject[] players;

	private BookLogic bookLogic;

	private Transform respawnPt;
    private Transform timeOutPt;
    public PhotonView photonView;
    public PlayerSoundManager psm;
    private GameObject cameraRig;
    private TextMesh deadText;

    private bool dead = false;
    private float deathTime = 0f;
    public float respawnLength = 2f;

	ScoreboardUpdater myScoreboard;

    PhotonView self_photonview;


    public int max_health = 100;
  //  [HideInInspector]
    public int current_health = 100;
    //   public int hp = 100;
    // Use this for initialization
    void Start()
    {
		bookLogic = transform.parent.GetComponentInChildren<BookLogic> ();
		hats = GameObject.FindGameObjectsWithTag("Grabbable");

		bookLogic = transform.parent.GetComponentInChildren<BookLogic> ();

        //Get camera rig if this object belogns to the client.
        if (photonView.isMine)
        {
            //Gets the Camera (eyes) and navigates to the Camera Rig object.
            cameraRig = Camera.main.transform.parent.gameObject;
            deadText = Camera.main.transform.GetChild(0).GetComponent<TextMesh>();
        }
        
        //Get's the location where the player will respawn.
        timeOutPt = GameObject.FindGameObjectWithTag("TimeOut").transform;
        respawnPt = GameObject.FindGameObjectWithTag("RespawnDefault").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Respawn Player when time out's done.
        if (dead == true)
        {
            if ((Time.time - deathTime) >  respawnLength)
            {

                Respawn();
            }

            else
            {
                if (photonView.isMine)
                {
					myScoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();

					if (myScoreboard.roundOver == false)
						deadText.text = "You were killed!\nRespawn in " + (respawnLength - (Time.time - deathTime));

					else
						deadText.text = "The round is over.";
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Die();
        }
    }

    public void heal(int healthAdded)
    {
        if (dead == false)
        {
            current_health += healthAdded;
        }

        if (current_health >= max_health)
        {
            current_health = max_health;
        }
    }

    //Reduces the health by the damage received.
    public void takeDamage(int damage)
    {
        print("TAKING DAMAGE!");
        // Ensure that this is the active player
        if (!photonView.isMine)
        {
            return;
        }

        if (dead == false)
        {
            current_health -= damage;
            print("took damage, health = " + current_health);
            psm.PlayerHurt();
        }

        if (current_health <= 0)
        {
            Die();
        }
    }

    // On death, we warp the camera rig of the corresponding player
    void Die()
    {
        //Move Player to the time out are if it belongs to the client.
        if (photonView.isMine)
        {
            cameraRig.transform.position = timeOutPt.position;

            deadText.gameObject.SetActive(true);

            // Increment scoreboard
            bool blueScored = ! this.transform.parent.GetComponent<TeamManager>().blue;
            Debug.Log("ABOUT TO RPC: BLUE SCORED " + blueScored);
            self_photonview = GetComponent<PhotonView>();
            if (blueScored)
            {
                //               photonView.RPC("UpdateScoreboard", PhotonTargets.All, blueScored);
                self_photonview.RPC("UpdateScoreboard", PhotonTargets.All, true);
            }
            else
            {
                //                photonView.RPC("UpdateScoreboard", PhotonTargets.All, ! blueScored);
                self_photonview.RPC("UpdateScoreboard", PhotonTargets.All, false);
            }
        }

        deathTime = Time.time;
        dead = true;
      //  Respawn();
    }

    [PunRPC]
    void UpdateScoreboard(bool blueScored)
    {
        //Why are we assigning this on runtime? It could be assigned through the NetworkManager.
        ScoreboardUpdater scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();

        Debug.Log(GameObject.FindGameObjectWithTag("Scoreboard").name);

        if (scoreboard == null)
        {
            Debug.Log("SCOREBOARD UPDATER IS NULL!");
        }

        Debug.Log("INSIDE RPC: BLUE SCORED " + blueScored);

        if (blueScored)
        {
            scoreboard.IncrementBlueScore();
        }
        else
        {
            scoreboard.IncrementRedScore();
        }
    }

//	[PunRPC]
	void ResetScoreboard()
	{
		//Why are we assigning this on runtime? It could be assigned through the NetworkManager.
		ScoreboardUpdater scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();

		//Debug.Log(GameObject.FindGameObjectWithTag("Scoreboard").name);

		if (scoreboard == null)
		{
			Debug.Log("SCOREBOARD UPDATER IS NULL!");
		}

			scoreboard.Reset();
	}

    //Reset health and move Player to respawn area.
    void Respawn()
    {
        dead = false;
        current_health = max_health;

        //Move Player to respawn area if it belongs to the client.
        if (photonView.isMine)
        {
			myScoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();
            // cameraRig.transform.position = respawnPt.position;
			if (myScoreboard.roundOver == false) {
				this.transform.parent.GetComponent<TeamManager> ().Respawn ();
			} else 
			{
                self_photonview.RPC("RestartRound", PhotonTargets.AllBuffered, null);
			}
			
			deadText.gameObject.SetActive (false);

        }
    }

    [PunRPC]
    public void RestartRound()
    {
        //        self_photonview.RPC("ResetScoreboard", PhotonTargets.All, null);
        ResetScoreboard();

        foreach (GameObject hat in hats)
        {
            hat.transform.SetParent(null);
            hat.GetComponent<HatLogic>().resetHat();
            hat.GetComponent<HatLogic>().onHead = false;
            hat.GetComponent<HatLogic>().resettable = true;
        }

        players = GameObject.FindGameObjectsWithTag("PCP");

        foreach (GameObject player in players)
        {
            player.GetComponentInChildren<PlayerStatus>().playerClass = PlayerClass.none;
        }
		print ("round restarted");
		cameraRig = Camera.main.transform.parent.gameObject;

        cameraRig.GetComponent<SpellcastingGestureRecognition>().enabled = false;
        cameraRig.GetComponent<PlatformController>().enabled = false;
        cameraRig.GetComponent<Edwon.VR.VRGestureRig>().enabled = false;

        bookLogic.UpdateUI();

		myScoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<ScoreboardUpdater>();
        myScoreboard.roundOver = false;
		print ("Scoreboard " +  myScoreboard.roundOver);

    }

	public void SetClass(PlayerClass pc)
	{
		playerClass = pc;

		if (playerClass == PlayerClass.none) {

			if (photonView.isMine) {
				cameraRig.GetComponent<SpellcastingGestureRecognition> ().enabled = false;
				cameraRig.GetComponent<Edwon.VR.VRGestureRig> ().enabled = false;
				bookLogic.UpdateUI ();
			}
		} else 
		{
			if(photonView.isMine)
			{
				cameraRig.GetComponent<SpellcastingGestureRecognition> ().enabled = true;
				cameraRig.GetComponent<Edwon.VR.VRGestureRig> ().enabled = true;
				cameraRig.GetComponent<PlatformController> ().enabled = true;
				bookLogic.UpdateUI ();
			}
		}
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // If you own the game object
        if (stream.isWriting)
        {
            // Sync all instances of health according to my health
            stream.SendNext(current_health);
        }
        // If you dont own the game object
        else
        {
            // Sync the avatar's health according to the owner of the avatar.
            current_health = (int)stream.ReceiveNext();
        }
    }
}