using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    Transform respawnPt;
    Transform timeOutPt;

    GameObject cameraRig;

    Scoreboard scoreboard;

    bool dead = false;
    float deathTime = 0f;
    float respawnLength = 2f;


    [HideInInspector]
    public int max_health = 100;
    [HideInInspector]
    public int current_health = 100;
    //   public int hp = 100;
    // Use this for initialization
    void Start()
    {
        if (this.GetComponent<PhotonView>().isMine)
        {
            cameraRig = Camera.main.transform.parent.parent.gameObject;
        }

        scoreboard = GameObject.FindGameObjectWithTag("Scoreboard").GetComponent<Scoreboard>();


        timeOutPt = GameObject.FindGameObjectWithTag("TimeOut").transform;
        respawnPt = GameObject.FindGameObjectWithTag("RespawnDefault").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            if ((Time.time - deathTime) >  respawnLength)
            {
                Respawn();
            }
        }
    }

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
        if (this.GetComponent<PhotonView>().isMine)
        {
           cameraRig.transform.position = timeOutPt.position;
            scoreboard.IncrementRedScore();
        }

        deathTime = Time.time;
        dead = true;
      //  Respawn();
    }

    void Respawn()
    {
        dead = false;
        current_health = max_health;

        if (this.GetComponent<PhotonView>().isMine)
        {
            cameraRig.transform.position = respawnPt.position;
        }

        // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // cube.transform.position = transform.position + new Vector3(0, 3, 0);
        //  gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(255f, 0, 0, 0);
        // transform.position = respawnPt.position;
    }
}