

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

	int attackTop = 2;
	int attackBottom = 0;

	int supportBottom = 3;
	int supportTop = 5;

	int healBottom = 6;
	int healTop = 9;

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
		if (playerClass == PlayerClass.attack) 
		{
			if (index > attackBottom)
			{
				index -= 1;
			}
			else 
			{
				index = attackTop;
			}
		}

		else if (playerClass == PlayerClass.support)
		{
			if (index > supportBottom)
			{
				index -= 1;
			}
			else 
			{
				index = supportTop;
			}
		}

		else if (playerClass == PlayerClass.heal) 
		{
			if (index > healBottom)
			{
				index -= 1;
			}
			else 
			{
				index = healTop;
			}
		}

		else if (playerClass == PlayerClass.all) 
		{
        if (index > 0)
        {
            index -= 1;
        }
        else
        {
            index = pages.Length - 2;
        }
		}
        if (animator)
            animator.SetTrigger("FlipRight");
        //  UpdateUI();
    }

    void FlipLeft()
    {

		if (playerClass == PlayerClass.attack) 
		{
			if (index < attackTop)
			{
				index += 1;
			}
			else 
			{
				index = attackBottom;
			}
		}

		else if (playerClass == PlayerClass.support)
		{
			if (index < supportTop)
			{
				index += 1;
			}
			else 
			{
				index = supportBottom;
			}
		}

		else if (playerClass == PlayerClass.heal) 
		{
			if (index < healTop)
			{
				index += 1;
			}
			else 
			{
				index = healBottom;
			}
		}

		else if (playerClass == PlayerClass.all) 
		{
			if (index < (pages.Length - 2))
			{
				index -= 1;
			}
			else
			{
				index = 0;
			}
		}
			
        if(animator)
            animator.SetTrigger("FlipLeft");
        // UpdateUI();
    }

    public void UpdateUI()
    {
		playerClass = playerStatus.playerClass;

			rend.material = pages[index];

		if (playerClass == PlayerClass.none) 
		{
			rend.material = pages[pages.Length-1];
		}

//		else if (playerClass == PlayerClass.attack) 
//		{
//			rend.material = pages[0];
//		}
//
//		else if (playerClass == PlayerClass.support)
//		{
//			rend.material = pages[2];
//		}
//
//		else if (playerClass == PlayerClass.heal) 
//		{
//			rend.material = pages[1];
//		}
    }
}