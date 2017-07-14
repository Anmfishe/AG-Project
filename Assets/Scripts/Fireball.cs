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
        if (other.tag == "SpellHitter")
        {
            print("VELOCITY: " + other.GetComponent<Rigidbody>().velocity.magnitude);
            if (other.GetComponent<Rigidbody>().velocity.magnitude > 1)
            {
                //Get particles.
                ParticleSystem.Particle[] emittedParticles = new ParticleSystem.Particle[this.GetComponent<ParticleSystem>().particleCount];
                this.GetComponent<ParticleSystem>().GetParticles(emittedParticles);

                //Get first particle's position. Get the rotation by looking at the original fireball's position.
                Vector3 particlePosition = emittedParticles[0].position;
                GameObject reflectedFireball = PhotonNetwork.Instantiate("Fireball_Spell", particlePosition, Quaternion.LookRotation(this.transform.forward * -1, this.transform.up * -1), 0);
                reflectedFireball.transform.localScale = new Vector3(2, 2, 2);
                //Destroy original.
                Destroy(this.gameObject);
            }
            else
            {
                PlayerStatus statusScript = other.transform.parent.Find("Torso").GetComponent<PlayerStatus>();
                if (statusScript != null) statusScript.takeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Player")
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
        else
        {
           // print(other.tag);
        }
    }
}
