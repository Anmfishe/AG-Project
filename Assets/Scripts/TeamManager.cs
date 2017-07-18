using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour {
    private GameObject[] redSquares;
    private GameObject[] blueSquares;
    private Transform avatar;
    private Transform torso;
    private Transform head;
    private Transform hat;
    public VRTK.VRTK_StraightPointerRenderer vrtk_spr;
    public Material blue_mat;
    public Material red_mat;
    [HideInInspector]
    public bool blue = false;
	// Use this for initialization
	void Start () {
        redSquares = GameObject.FindGameObjectsWithTag("RedPlatform");
        blueSquares = GameObject.FindGameObjectsWithTag("BluePlatform");
        //Debug.Log(num_players);
        //if(num_players % 2 == 0)
        //{
        //    SetBlue();
        //}
        //else
        //{
        //    SetRed();
        //}
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        
    }
    public void SetBlue()
    {
        blue = true;
        transform.position = blueSquares[Random.Range(0, blueSquares.Length - 1)].transform.position;
        vrtk_spr.blue = true;
        torso.GetComponent<Renderer>().material = blue_mat;
        head.GetComponent<Renderer>().material = blue_mat;
        hat.GetComponent<Renderer>().material = blue_mat;
    }
    public void SetRed()
    {
        blue = false;
        transform.position = redSquares[Random.Range(0, redSquares.Length - 1)].transform.position;
        vrtk_spr.blue = false;
        torso.GetComponent<Renderer>().material = red_mat;
        head.GetComponent<Renderer>().material = red_mat;
        hat.GetComponent<Renderer>().material = red_mat;
    }

    public void Respawn()
    {
        if (blue == false)
        {
            transform.position = redSquares[Random.Range(0, redSquares.Length - 1)].transform.position;
        }
        else
        {
            transform.position = blueSquares[Random.Range(0, blueSquares.Length - 1)].transform.position;
        }
    }
    public void SetAvatar(Transform _avatar)
    {
        avatar = _avatar;
        torso = avatar.Find("Torso");
        head = avatar.Find("Head");
        hat = head.Find("hat1").Find("Circle");
    }
}
