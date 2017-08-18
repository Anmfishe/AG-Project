using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballNew : MonoBehaviour
{
    public GameObject explosion;
    public float damage = 20;
    public float duration = 12f;
    public Vector3 direction; //Sets the direction to where the fireball is traveling to.
    public float speed = 10f;
    public float startup = 0.25f;
    public float minLinearVelocity = 5f;
    public float minAngularVelocity = 20f;
    public AudioSource audioSource;
    public AudioClip deflectAudio;
    [HideInInspector]
    public bool isMaster;
    [HideInInspector]
    public bool isSlave;

//    [SerializeField]
    private float activeTimer = 0;
//    [SerializeField]
    private SphereCollider fbCollider;
    // Use this for initialization
    void Start()
    {
        //Destroy(this.gameObject, duration);
        if (startup > 0) activeTimer = startup;
        if(fbCollider == null) fbCollider = this.GetComponent<SphereCollider>();
        if (audioSource == null) audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //Timer to activate collider.
        if (activeTimer > 0)
            activeTimer -= Time.deltaTime;
        else if (!fbCollider.enabled)
        {
            fbCollider.enabled = true;
            print("fbcollider enabled");
        }

        //this.GetComponent<Rigidbody>().AddForce(this.transform.forward * speed * Time.deltaTime, ForceMode);
        //this.GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
    }
    private void FixedUpdate()
    {
        this.transform.Translate(this.transform.forward * speed * Time.deltaTime, Space.World);
    }

    
     private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        print("Collided by " + other.name);

        if (! this.GetComponent<PhotonView>().isMine)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            print("hit the body");
            //Apply damage to object if it has the Player tag and implements the PlayerStatus script.
            //            PlayerStatus statusScript = other.GetComponent<PlayerStatus>();
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv != null)
            {
                pv.RPC("TakeDamage", PhotonTargets.All, damage);
            }
            else
            {
                print("pv is null");
            }
            //Instantiate new explosion.
            GameObject newExplosion = PhotonNetwork.Instantiate(explosion.name, this.transform.position, new Quaternion(), 0);

            //Delete this game object.
            DestroyFireball();
        }
        else if(other.CompareTag("put"))
        {
            print("hit on head");
            //Apply damage to object if it has the Player tag and implements the PlayerStatus script.
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv != null)
            {
                pv.RPC("TakeDamage", PhotonTargets.All, damage);
            }
            else
            {
                print("pv is null");
            }
            //Instantiate new explosion.
            GameObject newExplosion = PhotonNetwork.Instantiate(explosion.name, this.transform.position, new Quaternion(), 0);

            //Delete this game object.
            DestroyFireball();
        }
        else if (other.CompareTag("Shield"))
        {
            print("hit on shield");
            //Apply damage to the shield.
            Damageable damageScript = other.GetComponent<Damageable>();
            if (damageScript != null) damageScript.TakeDamage(damage);
            //Instantiate new explosion.
            GameObject newExplosion = PhotonNetwork.Instantiate(explosion.name, this.transform.position, new Quaternion(), 0);

            //Delete this game object.
            DestroyFireball();
        }
        else if (other.CompareTag("Spell"))
        {
            print("hit on spell");
            //Get the point between the two fireballs.
            //Vector3 midpoint = this.transform.position + ((other.transform.position - this.transform.position) * 0.5f);

            //Instantiate new explosion.
            GameObject newExplosion = PhotonNetwork.Instantiate(explosion.name, this.transform.position, new Quaternion(), 0);

            //Delete this game object.
            DestroyFireball();
        }
    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    GameObject other = collider.gameObject;
    //    print("Triggered (heh) by " + other.name);
    //    if (other.CompareTag("SpellHitter"))
    //    {
    //        print("triggered spellhitter");
    //        //Create reflected fireball if it was hit hard enough by the spell hitter.
    //        Rigidbody otherBody = other.GetComponent<Rigidbody>();

    //        print("VELOCITY: " + otherBody.velocity.magnitude + " | ANGULAR V: " + otherBody.angularVelocity.magnitude);
    //        if (otherBody.velocity.magnitude > minLinearVelocity || otherBody.angularVelocity.magnitude > minAngularVelocity)
    //        {
    //            print("Invert Fireball!");
    //            //this.transform.rotation = Quaternion.LookRotation(this.transform.forward * -1, this.transform.up * -1);
    //            //this.GetComponent<Rigidbody>().velocity *= -1;
    //            //StartRecovery();
    //            //GameObject reflectedFireball = PhotonNetwork.Instantiate("Fireball", this.transform.position, Quaternion.LookRotation(otherBody.transform.forward, otherBody.transform.up), 0);
    //            fbCollider.enabled = false;
    //            GameObject reflectedFireball = PhotonNetwork.Instantiate("Fireball", this.transform.position + this.transform.forward * -1, Quaternion.LookRotation(this.transform.forward * -1, this.transform.up * -1), 0);
    //            DestroyFireball();
    //            if (deflectAudio != null) audioSource.PlayOneShot(deflectAudio);
    //        }

    //    }
    //    else if (other.CompareTag("Player"))
    //    {
    //        if (PhotonNetwork.isMasterClient)
    //        {
    //            print("on trigger enter, hit torso");

    //            other.GetPhotonView().RPC("TakeDamage", PhotonTargets.AllBuffered, damage);
                
    //            //Instantiate new explosion. May not be properly destroyed on the network
    //            GameObject newExplosion = PhotonNetwork.Instantiate(explosion.name, this.transform.position, new Quaternion(), 0);
    //            DestroyFireball();
    //        }
    //    }
    //}

    private void StartRecovery()
    {
        activeTimer = startup;
        fbCollider.enabled = false;
    }

    private void DestroyFireball()
    {
        Debug.Log("Attempting to destory fireball");
        //Destroy game object.
        PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
//        Destroy(this);
    }

}
