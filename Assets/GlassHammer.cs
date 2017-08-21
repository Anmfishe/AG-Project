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


    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
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
            Vector3 direction = wand.position + wand.forward * 5;
            direction -= this.transform.position;
            this.transform.position += direction.normalized * Time.deltaTime;
            this.transform.rotation = wand.rotation;

            if ((Time.time - startTime) > destroyTime)
                PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (GetComponent<PhotonView>().isMine)
            {
                if (other.transform.parent.GetComponent<TeamManager>().blue != blue)
                {
                    other.gameObject.GetPhotonView().RPC("TakeDamage", PhotonTargets.AllBuffered, damage);
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
    }

    public void SetBlue(bool blue_)
    {
        blue = blue_;
    }
}
