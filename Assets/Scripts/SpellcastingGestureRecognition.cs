using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR;
using Edwon.VR.Gesture;

public class SpellcastingGestureRecognition : MonoBehaviour {

    public GameObject fireball;
    public GameObject shield;
    public GameObject heal;
    public GameObject swipeLeft;
    public GameObject swipeRight;
    public AudioClip cast_success;
    public AudioClip cast_failure;

    public Targeting target;

    //public AudioClip spell_deflected;
    //--->Private Vars<---//
    Camera mainCam;
    float triggerL;
    float triggerR;
    SpellLogic spellLogic;
    bool triggerUsed = false;
    private AudioSource audioSource;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        spellLogic = GetComponent<SpellLogic>();
        spellLogic.mainCam = mainCam;
        audioSource = GetComponent<AudioSource>();
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

    void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble)
    {
        //string confidenceString = confidence.ToString().Substring(0, 4);
        //Debug.Log("detected gesture: " + gestureName + " with confidence: " + confidenceString);

        switch (gestureName)
        {
            case "Fire":
                //Transform t = null;
                //t.position = mainCam.transform.position;
                //t.LookAt(target.target);
                 GameObject fb = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0,.3f, 0),mainCam.transform.rotation, 0);

                // GameObject fb = Instantiate(fireball, mainCam.transform.position, mainCam.transform.rotation);

                break;
            case "Shield":
                if (target.target != null)
                {
                    Transform t = mainCam.transform;
                    t.position = mainCam.transform.position;
                    t.LookAt(target.target.position + new Vector3(0, 0.5f, 0));
                    GameObject fb2 = PhotonNetwork.Instantiate(shield.name, mainCam.transform.position - new Vector3(0, .3f, 0), mainCam.transform.rotation, 0);
                }
                else
                {
                    Transform t = mainCam.transform;
                    GameObject fb3 = PhotonNetwork.Instantiate(shield.name, mainCam.transform.position - new Vector3(0, .3f, 0), t.rotation, 0);
                }
                break;
            case "Heal":
                break;
            case "SwipeLeft":
                //   GameObject fb2 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), mainCam.transform.rotation, 0);
                spellLogic.Deflect();
                //audioSource.PlayOneShot(cast_success);
                break;
            case "SwipeRight":
                if (target.target != null)
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
}


