using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_shield : MonoBehaviour
{
   
    public float Bubble_shieldDuration = 10f;
    private float Bubble_shieldTimer;
    Transform torso;
    Collider other;
    bool blue;
    //Transform Bubble_shieldSpot;

    // Use this for initialization
    void Start()
    {
        Bubble_shieldTimer = Bubble_shieldDuration;
    }

    // Update is called once per frame
    void Update()
    {
       
            if (this.GetComponent<PhotonView>().isMine)
            {
                //if (other.transform.parent.GetComponent<TeamManager>().blue = blue)
                //{
                    if (torso == null)
                    {
                        return;
                    }

                    this.transform.position = torso.position;
                    this.transform.rotation = torso.rotation;
            if (Bubble_shieldTimer <= 0)
            PhotonNetwork.Destroy(gameObject);
               // }
        }
        Bubble_shieldTimer -= Time.deltaTime;
        
    }

    public void SetTorso(Transform torso_)
    {
        torso = torso_;
        StartCoroutine(torso.GetComponent<PlayerStatus>().setBubble_shield(Bubble_shieldDuration));
        //Bubble_shieldSpot = torso.Find("Bubble_ShieldPt");
    }

    public void SetBlue(bool blue_)
    {
        blue = blue_;
    }

    public bool GetBlue()
    {
        return blue;
    }

}
