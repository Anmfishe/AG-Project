using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    Transform respawnPt;
    int hp = 100;
    // Use this for initialization
    void Start()
    {
        respawnPt = GameObject.FindGameObjectWithTag("RespawnDefault").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void takeDamage(int damage)
    {
        hp -= damage;
        
        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
       Respawn();
    }

    void Respawn()
    {
        print("dead");
       // GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
       // cube.transform.position = transform.position + new Vector3(0, 3, 0);
      //  gameObject.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(255f, 0, 0, 0);
       // transform.position = respawnPt.position;
    }
    //void OnParticleCollision(Collision collision)
    //{
    //  //  print("too swag");
    //}
}
