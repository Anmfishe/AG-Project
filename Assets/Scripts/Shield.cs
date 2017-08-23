using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public float shieldDuration = 10f;

    private float shieldTimer;
    Transform book;
    bool blue;
    Transform shieldSpot;

    // Use this for initialization
    void Start()
    {
        shieldTimer = shieldDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<PhotonView>().isMine)
        {
            if (book == null)
            {
                return;
            }

            this.transform.position = (shieldSpot.position);
            this.transform.rotation = shieldSpot.rotation;

            shieldTimer -= Time.deltaTime;

            if (shieldTimer <= 0)
                PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
        }
    }

    public void SetBook(Transform book_)
    {
        book = book_;
        shieldSpot = book.Find("ShieldPt");
    }

    public void SetBlue(bool blue_)
    {
        blue = blue_;
    }

    public bool GetBlue()
    {
        return blue;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ShieldBreaker"))
        {
            shieldTimer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ShieldBreaker"))
        {
            shieldTimer = 0;
        }
    }
} 
