

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookLogic : MonoBehaviour
{
	private PlayerStatus playerStatus; 
	private PlayerClass playerClass;
    GameObject page;
    public Material[] pages;
    Renderer rend;
    Animator animator;

    SteamVR_TrackedObject trackedObj;
    SteamVR_Controller.Device device;

    float trackpadPos;
    float startPressPos;
    float swipeThresh = 0.03f;
    int index = 0;

    private void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start()
    {
		playerStatus = transform.parent.parent.GetComponentInChildren<PlayerStatus> ();
		//print ("PLAYER" + playerStatus);

        if (transform.GetChild(1)!= null)
        {
					
            page = this.gameObject.transform.GetChild(1).gameObject;
            rend = page.GetComponent<Renderer>();
                

			rend.material = pages [pages.Length-1];
            animator = GetComponent<Animator>();
        }
        //page.Set
    }

    // Update is called once per frame
    void Update()
    {
        trackpadPos = Input.GetAxis("TrackpadHoriz");
        if (Input.GetKeyDown("joystick button 16"))
        {
            startPressPos = trackpadPos;
            //if (trackpadPos < -0.05f)
            //{
            //    FlipLeft();
            //}
            //else if (trackpadPos > 0.05f)
            //{
            //    FlipRight();
            //}
        }

        if (Input.GetKeyUp("joystick button 16"))
        {
            if (trackpadPos > startPressPos + swipeThresh )
            {
                FlipRight();
            }

            else if (trackpadPos < startPressPos - swipeThresh)
            {
                FlipLeft();
            }

        }


            //if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            //{
            //    Vector2 touchpad = (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
            //    print("Pressing Touchpad");

            //    if (touchpad.y > 0.7f)
            //    {
            //        print("Moving Up");
            //    }

            //    else if (touchpad.y < -0.7f)
            //    {
            //        print("Moving Down");
            //    }

            //    if (touchpad.x > 0.7f)
            //    {
            //        print("Moving Right");
            //        FlipRight();

            //    }

            //    else if (touchpad.x < -0.7f)
            //    {
            //        print("Moving left");
            //        FlipLeft();
            //    }
            if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FlipLeft();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //if (SwipeLeft)
                FlipRight();
        }
    }

    private void FixedUpdate()
    {
        // device = SteamVR_Controller.Input((int)trackedObj.index);
    }

    void FlipRight()
    {
        if (index > 0)
        {
            index -= 1;
        }
        else
        {
            index = pages.Length - 2;
        }
        if (animator)
            animator.SetTrigger("FlipRight");
        //  UpdateUI();
    }

    void FlipLeft()
    {
        if (index < (pages.Length - 2))
        {
            index += 1;
        }
        else
        {
            index = 0;
        }
        if(animator)
            animator.SetTrigger("FlipLeft");
        // UpdateUI();
    }

    public void UpdateUI()
    {
		playerClass = playerStatus.playerClass;

		if (playerClass == PlayerClass.all) 
		{
			rend.material = pages[index];
            SpellcastingGestureRecognition sgr = Camera.main.transform.parent.GetComponent<SpellcastingGestureRecognition>();
            if (sgr == null)
            {
                Debug.Log("BookLogic.cs : UpdateUI() : sgr is null!");
                return;
            }
            sgr.SetSpell(index);
            print("BookLogic.cs : UpdateUI() : Should have set spell " + index);
		}

		else if (playerClass == PlayerClass.none) 
		{
			rend.material = pages[pages.Length-1];
		}

		else if (playerClass == PlayerClass.attack) 
		{
			rend.material = pages[0];
		}

		else if (playerClass == PlayerClass.support)
		{
			rend.material = pages[2];
		}

		else if (playerClass == PlayerClass.heal) 
		{
			rend.material = pages[1];
		}
    }
}