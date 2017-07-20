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
    public bool countDown = false;
    public bool keyCard = false;

    public AudioSource audioSource;
    private AudioClip bounceSound;

    public float padding = 50.0f;

    public int index;
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

        startTime = Time.time;
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
           
        }

        if (held == true && warped == false && memory == true)
        {

            if (fixedPos == false)
            {
    
                warped = true;
            }

            else
            {
              
            }
        }

        if (deactivated == false && Time.time - startTime > .5 && memory == true)
        {
            int i = 0;
            deactivated = true;
        }
 
   }
    void OnTriggerEnter(Collider collision)
    {
        //thrown = true;
        if (thrown == true)
        {
            if (collision.transform.CompareTag("Water"))
            {
             
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
                    rigidbody.velocity = new Vector3(rigidbody.velocity.x, -rigidbody.velocity.y * .75f, rigidbody.velocity.z);   
                }

            }
            else
            {
                print(collision.gameObject.tag);
            }
        }
    }
}