using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBlade : MonoBehaviour {

    public GameObject hitSpark;
    private bool isDecaying = false;
    public float duration = 1;
    public float hitBonusTime = 0.25f;
    private float durationTimer = 0;
    public float damage = 50;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (isDecaying)
        {
            if (durationTimer > 0)
                durationTimer -= Time.deltaTime;
            else
                PhotonNetwork.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStatus>().takeDamage(damage);
            PhotonNetwork.Instantiate(hitSpark.name, other.transform.position, new Quaternion(), 0);
            if (isDecaying)
                durationTimer += hitBonusTime;
            else
            {
                durationTimer = duration;
                isDecaying = true;
            }
        }
    }
}
