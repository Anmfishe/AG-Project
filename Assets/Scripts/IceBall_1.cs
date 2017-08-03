using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall_1 : MonoBehaviour {
    public GameObject IceBall_2;
    public bool blue;
    float speed = 7.5f;
    int damage = 10;
    PhotonView photonView;
	// Use this for initialization
	void Start () {
        photonView = GetComponent<PhotonView>();
        StartCoroutine(lifetime());
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp("joystick button 15") && photonView.isMine) 
        {
            PhotonNetwork.Instantiate(IceBall_2.name, transform.position, Quaternion.identity, 0);
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
        yield return new WaitForSeconds(10);
        GameObject ib2 = PhotonNetwork.Instantiate(IceBall_2.name, transform.position, Quaternion.identity, 0);
        ib2.GetComponent<IceBall_2>().blue = blue;
        PhotonNetwork.Destroy(photonView);
    }

}
