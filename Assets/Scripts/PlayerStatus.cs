using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Transform respawnPt;
    private Transform timeOutPt;
    public PhotonView photonView;
    private GameObject cameraRig;

    private Scoreboard scoreboard;

    private bool dead = false;
    private float deathTime = 0f;
    public float respawnLength = 2f;


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

        //Why are we assigning this on runtime? It could be assigned through the NetworkManager.
        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();

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
        if (dead == false)
        {
            current_health -= damage;
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

            if (cameraRig.GetComponent<TeamManager>() == null)
            {

            }

            if (cameraRig.GetComponent<TeamManager>().blue)
            {
                scoreboard.IncrementRedScore();
            }
            else
            {
                scoreboard.IncrementBlueScore();
            }
        }

        deathTime = Time.time;
        dead = true;
      //  Respawn();
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
}