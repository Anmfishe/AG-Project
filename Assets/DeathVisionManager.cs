using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVisionManager : MonoBehaviour {

    public SpriteRenderer deathVision;
    public float final_a;
    public float duration;

	// Use this for initialization
	void Start () {
        //        deathVision.color = new Color(0f, 0f, 0f, 0.84f);
        deathVision.gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        Color color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, final_a), duration);
        deathVision.color = color;
    }

    public void TurnOn()
    {
        
    }
}
