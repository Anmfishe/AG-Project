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

    Camera mainCam;
    float triggerL;
    float triggerR;

    bool triggerUsed = false;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
                GameObject fb = Instantiate(fireball, mainCam.transform.position, mainCam.transform.rotation);
                
            break;
            case "Shield":
                break;
            case "Heal":
                break;
            case "SwipeLeft":
                GameObject fb2 = Instantiate(fireball, mainCam.transform.position, mainCam.transform.rotation);
                break;
            case "SwipeRight":
                GameObject fb3 = Instantiate(fireball, mainCam.transform.position, mainCam.transform.rotation);
                break;
        }
    }

    void OnGestureRejected(string error, string gestureName = null, double confidenceValue = 0)
    {
    }
}


