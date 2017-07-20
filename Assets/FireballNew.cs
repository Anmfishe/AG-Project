using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballNew : MonoBehaviour {

    public int damage = 20;
    public float duration = 12f;
    public Vector3 direction; //Sets the direction to where the fireball is traveling to.
    public float speed = 1f;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, duration);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(this.transform.forward * speed * Time.deltaTime, Space.World);
	}

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        if(other.CompareTag("Player"))
        {
            //Apply damage to object if it has the Player tag and implements the PlayerStatus script.
            PlayerStatus statusScript = other.GetComponent<PlayerStatus>();
            if (statusScript != null) statusScript.takeDamage(damage);

        }
        else if (other.CompareTag("Shield"))
        {
            //Apply damage to the shield.
            Damageable damageScript = other.GetComponent<Damageable>();
            if (damageScript != null) damageScript.TakeDamage(damage);
        }
        else
        {

        }
		Destroy (this.gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        if (other.CompareTag("SpellHitter"))
        {
            //Create reflected fireball if it was hit hard enough by the spell hitter.
            Rigidbody otherBody = other.GetComponent<Rigidbody>();
            if (otherBody.velocity.magnitude > 2 || otherBody.angularVelocity.magnitude > 2)
            {
                GameObject reflectedFireball = PhotonNetwork.Instantiate("Fireball", other.transform.position, Quaternion.LookRotation(this.transform.forward * -1, this.transform.up * -1), 0);
            }
        }

    }

}
