using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadTeleport : MonoBehaviour
{
    public VRTK.VRTK_BasicTeleport basicTeleport;
    BeamTrail beamTrail;
    public LineRenderer lineRend;

    private Vector3[] points = new Vector3[2];

    public LayerMask blueLayersToIgnore;
    public LayerMask redLayersToIgnore;
    SpellcastingGestureRecognition spellcast;

    //[HideInInspector]
    public bool blue;

    bool active;
    public Transform origin;
    Transform padHit;
    bool neutral;
    Vector3 warpSpot;

    VRTK.VRTK_StraightPointerRenderer vrtk_spr;
    bool set = false;
    GameObject rightHand;
    // Use this for initialization
    void Start ()
    {
        spellcast = GetComponent<SpellcastingGestureRecognition>();
        beamTrail = lineRend.GetComponent<BeamTrail>();
	}

    private void OnEnable()
    {
    }


    private void FixedUpdate()
    {
        if (rightHand != null && !set)
        {
            vrtk_spr = rightHand.GetComponent<VRTK.VRTK_StraightPointerRenderer>();
            if (blue && vrtk_spr != null)
            {
                set = true;
                vrtk_spr.blue = true;
            }
            else if (!blue && vrtk_spr != null)
            {
                set = true;
                vrtk_spr.blue = false;
            }
        }
        else if (!set)
        {
            rightHand = GameObject.Find("RightController");
        }
    }

    // Update is called once per frame
    void Update ()
    {
        Vector3 fwd = origin.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (active == true)
        {
            lineRend.enabled = true;
          //  points[0] = transform.position;
           // points[1] = origin.transform.position + origin.transform.forward * .15f;
            beamTrail.destination = origin.transform.position + origin.transform.forward * 10f;
            //lineRend.SetPositions(points);

            if (padHit != null)
                disableHighlight(padHit);

            if (blue)
            {
                if (Physics.Raycast(origin.transform.position, fwd, out hit, 1000, blueLayersToIgnore))
                {
                    padHit = hit.transform;
                    warpSpot = hit.point;
                    beamTrail.destination = warpSpot;

                    if (padHit.gameObject.tag == "Neutral")
                    {
                        neutral = true;
                        
                    }

                    else
                    {
                        enableHighlight(padHit);
                        neutral = false;
                    }
                }

                else
                    if (padHit != null)
                        disableHighlight(padHit);

            }
            else
            {
                if (Physics.Raycast(origin.transform.position, fwd, out hit, 1000, redLayersToIgnore))
                {
                    padHit = hit.transform;
                    warpSpot = hit.point;
                    beamTrail.destination = warpSpot;
                    if (padHit.gameObject.tag == "Neutral")
                    {
                        neutral = true;
                    }

                    else
                    {
                        enableHighlight(padHit);
                        neutral = false;
                    }
                }

                else
                    if (padHit != null)
                        disableHighlight(padHit);

            }
        }

        else
        {
            lineRend.enabled = false;
        }

        if (Input.GetKeyDown("joystick button 9"))
        {
            active = true;
        }
        if (Input.GetKeyUp("joystick button 9"))
        {
            if (padHit != null)
                disableHighlight(padHit);

            active = false;
            if (padHit != null)
            {
                print("Pad hit! " +padHit);

                if (neutral == false)
                {
                    basicTeleport.Teleport(padHit.transform, padHit.transform.position);
                }

                else
                {
                    basicTeleport.Teleport(padHit.transform, warpSpot);
                }


            }

            lineRend.enabled = false;

        }

    }


    void disableHighlight(Transform highlighted)
    {
        if (highlighted.gameObject.tag == "GrayPlatform" || highlighted.gameObject.tag == "BluePlatform" || highlighted.gameObject.tag == "RedPlatform")
        {

            if (highlighted.childCount > 0)
                highlighted.GetChild(0).gameObject.SetActive(false);
        }
    
                
    }

    void enableHighlight(Transform highlighted)
    {
        if (highlighted.gameObject.tag == "GrayPlatform" || highlighted.gameObject.tag == "BluePlatform" || highlighted.gameObject.tag == "RedPlatform")
        {
            if (highlighted.childCount > 0)
                highlighted.GetChild(0).gameObject.SetActive(true);
        }


    }
}
