using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hat_put : MonoBehaviour {

    // Detect the collision of hat and head
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "put")
        {
          gameObject.transform.SetParent(other.gameObject.transform);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            // Search for the child hat in player
            foreach (Transform child in other.transform) if (child.CompareTag("findHat"))
            {
                    gameObject.transform.position = child.transform.position;
                    gameObject.transform.rotation = child.transform.rotation;
                    //gameObject.transform.scale = child.transform.scale;
                }

        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
