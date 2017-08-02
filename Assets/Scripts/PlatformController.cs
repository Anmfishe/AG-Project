﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    public LayerMask blue_platforms;
    public LayerMask red_platforms;
    public Transform target;
    private LayerMask mask;
    [HideInInspector]
    public Transform currPlatform;
    [HideInInspector]
    public GameObject avatar;
    //public bool useRight = true;
    //public bool useLeft = true;
    [HideInInspector]
    public bool lerp = true;
   
    public float CD = 1;
    public AudioClip cd_Sound;
    [HideInInspector]
    public bool canMove = true;
    private Vector3 targetPos;
    private AudioSource audS;
    private GameObject camObj;
    float trackpadPosHorizontal;
    float trackpadPosVertical;
    float startPressPosHoriz;
    float startPressPosVert;
    float swipeThresh = 0.03f;
    public float speed = 10f;

    // Use this for initialization
    private void Awake()
    {
        camObj = GetComponentInChildren<Camera>().gameObject;
    }
    void Start () 
	{
        audS = GetComponent<AudioSource>();
        //blue_platforms = ~(int)1 << LayerMask.NameToLayer("BluePlatform");
        //red_platforms = ~(int)1 << LayerMask.NameToLayer("RedPlatform");
        if (avatar != null && avatar.GetComponent<TeamManager>().blue)
        {
            mask = blue_platforms;
        }
        else
        {
            mask = red_platforms;
        }
        Physics.queriesHitBackfaces = false;
	}
    void FixedUpdate()
    {
        //device1 = SteamVR_Controller.Input((int)trackedObj1.index);
        //device2 = SteamVR_Controller.Input((int)trackedObj2.index);
        if (avatar != null)
        {
            if (avatar.GetComponent<TeamManager>().blue && avatar.GetComponent<TeamManager>().photonView.isMine)
            {
                mask = blue_platforms;
            }
            else
            {
                mask = red_platforms;
            }
        }
        else
        {
        }
    }

    private void OnEnable()
    {
        canMove = true;
        lerp = true;
    }
    // Update is called once per frame
    void Update()
    {
        target.position = currPlatform.position;
        Vector3 rightVec = Quaternion.AngleAxis(90, target.up) * target.forward * 100;
        rightVec.y = 0;
		Vector3 forwardVec = target.forward;// * 100;
        forwardVec.y = 0;
        Vector3 leftVec = Quaternion.AngleAxis(-90, target.up) * target.forward * 100;
        leftVec.y = 0;
        Vector3 backVec = Quaternion.AngleAxis(180, target.up) * target.forward * 100;
        backVec.y = 0;
        Debug.DrawRay(target.position, forwardVec, Color.black);
        Debug.DrawRay(target.position, rightVec, Color.red);
        Debug.DrawRay(target.position, leftVec, Color.blue);
        Debug.DrawRay(target.position, backVec, Color.green);
        if (lerp)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);

            trackpadPosHorizontal = Input.GetAxis("TrackpadHoriz2");
            trackpadPosVertical = Input.GetAxis("TrackpadVert");
            if (Input.GetKeyDown("joystick button 17"))
            {
                startPressPosHoriz = trackpadPosHorizontal;
                startPressPosVert = trackpadPosVertical;
				//print(startPressPosHoriz + " | " + startPressPosVert);
			}
            if (Input.GetKeyUp("joystick button 17") && canMove)
            {
                
                PlatformNeighbors currNeighborhood = currPlatform.GetComponent<PlatformNeighbors>();

				Vector2 startPress = new Vector2 (startPressPosHoriz, startPressPosVert);
				Vector2 endPress = new Vector2 (trackpadPosHorizontal, trackpadPosVertical);


				Vector3 cross = Vector3.Cross(startPress, endPress);
				float angle = Vector2.Angle (startPress, endPress);
				if (cross.y < 0) 
					angle = -angle;



				if (Vector2.Distance(startPress, endPress) > swipeThresh)
				{
					RaycastHit up;
					print (forwardVec);
					Debug.DrawRay(target.position, (forwardVec + new Vector3(angle, 0,0)), Color.red, 10);
					if (Physics.Raycast(target.position, forwardVec + new Vector3(angle, 0,0), out up, 10, mask))
					{
						MoveUp(up.collider.gameObject);
					}
				}
//				print(startPress + " | " + endPress);


//                if (trackpadPosHorizontal > startPressPosHoriz + swipeThresh /*&& currNeighborhood.right != null*/ )
//                {
//                    RaycastHit right;
//                    if (Physics.Raycast(target.position, rightVec, out right, 10, mask))
//                    {
//                        MoveRight(right.collider.gameObject);
//                    }
//                }
//
//                else if (trackpadPosHorizontal < startPressPosHoriz - swipeThresh /*&& currNeighborhood.left != null*/)
//                {
//                    RaycastHit left;
//                    if (Physics.Raycast(target.position, leftVec, out left, 10, mask))
//                    {
//                        MoveLeft(left.collider.gameObject);
//                    }
//                }
//                else if (trackpadPosVertical > startPressPosVert + swipeThresh /*&& currNeighborhood.up != null*/)
//                {
//                    RaycastHit up;
//                    if (Physics.Raycast(target.position, forwardVec, out up, 10, mask))
//                    {
//                        MoveUp(up.collider.gameObject);
//                    }
//                }
//                else if (trackpadPosVertical < startPressPosVert - swipeThresh /*&& currNeighborhood.down != null*/)
//                {
//                    RaycastHit down;
//                    if (Physics.Raycast(target.position, backVec, out down, 10, mask))
//                    {
//                        MoveDown(down.collider.gameObject);
//                    }
//                }
            }
        }




        /*if ((device1 != null && device1.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)))
        {
            if (!canMove)
            {
                play_err();
            }
            else
            {
                Vector2 touchpad = (device1.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

                if (touchpad.y > 0.7f)
                {
                    if (currPlatform.GetComponent<PlatformNeighbors>().up != null)
                    {
                        MoveUp();
                    }
                    else
                    {
                        play_err();
                    }
                }

                else if (touchpad.y < -0.7f)
                {
                    if (currPlatform.GetComponent<PlatformNeighbors>().down != null)
                    {
                        MoveDown();
                    }
                    else
                    {
                        play_err();
                    }
                }

                if (touchpad.x > 0.7f)
                {
                    if (currPlatform.GetComponent<PlatformNeighbors>().right != null)
                    {
                        MoveRight();
                    }
                    else
                    {
                        play_err();
                    }

                }

                else if (touchpad.x < -0.7f)
                {
                    if (currPlatform.GetComponent<PlatformNeighbors>().left != null)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        play_err();
                    }
                }
            }
        }
        else if (device2 != null && device2.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (!canMove)
            {
                play_err();
            }
            else
            {
                Vector2 touchpad = (device2.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

                if (touchpad.y > 0.7f)
                {
                    if (currPlatform.GetComponent<PlatformNeighbors>().up != null)
                    {
                        MoveUp();
                    }
                    else
                    {
                        play_err();
                    }
                }

                else if (touchpad.y < -0.7f)
                {
                    if (currPlatform.GetComponent<PlatformNeighbors>().down != null)
                    {
                        MoveDown();
                    }
                    else
                    {
                        play_err();
                    }
                }

                if (touchpad.x > 0.7f)
                {
                    if (currPlatform.GetComponent<PlatformNeighbors>().right != null)
                    {
                        MoveRight();
                    }
                    else
                    {
                        play_err();
                    }

                }

                else if (touchpad.x < -0.7f)
                {
                    if (currPlatform.GetComponent<PlatformNeighbors>().left != null)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        play_err();
                    }
                }
            }
        }*/
    }

    public void SetPlatform(Transform platform)
    {
        currPlatform = platform;
        Vector3 newTrans = platform.position;
        newTrans.x -= camObj.transform.localPosition.x;
        newTrans.z -= camObj.transform.localPosition.z;
        transform.position = newTrans;
        targetPos = newTrans;
    }
    public void SetAvatar(GameObject _avatar)
    {
        avatar = _avatar;
    }
    private void MoveUp(GameObject np)
    {
        
        Transform newplatform = np.transform;
        setNewPos(newplatform);
    }
    private void MoveDown(GameObject np)
    {
        Transform newplatform = np.transform;
        setNewPos(newplatform);
    }
    private void MoveLeft(GameObject np)
    {
        Transform newplatform = np.transform;
        setNewPos(newplatform);
    }
    private void MoveRight(GameObject np)
    {
        Transform newplatform = np.transform;
        setNewPos(newplatform);
    }
    IEnumerator coolDown()
    {
        canMove = false;
        yield return new WaitForSeconds(CD);
        canMove = true;
    }
    void setNewPos(Transform newplatform)
    {
        //StartCoroutine(coolDown());
        Vector3 newTrans = newplatform.position;
        newTrans.x -= camObj.transform.localPosition.x;
        newTrans.z -= camObj.transform.localPosition.z;
        //transform.position = newTrans;
        currPlatform = newplatform;
        targetPos = newTrans;
    }
    void play_err()
    {
        audS.PlayOneShot(cd_Sound);
    }
}
