using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_shield : MonoBehaviour
{
   
    public float shieldDuration = 5f;
    private float shieldTimer;
    Transform torso;
    bool blue;

    // Use this for initialization
    void Start()
    {
        shieldTimer = shieldDuration;
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.GetComponent<PhotonView>().isMine)
        //{
            if (torso == null)
            {
                return;
            }

            this.transform.position = torso.position;
            this.transform.rotation = torso.rotation;
       // }

        shieldTimer -= Time.deltaTime;
        if (shieldTimer <= 0)
            Destroy(gameObject);
    }
   
    public void SetBook(Transform torso_)
    {
        torso = torso_;
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
