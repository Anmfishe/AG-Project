using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineTrap : MonoBehaviour {

    public float duration;
    public float durationTimer;
    public bool isSet;
    public bool isActivated;
    public Transform player;
    public PlayerStatus playerStatus;

    public float damagePerCycle;
    public float damageCycle;
    public float damageTimer;

    public Transform seed;
    public Transform explosion;
    public Transform body;

	// Use this for initialization
	void Start () {
        SetUp();
	}
	
	// Update is called once per frame
	void Update () {
        if (isActivated)
        {
            /*
            if (durationTimer > 0)
                durationTimer -= Time.deltaTime;
            else
                PhotonNetwork.Destroy(this.gameObject);
            */



            if (damageTimer > 0)
                damageTimer -= Time.deltaTime;
            else
                DealDamage();
        }
	}
    void SetUp()
    {
        isSet = true;
        seed.gameObject.SetActive(true);
    }

    void Activate()
    {
        isActivated = true;
        seed.gameObject.SetActive(false);
        explosion.gameObject.SetActive(true);
        body.gameObject.SetActive(true);
        DealDamage();
        durationTimer = duration;
    }
    void DealDamage()
    {
        damageTimer = damageCycle;
        //Destroy if player dies.
        if(playerStatus.takeDamage(damagePerCycle))
            PhotonNetwork.Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (isActivated) return;

        Transform other = trigger.transform;
        print(other.tag);

        if(other.CompareTag("Player"))
        {
            player = other;
            playerStatus = player.GetComponent<PlayerStatus>();
            Activate();
        }


    }

}
