using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    
    public   float yOffset = 1.8f;
    public GameObject owner;
    public GameObject bar;
    private Transform camera;

    public bool isADummy = false;
    public Transform dummyCam;

    private Transform ownerTrans;
    public PlayerStatus ownerStat;
	// Use this for initialization
	void Start () {

        if (isADummy == false)
            camera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        else
            camera = dummyCam;

        
        ownerTrans = owner.GetComponent<Transform>();
//        ownerStat = owner.GetComponent<PlayerStatus>();
    }
	
	// Update is called once per frame
	void Update () {
       
        //       myTrans.position = new Vector3(owner.position.x, owner.position.y+yOffset,owner.position.z);

        GetComponent<Transform>().position = new Vector3(ownerTrans.position.x, ownerTrans.position.y + yOffset, ownerTrans.position.z);
        GetComponent<Transform>().forward = camera.forward;

        // Debug.Log(camera);
        if ((float)ownerStat.current_health >= 0)
        {
            setHealthbarScale((float)ownerStat.current_health / (float)ownerStat.max_health);
        }
        else
        {
            setHealthbarScale(0f);
        }

    }

    void setHealthbarScale(float maHealth)
    {
        // bar.transform.localScale = new Vector3(maHealth, bar.transform.localScale.y, bar.transform.localScale.z);
        bar.transform.localScale = new Vector3(Mathf.Lerp(bar.transform.localScale.x, maHealth, Time.deltaTime*5), bar.transform.localScale.y, bar.transform.localScale.z);
        
    }
}
