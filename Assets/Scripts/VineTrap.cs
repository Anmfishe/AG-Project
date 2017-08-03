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
             * COMMENTED DUE TO CHANGE IN MECHANICS, VINE TRAP NOW CAN ONLY BE RID BY DISENCHANT SPELL *
            if (durationTimer > 0)
                durationTimer -= Time.deltaTime;
            else
                PhotonNetwork.Destroy(this.gameObject);
            */


            //Deals damage every time the timer reaches 0.
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
        //Flag as activated.
        isActivated = true;
        //Disable seed particle.
        seed.gameObject.SetActive(false);

        //Enable explosion and body particles.
        explosion.gameObject.SetActive(true);
        body.gameObject.SetActive(true);

        //Disable player's movement.
        playerStatus.EnableMovement(false);

        //Start dealing damage.
        DealDamage();

        //Set duration timer.
        durationTimer = duration;
    }
    void DealDamage()
    {
        damageTimer = damageCycle;
        //Destroy if player dies.
        if (playerStatus.takeDamage(damagePerCycle))
        {
            //Enable movement before destroy itself.
            DestroyVines();
        }
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

    [PunRPC]
    public void DestroyVines()
    {
        if (playerStatus != null)
            playerStatus.EnableMovement(true);

        PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
    }

}
