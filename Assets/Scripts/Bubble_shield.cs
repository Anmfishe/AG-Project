using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_sheild : MonoBehaviour {

    public float shieldDuration = 5f;
    private float shieldTimer;
    Transform book;
    bool blue;

    // Use this for initialization
    void Start () {
        shieldTimer = shieldDuration;
    }
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<PhotonView>().isMine)
        {
            if (book == null)
            {
                return;
            }

            this.transform.position = book.position + book.forward;
            this.transform.rotation = book.rotation;
        }

        shieldTimer -= Time.deltaTime;
        if (shieldTimer <= 0)
            Destroy(gameObject);
    }

    public void SetBook(Transform book_)
    {
        book = book_;
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
