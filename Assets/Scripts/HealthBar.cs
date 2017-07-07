using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    public int max_health;
    public int current_health;
    private float yOffset = 1;
    public Transform owner;
    public GameObject bar;
    private Transform camera;
    private Transform myTrans;

	// Use this for initialization
	void Start () {
        camera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        myTrans = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        myTrans.position = new Vector3(owner.position.x, owner.position.y+yOffset,owner.position.z);
        myTrans.forward = camera.forward;
        setHealthbarScale((float)current_health / (float)max_health);
    }

    void setHealthbarScale(float maHealth)
    {
        bar.transform.localScale = new Vector3(maHealth, bar.transform.localScale.y, bar.transform.localScale.z);
    }
}
