using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    [HideInInspector]
    public Transform currPlatform;
    [HideInInspector]
    public GameObject avatar;
    public bool useRight = true;
    public bool useLeft = true;
    public float CD = 1;
    public AudioClip cd_Sound;
    private bool canMove = true;
    private Transform targetPos;
    private AudioSource audS;
    private GameObject camObj;
    SteamVR_TrackedObject trackedObj1;
    SteamVR_TrackedObject trackedObj2;
    SteamVR_Controller.Device device1;
    SteamVR_Controller.Device device2;
    // Use this for initialization
    private void Awake()
    {
        trackedObj1 = transform.Find("Controller (left)").GetComponent<SteamVR_TrackedObject>();
        trackedObj2 = transform.Find("Controller (right)").GetComponent<SteamVR_TrackedObject>();
        camObj = GetComponentInChildren<Camera>().gameObject;
    }
    void Start () {
        audS = GetComponent<AudioSource>();
	}
    void FixedUpdate()
    {
        device1 = SteamVR_Controller.Input((int)trackedObj1.index);
        device2 = SteamVR_Controller.Input((int)trackedObj2.index);
    }


    // Update is called once per frame
    void Update()
    {
		

        if ((device1 != null && device1.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)))
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
        }
    }

    public void SetPlatform(Transform platform)
    {
        currPlatform = platform;
        Vector3 newTrans = platform.position;
        newTrans.x -= camObj.transform.localPosition.x;
        newTrans.z -= camObj.transform.localPosition.z;
        transform.position = newTrans;
    }
    public void SetAvatar(GameObject _avatar)
    {
        avatar = _avatar;
    }
    private void MoveUp()
    {
        StartCoroutine(coolDown());
        Transform newplatform = currPlatform.GetComponent<PlatformNeighbors>().up;
        Vector3 newTrans = newplatform.position;
        newTrans.x -= camObj.transform.localPosition.x;
        newTrans.z -= camObj.transform.localPosition.z;
        transform.position = newTrans;
        currPlatform = newplatform;
    }
    private void MoveDown()
    {
        StartCoroutine(coolDown());
        Transform newplatform = currPlatform.GetComponent<PlatformNeighbors>().down;
        Vector3 newTrans = newplatform.position;
        newTrans.x -= camObj.transform.localPosition.x;
        newTrans.z -= camObj.transform.localPosition.z;
        transform.position = newTrans;
        currPlatform = newplatform;
    }
    private void MoveLeft()
    {
        StartCoroutine(coolDown());
        Transform newplatform = currPlatform.GetComponent<PlatformNeighbors>().left;
        Vector3 newTrans = newplatform.position;
        newTrans.x -= camObj.transform.localPosition.x;
        newTrans.z -= camObj.transform.localPosition.z;
        transform.position = newTrans;
        currPlatform = newplatform;
    }
    private void MoveRight()
    {
        StartCoroutine(coolDown());
        Transform newplatform = currPlatform.GetComponent<PlatformNeighbors>().right;
        Vector3 newTrans = newplatform.position;
        newTrans.x -= camObj.transform.localPosition.x;
        newTrans.z -= camObj.transform.localPosition.z;
        transform.position = newTrans;
        currPlatform = newplatform;
    }
    IEnumerator coolDown()
    {
        canMove = false;
        yield return new WaitForSeconds(CD);
        canMove = true;
    }
    void play_err()
    {
        audS.PlayOneShot(cd_Sound);
    }
}
