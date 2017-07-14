using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IPunObservable
{
    private Transform respawnPt;
    private Transform timeOutPt;
    public PhotonView photonView;
    public PlayerSoundManager psm;
    private GameObject cameraRig;

    private bool dead = false;
    private float deathTime = 0f;
    public float respawnLength = 2f;

    PhotonView test_photonview;


    public int max_health = 100;
  //  [HideInInspector]
    public int current_health = 100;
    //   public int hp = 100;
    // Use this for initialization
    void Start()
    {

        //Get camera rig if this object belogns to the client.
        if (photonView.isMine)
        {
            //Gets the Camera (eyes) and navigates to the Camera Rig object.
            cameraRig = Camera.main.transform.parent.gameObject;
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
        // Ensure that this is the active player
        if (!photonView.isMine)
        {
            return;
        }

        if (dead == false)
        {
            current_health -= damage;
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


            bool blueScored = ! cameraRig.GetComponent<TeamManager>().blue;
            Debug.Log("ABOUT TO RPC: BLUE SCORED " + blueScored);
            test_photonview = GetComponent<PhotonView>();
            if (blueScored)
            {
                //               photonView.RPC("UpdateScoreboard", PhotonTargets.All, blueScored);
                test_photonview.RPC("UpdateScoreboard", PhotonTargets.All, true);
            }
            else
            {
                //                photonView.RPC("UpdateScoreboard", PhotonTargets.All, ! blueScored);
                test_photonview.RPC("UpdateScoreboard", PhotonTargets.All, false);
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
        Scoreboard scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();

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

    //Reset health and move Player to respawn area.
    void Respawn()
    {
        dead = false;
        current_health = max_health;

        //Move Player to respawn area if it belongs to the client.
        if (photonView.isMine)
        {
            // cameraRig.transform.position = respawnPt.position;
            cameraRig.GetComponent<TeamManager>().Respawn();
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(current_health);
        }
        else
        {
            current_health = (int)stream.ReceiveNext();
        }
    }
}