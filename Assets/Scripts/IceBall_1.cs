﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall_1 : MonoBehaviour {
    public GameObject IceBall_2;
    public bool blue;
    float speed = 7.5f;
    int damage = 10;
    bool mine;

    [HideInInspector]
    public SpellcastingGestureRecognition spellcast;

    PhotonView photonView;
	// Use this for initialization
	void Start () {
        photonView = GetComponent<PhotonView>();
        StartCoroutine(lifetime());

        if (photonView.isMine)
        {
            mine = true;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (mine == true)
            SteamVR_Controller.Input(spellcast.rightControllerIndex).TriggerHapticPulse(300);


        if (Input.GetKeyDown("joystick button 15") && photonView.isMine) 
        {
            GameObject ib2 = PhotonNetwork.Instantiate(IceBall_2.name, transform.position, Quaternion.identity, 0);
            ib2.GetComponent<IceBall_2>().blue = blue;
            spellcast.Vibrate(.1f, 3999);
            PhotonNetwork.Destroy(photonView);
        }
	}
    private void FixedUpdate()
    {
        this.transform.Translate(this.transform.forward * speed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            print("hit on shield");
            //Apply damage to the shield.
            Damageable damageScript = collision.gameObject.GetComponent<Damageable>();
            if (damageScript != null) damageScript.TakeDamage(damage);
            //Instantiate new explosion.
            PhotonNetwork.Destroy(photonView);
        }
    }
    IEnumerator lifetime()
    {
        yield return new WaitForSeconds(4);
        GameObject ib2 = PhotonNetwork.Instantiate(IceBall_2.name, transform.position, Quaternion.identity, 0);
        ib2.GetComponent<IceBall_2>().blue = blue;
        print("BLUE:" + blue);
        print("HIS BLUE: " + ib2.GetComponent<IceBall_2>().blue);
        spellcast.Vibrate(.1f, 3999);

        if (mine == true)
            PhotonNetwork.Destroy(photonView);
    }

}
