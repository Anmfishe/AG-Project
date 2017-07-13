﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR;
using Edwon.VR.Gesture;

public class SpellcastingGestureRecognition : MonoBehaviour {

    public GameObject fireball;
    public Gradient fireballGradient;
    public GameObject shield;
    public Gradient shieldGradient;
    public Gradient healGradient;
    public GameObject heal;
    public GameObject swipeLeft;
    public GameObject swipeRight;
    public AudioClip cast_success;
    public AudioClip cast_failure;
    public Transform wand;
    public Transform book;
   // [HideInInspector]
    public Targeting target;
    public Transform avatar;

    //public AudioClip spell_deflected;
    //--->Private Vars<---//
    Camera mainCam;
    float triggerL;
    float triggerR;
    SpellLogic spellLogic;
    bool triggerUsed = false;
    private AudioSource audioSource;

    public bool hasSpell;
    public GameObject currentSpell;
    public Gradient currentSpellGradient;
    public float spellCooldown = 3f;
    private float spellTimer = 0;
    private bool isCoolingDown = false;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        spellLogic = GetComponent<SpellLogic>();
        spellLogic.mainCam = mainCam;
        audioSource = GetComponent<AudioSource>();
        //target = GetComponentInChildren<Targeting>();
        //if (target == null)
        //{
        //    Debug.Log("target is NULL");
        //}
    }

    void OnEnable()
    {
        GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
        GestureRecognizer.GestureRejectedEvent += OnGestureRejected;
    }

    void OnDisable()
    {
        GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
        GestureRecognizer.GestureRejectedEvent -= OnGestureRejected;
    }


    private void Update()
    {
        //Check if we're cooling down.
        if (isCoolingDown)
        {
            //If fireball timer is still active.
            if (spellTimer > 0)
            {
                spellTimer -= Time.deltaTime;
            }
            else
            {
                isCoolingDown = false;
                GetComponent<VRGestureRig>().enabled = true;
                if (wand != null)
                {
                    wand.Find("tip").Find("spark").GetComponent<ParticleSystem>().Play();
                    //wand.Find("tip").Find("spark").Find("dust").GetComponent<ParticleSystem>().Play();
                }

            }
        }
        if (hasSpell && Input.GetKeyDown("joystick button 15"))
        {
            spellTimer = spellCooldown;

            if(currentSpell == heal)
            {
                print("heal");
                //currentSpell.GetComponent<HealSpell>().target = target.target;
            }
            GameObject fireballGO = PhotonNetwork.Instantiate(currentSpell.name, wand.Find("tip").position, wand.Find("tip").rotation, 0);

            if (target != null && target.target != null)
            {
                fireballGO.transform.LookAt(target.target);
                Debug.Log("target");
            }
            else
            {
                Debug.Log("no target");
            }
            

            hasSpell = false;
            currentSpell = null;

            if (wand != null)
            {
                wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>().Stop();
                ParticleSystem ps = wand.Find("tip").Find("smoke").GetComponent<ParticleSystem>();
                ps.Stop();
                var main = ps.main;
                main.duration = spellCooldown;
                ps.Play();
            }
            isCoolingDown = true;
        }
    }

    void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble)
    {
        //string confidenceString = confidence.ToString().Substring(0, 4);
        //Debug.Log("detected gesture: " + gestureName + " with confidence: " + confidenceString);

        switch (gestureName)
        {
            case "Fire":
                currentSpell = fireball;
                currentSpellGradient = fireballGradient;
                hasSpell = true;
                {
                    ParticleSystem wandParticle = wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>();
                    wandParticle.Stop();
                    var wandParticleModule = wandParticle.colorOverLifetime;
                    wandParticleModule.color = currentSpellGradient;
                    wandParticle.Play();
                }
                GetComponent<VRGestureRig>().enabled = false;
                //Transform t = null;
                //t.position = mainCam.transform.position;
                //t.LookAt(target.target);
                //GameObject fb = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0,.3f, 0),mainCam.transform.rotation, 0);

                // GameObject fb = Instantiate(fireball, mainCam.transform.position, mainCam.transform.rotation);
                audioSource.PlayOneShot(cast_success);
                break;
            case "Shield":
                currentSpell = shield;
                currentSpellGradient = shieldGradient;
                hasSpell = true;
                if (wand != null)
                {
                    ParticleSystem wandParticle = wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>();
                    wandParticle.Stop();
                    var wandParticleModule = wandParticle.colorOverLifetime;
                    wandParticleModule.color = currentSpellGradient;
                    wandParticle.Play();
                }
                GetComponent<VRGestureRig>().enabled = false;
                /*
                if (target!=null && target.target != null)
                {
                    Transform t = mainCam.transform;
                    t.position = mainCam.transform.position;
                    t.LookAt(target.target.position + new Vector3(0, 0.5f, 0));
                    GameObject fb2 = PhotonNetwork.Instantiate(shield.name, wand.Find("tip").position, wand.Find("tip").rotation, 0);
                }
                else
                {
                    Transform t = mainCam.transform;
                    GameObject fb3 = PhotonNetwork.Instantiate(shield.name, wand.Find("tip").position, wand.Find("tip").rotation, 0);
                }
                */
                audioSource.PlayOneShot(cast_success);
                break;
            case "Heal":
                currentSpell = heal;
                currentSpellGradient = healGradient;
                hasSpell = true;
                if (wand != null)
                {
                    ParticleSystem wandParticle = wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>();
                    wandParticle.Stop();
                    var wandParticleModule = wandParticle.colorOverLifetime;
                    wandParticleModule.color = currentSpellGradient;
                    wandParticle.Play();
                }
                GetComponent<VRGestureRig>().enabled = false;
                break;
            case "SwipeLeft":
                //   GameObject fb2 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), mainCam.transform.rotation, 0);
                spellLogic.Deflect();
                //audioSource.PlayOneShot(cast_success);
                break;
            case "SwipeRight":
                if (target != null && target.target != null)
                {
                    Transform t = mainCam.transform;
                t.position = mainCam.transform.position;
                t.LookAt(target.target.position + new Vector3(0, 0.5f, 0));
                GameObject fb3 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), t.rotation, 0);
                }
                else
                {
                    Transform t = mainCam.transform;
                    GameObject fb3 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), t.rotation, 0);
                }
                audioSource.PlayOneShot(cast_success);
                //spellLogic.Deflect();
                //GameObject fb3 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), mainCam.transform.rotation, 0);
                break;
        }
    }

    void OnGestureRejected(string error, string gestureName = null, double confidenceValue = 0)
    {
        audioSource.PlayOneShot(cast_failure);
    }

    public void SetAvatar(Transform _avatar)
    {
        avatar = _avatar;
        wand = avatar.Find("Right Hand").Find("MagicWand");
        book = avatar.Find("Left Hand").Find("SpellBook");
    }
}


