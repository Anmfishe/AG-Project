﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class PadTeleport : MonoBehaviour
{
    public VRTK.VRTK_BasicTeleport basicTeleport;
    BeamTrail beamTrail;
    public LineRenderer lineRend;
    public Material reticleMat;

    public Gradient highlightColor;

    private GameObject reticle;

    private float reticleSize = 0.02f;
    private float reticleSizeUpdate = 0.05f;

    private Vector3[] points = new Vector3[2];

    public LayerMask blueLayersToIgnore;
    public LayerMask redLayersToIgnore;
    public LayerMask groundLayer;
    SpellcastingGestureRecognition spellcast;

    [HideInInspector]
    public bool blue;
	bool isOculus;

    bool active;
    public Transform origin;
    Transform padHit;
    bool neutral;
    Vector3 warpSpot;

	string teleportButton = "joystick button 9";

    VRTK.VRTK_StraightPointerRenderer vrtk_spr;
    bool set = false;
    GameObject rightHand;
    // Use this for initialization
    void Start ()
    {
        spellcast = GetComponent<SpellcastingGestureRecognition>();
        beamTrail = lineRend.GetComponent<BeamTrail>();
        
        // Set up the reticle
        reticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        reticle.transform.localScale = new Vector3(reticleSize, reticleSize, reticleSize);
        reticle.GetComponent<Renderer>().material = reticleMat;
        reticle.SetActive(false);

		if (VRDevice.model.ToLower ().Contains ("oculus"))
		{
			isOculus = true;
			teleportButton = "joystick button 0";
		}
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
//		if (Input.anyKeyDown)
//		{
//			print(Input.inputString);
//		}

        Vector3 fwd = origin.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        Physics.queriesHitTriggers = true;

        if (active == true)
        {
            lineRend.enabled = true;
            beamTrail.destination = origin.transform.position + origin.transform.forward * 10f;
            reticle.transform.position = origin.transform.position + origin.transform.forward * 10f;
            reticleSizeUpdate = reticleSize * Vector3.Distance(this.transform.position, reticle.transform.position);

            if (Physics.Raycast(origin.transform.position, fwd, out hit, 1000, blueLayersToIgnore))
            {
                    disableHighlight(padHit, blue);
                    padHit = hit.transform;
                    warpSpot = hit.point;
                    beamTrail.destination = warpSpot;
                    reticle.transform.position = hit.point;

                    reticleSizeUpdate = reticleSize * Vector3.Distance(this.transform.position, reticle.transform.position);

                if (padHit.gameObject.tag == "Neutral")
                {
                    neutral = true;
                }

                else if (padHit.gameObject.tag == "HatTable")
                {
                    Vector3 down = Vector3.down;
                    RaycastHit downHit;
                    neutral = true;

                    if (Physics.Raycast(warpSpot, down, out downHit, 1000, groundLayer))
                    {
                        warpSpot = downHit.point;
                        warpSpot += ((Vector3.Normalize(transform.position - downHit.point)) * .25f);
                        reticle.transform.position = downHit.point;
                        reticle.transform.position = warpSpot; 
                    }
                }

                // If it's the teleport pad
                else
                {
                    if (padHit.GetComponentInParent<PlatformNeighbors>().hasPlayer == true)
                    {
                        padHit = null;
                        neutral = false;
                    }

                    // If it doesn't have a player
                    else
                    {
                        enableHighlight(padHit, blue);
                        neutral = false;
                    }
                }
            }

            // If the raycast isn't successful
            else if (padHit != null)
            {
                disableHighlight(padHit, blue);
                padHit = null;
            }

            reticle.transform.localScale = new Vector3(reticleSizeUpdate, reticleSizeUpdate, reticleSizeUpdate);
        }

        else if (lineRend.enabled == true)
        {
            lineRend.enabled = false;
        }

        if (Input.GetKeyDown(teleportButton))
        {
            active = true;

            // Enable reticle
            reticle.SetActive(true);
        }

        // If we release the button
        if (Input.GetKeyUp(teleportButton))
        {
            active = false;

            // disable reticle
            reticle.SetActive(false);

            if (neutral == false && padHit!= null && (padHit.parent.gameObject.tag == "GrayPlatform" || (blue && padHit.parent.gameObject.tag == "BluePlatform") || (!blue && padHit.parent.gameObject.tag == "RedPlatform")))
                {
               // print("not NEUTRAL");
                basicTeleport.Teleport(padHit.transform, padHit.transform.position);
                }

                else if (neutral == true)
                {
                    basicTeleport.Teleport(padHit, warpSpot);
                }

            // Disable highlight
            if (padHit != null)
            {
                disableHighlight(padHit, blue);
                padHit = null;
            }

        lineRend.enabled = false;

        }

    }


    public void disableHighlight(Transform highlighted, bool myBlue)
    {
        if (highlighted != null && (highlighted.gameObject.tag == "GrayPlatform" || highlighted.gameObject.tag == "BluePlatform" || highlighted.gameObject.tag == "RedPlatform" || highlighted.gameObject.tag == "PlatformTrigger"))
        {

            if (highlighted.parent.childCount > 1)
                highlighted.parent.GetChild(1).gameObject.SetActive(false);
        }          
    }

    public void enableHighlight(Transform highlighted, bool myBlue)
    {
        if (highlighted.parent.childCount > 1)
        {
            if (highlighted.parent.GetChild(1).gameObject.activeSelf == true)
                   return;
        }

        var mainModule = highlighted.parent.GetChild(1).gameObject.GetComponent<ParticleSystem>().main;
        mainModule.startColor = highlightColor;

        if (highlighted.gameObject.tag == "PlatformTrigger")
        {
            if ((highlighted.parent.gameObject.tag == "GrayPlatform" || (myBlue && highlighted.parent.gameObject.tag == "BluePlatform") || (!myBlue && highlighted.parent.gameObject.tag == "RedPlatform")))
            {
                if (highlighted.parent.childCount > 1)
                {
                    highlighted.parent.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
    }
}
