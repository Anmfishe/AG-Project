using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCooldowns : MonoBehaviour
{
    public float fireCD = 10f;
    public float iceCD = 10f;
    public float swordCD = 10f;
    public float meteorCD = 10f;
    public float shieldCD = 10f;
    public float pongCD = 10f;
    public float vinesCD = 10f;
    public float healCD = 10f;
    public float blessingCD = 10f;
    public float flipCD = 10f;

    [HideInInspector]
    public float fire;
    public float ice;
    public float sword;
    public float meteor;
    public float shield = 10f;
    public float pong = 10f;
    public float vines = 10f;
    public float heal = 10f;
    public float blessing = 10f;
    public float flip = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
