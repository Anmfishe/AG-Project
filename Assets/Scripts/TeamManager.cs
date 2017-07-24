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
    private GameObject rightHand;
    private bool set = false;
    private VRTK.VRTK_StraightPointerRenderer vrtk_spr;
    public Material blue_mat;
    public Material red_mat;
    [HideInInspector]
    public bool blue = false;
    PhotonView photonView;

    GameObject cameraRig;

    private void Awake()
    {

        photonView = GetComponent<PhotonView>();
    }
    // Use this for initialization
    void Start () {
        redSquares = GameObject.FindGameObjectsWithTag("RedPlatform");
        blueSquares = GameObject.FindGameObjectsWithTag("BluePlatform");

        //if (blue)
        //{

        //    transform.position = blueSquares[Random.Range(0, blueSquares.Length - 1)].transform.position;
        //    //if (vrtk_spr != null)
        //    //    vrtk_spr.blue = true;
        //}
        //else
        //{

        //    transform.position = redSquares[Random.Range(0, redSquares.Length - 1)].transform.position;
        //    //if (vrtk_spr != null)
        //    //    vrtk_spr.blue = false;
        //}
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
	void Update ()
    {
        if(rightHand != null && photonView.isMine && !set)
        {
            set = true;
            vrtk_spr = rightHand.GetComponent<VRTK.VRTK_StraightPointerRenderer>();
            if(blue && vrtk_spr != null)
            {
                vrtk_spr.blue = true;
            }
            else if(!blue && vrtk_spr != null)
            {
                vrtk_spr.blue = false;
            }
        }
        else if(!set)
        {
            rightHand = GameObject.Find("RightController");
        }
    }
    
    public void SetBlue()
    {
        Debug.Log("Set Blue + " + Time.time);
        
        blue = true;

        //Respawn();

        TeamSetter[] children = GetComponentsInChildren<TeamSetter>();
        foreach(TeamSetter ts in children)
        {
            ts.SetBlue();
        }
        rightHand = GameObject.Find("RightController");
        if (rightHand)
        {
            vrtk_spr = rightHand.GetComponent<VRTK.VRTK_StraightPointerRenderer>();
            vrtk_spr.blue = true;
        }
    }
    public void SetRed()
    {
        Debug.Log("Set Red + " + Time.time);
        
        blue = false;

        //Respawn();
        TeamSetter[] children = GetComponentsInChildren<TeamSetter>();
        foreach (TeamSetter ts in children)
        {
            ts.SetRed();
        }
        rightHand = GameObject.Find("RightController");
        if (rightHand)
        {
            vrtk_spr = rightHand.GetComponent<VRTK.VRTK_StraightPointerRenderer>();
            vrtk_spr.blue = false;
        }
    }

    public void Respawn()
    {
        redSquares = GameObject.FindGameObjectsWithTag("RedPlatform");
        blueSquares = GameObject.FindGameObjectsWithTag("BluePlatform");
        if (blue)
        {
            GameObject.FindGameObjectWithTag("CameraRig").GetComponent<PlatformController>().SetPlatform(blueSquares[Random.Range(0, blueSquares.Length - 1)].transform);
        }
        else
        {
            GameObject.FindGameObjectWithTag("CameraRig").GetComponent<PlatformController>().SetPlatform(redSquares[Random.Range(0, redSquares.Length - 1)].transform);
        }
    }

    public void SetAvatar(Transform _avatar)
    {
        avatar = _avatar;
        torso = avatar.Find("Torso");
        head = avatar.Find("Head");
    }
}
