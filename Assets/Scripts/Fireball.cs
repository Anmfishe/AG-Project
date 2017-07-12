using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
    int times_hit = 0;
    int damage = 100;
    // Use this for initialization
    void Start() {
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update() {

    }
    void OnParticleCollision(GameObject other)
    {
        times_hit++;
        //print("hit");
        // print(other);
        if (other.transform.parent.gameObject.tag == "Player")// && times_hit > 1)
        {
            other.transform.parent.gameObject.GetComponent<PlayerStatus>().takeDamage(damage);
        }

        else
        {
            print(other.tag);
        }

        //other.transform.parent!=null && 

        //if ((other) && (other.transform.parent.gameObject != null) && other.transform.parent.gameObject.tag == "Player")
        //{
        //    print("okie");
        //    other.transform.parent.gameObject.GetComponent<PlayerStatus>().takeDamage(damage);
        //}
        //Rigidbody body = other.GetComponent<Rigidbody>();
        //if (body)
        //{
        //    Vector3 direction = other.transform.position - transform.position;
        //    direction = direction.normalized;
        //    body.AddForce(direction * 5);
        //}
    }
}
