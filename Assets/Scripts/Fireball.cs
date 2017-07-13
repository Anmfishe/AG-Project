using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    int times_hit = 0; //Doesn't the fireball die on first hit?
    public int damage = 20;
    public float duration = 10f;

    // Use this for initialization
    void Start() {
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update() {
        print(GetComponent<ParticleSystem>().particleCount);
    }
    void OnParticleCollision(GameObject other)
    {
        print(other + " : " + other.tag);
        times_hit++;

        //Apply damage to object if it has the Player tag and implements the PlayerStatus script.
        if (other.tag == "Player")
        {
            PlayerStatus statusScript = other.GetComponent<PlayerStatus>();
            if(statusScript != null) statusScript.takeDamage(damage);
        }
        //Apply damage to object if it has the Shield tag and implements the Damageable script.
        else if (other.tag == "Shield")
        {
            Damageable damageScript = other.GetComponent<Damageable>();
            if (damageScript != null) damageScript.TakeDamage(damage);
        }
        else if(other.tag == "SpellHitter")
        {
            if(other.GetComponent<Rigidbody>().velocity.magnitude > 4)
            {
                GameObject reflectedFireball = PhotonNetwork.Instantiate("Fireball_Spell", this.GetComponent<ParticleSystem>().transform.position, this.transform.rotation, 0);
                //reflectedFireball.transform.LookAt(this.transform.position);

                Destroy(this.gameObject);
            }
        }
        else
        {
            print(other.tag);
        }
    }
}
