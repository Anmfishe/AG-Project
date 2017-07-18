using UnityEngine;
using System.Collections;

public class Holdable : MonoBehaviour
{

    public int force = 750;

    public bool thrown = false;
    public bool held = false;
    public bool warped = false;
    public bool deactivated = false;
    public bool holdable = true;
    public bool memory = false;
    public bool resettable = false;
    public bool countDown = false;
    public bool keyCard = false;

    public AudioSource audioSource;
    private AudioClip bounceSound;

    public float padding = 50.0f;

    //public guardAI guard1;

    public int index;
    //float[] heights = { 0, 4 };
  //  GameObject viveCam;
    //public LevelStates levelStates;
    float startTime;
    float warpTimeBase;
    float warpTimeCap = 5.0f;
    float maxDist = 0.5f;

    Rigidbody rigidbody;

    public bool fixedPos = false;
    Vector3 myStartPos;
    Vector3 startPos;
    Vector3 startRot;
    Vector3 thisStartPos;
    Transform landingPoint;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        bounceSound = Resources.Load<AudioClip>("Rocks/Splash");


    rigidbody = GetComponent<Rigidbody>();
       // viveCam = GameObject.FindGameObjectWithTag("Vive");
        //viveCam.SetActive(false);
        //heights[0] = viveCam.transform.position.y;
        //levelStates = viveCam.GetComponent<LevelStates>();
        startTime = Time.time;
        if (memory == true)
        {
            resettable = true;
        }
        myStartPos = transform.position;
        startRot = transform.rotation.eulerAngles;

    }

    // Update is called once per frame
    void Update()
    {
        if (held == true)// || Vector3.Distance(myStartPos, transform.position) < maxDist)
        {
            warpTimeBase = 0.0f;
            countDown = false;
        }

        else
        {
            if (countDown == false)
            {
                countDown = true;
                warpTimeBase = Time.time;
            }
            if (Time.time - warpTimeBase > warpTimeCap && resettable == true)
            {
                transform.position = myStartPos;
                //transform.rotation.eulerAngle = startRot;
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }
        }

        if (held == true && warped == false && memory == true)
        {
           // startPos = viveCam.transform.position;
           // startRot = viveCam.transform.localEulerAngles;

            if (fixedPos == false)
            {
           //     levelStates.levelStates[index].SetActive(true);
                //viveCam.transform.position = new Vector3(viveCam.transform.position.x, viveCam.transform.position.y + levelStates.levelStates[index].transform.position.y, viveCam.transform.position.z);
            //    levelStates.levelStates[0].SetActive(false);
                //print(levelStates.levelStates[index].active);
                warped = true;
            }

            else
            {
                //foreach (Transform child in levelStates.levelStates[index].transform)
                //    if (child.CompareTag("Landin"))
                //    {
                //        print("gotcha");
                //        if (name == "1_Flashlight")
                //        {
                //            print("here");
                //            transform.GetChild(0).gameObject.SetActive(true);
                //        }

                //        landingPoint = child.transform;

                //        levelStates.levelStates[index].SetActive(true);
                //        viveCam.transform.position = new Vector3(landingPoint.position.x, landingPoint.position.y, landingPoint.position.z);
                //        levelStates.levelStates[0].SetActive(false);
                //        //print(levelStates.levelStates[index].active);
                //        warped = true;
                //    }
            }
        }

        if (deactivated == false && Time.time - startTime > .5 && memory == true)
        {
            int i = 0;
            //foreach (GameObject state in levelStates.levelStates)
            //{
            //  //  levelStates.levelStates[i].SetActive(false);
            //   // levelStates.levelStates[0].SetActive(true);
            //    ++i;
            //}
           // viveCam.SetActive(true);
            deactivated = true;
        }
 
   }
    void OnTriggerEnter(Collider collision)
    {
        //thrown = true;
        if (thrown == true)
        {
           // if rest

            //print("HITTTTTT");
            if (collision.transform.CompareTag("Water"))
            {
                //print("bounce");
                //  print(rigidbody.velocity);
                //rigidbody.AddForce(Vector3.up * force);



                // Bouncy shit here
                float modifier = Random.Range(-0.3f, 0.3f);
                audioSource.pitch = 1 + modifier;
                audioSource.PlayOneShot(bounceSound,1.0f);
                print("played audio");

                print(rigidbody.velocity + " | " + rigidbody.velocity.magnitude);
                float angle = Mathf.Asin(rigidbody.velocity.y / rigidbody.velocity.magnitude) / 3.14f * 180f;
                angle = -angle;
                float goldenAngle = 20;
                print("Angle: " + angle + 
                    "\nGolden Angle: " + goldenAngle +
                    "\nLow Limit: " + (goldenAngle - padding) +
                    "\nHigh Limit: " + (goldenAngle + padding));
                if ((angle > (goldenAngle - padding)) && (angle < (goldenAngle + padding)))
                {

                    /*
                    if (angle < 20)
                    {
                        rigidbody.AddForce(Vector3.up * force);
                    }

                    else
                    {
                    }*/
                    rigidbody.velocity = new Vector3(rigidbody.velocity.x, -rigidbody.velocity.y * .75f, rigidbody.velocity.z);   
                // rigidbody.AddForce(Vector3.up * force);
                }


                // rigidbody.AddForce(Vector3.up * force);







            }
            else
            {
                print(collision.gameObject.tag);
            }
        }
    }
}