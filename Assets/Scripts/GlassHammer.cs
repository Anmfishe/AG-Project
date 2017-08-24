using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassHammer : MonoBehaviour
{

    public GameObject hitSpark;
    public Transform wand;
    private bool blue;
    public float destroyTime = 15;
    private float startTime;
    public float damage = 20;
    private Rigidbody rb;


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
                if (other.GetComponent<ITeamOwned>().GetBlue() != blue)
                {
                    other.gameObject.GetPhotonView().RPC("DestroyShield", PhotonTargets.AllBuffered);
                    other.GetComponent<ITeamOwned>().owner.GetComponentInChildren<PlayerStatus>().TakeDamage(damage);
                    PhotonNetwork.Instantiate(hitSpark.name, other.transform.position, new Quaternion(), 0);
                    PhotonNetwork.Destroy(GetComponent<PhotonView>());
                }
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            if (GetComponent<PhotonView>().isMine)
            {
                other.GetComponent<PlayerStatus>().TakeDamage(damage);
                PhotonNetwork.Instantiate(hitSpark.name, other.transform.position, new Quaternion(), 0);
                PhotonNetwork.Destroy(GetComponent<PhotonView>());
            }
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
