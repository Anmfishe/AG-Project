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

    }
    void OnParticleCollision(GameObject other)
    {
        times_hit++;

        GameObject otherObject = other.transform.parent.gameObject;

        //Apply damage to object if it has the Player tag and implements the PlayerStatus script.
        if (otherObject.tag == "Player")
        {
            PlayerStatus statusScript = otherObject.GetComponent<PlayerStatus>();
            if(statusScript != null) statusScript.takeDamage(damage);
        }
        //Apply damage to object if it has the Shield tag and implements the Damageable script.
        else if (otherObject.tag == "Shield")
        {
            Damageable damageScript = otherObject.GetComponent<Damageable>();
            if (damageScript != null) damageScript.TakeDamage(damage);
        }
        else
        {
            print(other.tag);
        }
    }
}
