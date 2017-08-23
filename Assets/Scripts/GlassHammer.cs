using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassHammer : MonoBehaviour
{

    public GameObject hitSpark;
    public Transform wand;
    private bool blue;
    private bool isDecaying = false;
    public float duration = 1;
    public float destroyTime = 15;
    public float hitBonusTime = 0.25f;
    private float durationTimer = 0;
    private float startTime;
    public float damage = 50;
    private Rigidbody rb;
    public float steeringForce = 1f;


    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        rb = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<PhotonView>().isMine)
        {
            if (wand == null)
            {
                return;
            }
            if ((Time.time - startTime) > destroyTime)
                PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }
    private void FixedUpdate()
    {
        if (this.GetComponent<PhotonView>().isMine)
        {
            if (wand == null)
            {
                return;
            }


            Vector3 direction = wand.position + wand.forward;
            direction -= this.transform.position;

            //rb.AddForce(direction.normalized * Time.smoothDeltaTime * steeringForce);

            //this.transform.position += direction.normalized * Time.deltaTime;
            this.transform.position = wand.position;
            this.transform.rotation = wand.rotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shield"))
        {
            if (GetComponent<PhotonView>().isMine)
            {
                if (other.transform.parent.GetComponent<TeamManager>().blue != blue)
                {
                    other.gameObject.GetPhotonView().RPC("DestroyShield", PhotonTargets.AllBuffered);
                    PhotonNetwork.Instantiate(hitSpark.name, other.transform.position, new Quaternion(), 0);
                    PhotonNetwork.Destroy(GetComponent<PhotonView>());

                }
            }

            print("my blue: " + blue + " | their blue: " + other.transform.parent.GetComponent<TeamManager>().blue);
        }
    }

    public void SetWand(Transform wand_) 
    {
        wand = wand_;
        this.transform.position = wand.position;
        this.transform.rotation = wand.rotation;
    }

    public void SetBlue(bool blue_)
    {
        blue = blue_;
    }
}
