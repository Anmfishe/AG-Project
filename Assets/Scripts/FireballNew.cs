using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballNew : MonoBehaviour
{
    public GameObject explosion;
    public int damage = 20;
    public float duration = 12f;
    public Vector3 direction; //Sets the direction to where the fireball is traveling to.
    public float speed = 10f;
    public float startup = 0.125f;
    public float minLinearVelocity = 5f;
    public float minAngularVelocity = 20f;
    public AudioSource audioSource;
    public AudioClip deflectAudio;
    [HideInInspector]
    public bool isMaster;
    [HideInInspector]
    public bool isSlave;

    [SerializeField]
    private float activeTimer = 0;
    [SerializeField]
    private SphereCollider fbCollider;
    // Use this for initialization
    void Start()
    {
        //Destroy(this.gameObject, duration);
        if (startup > 0) activeTimer = startup;
        if(fbCollider == null) this.GetComponent<SphereCollider>();
        if (audioSource == null) this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //Timer to activate collider.
        if (activeTimer > 0)
            activeTimer -= Time.deltaTime;
        else if (!fbCollider.enabled)
            fbCollider.enabled = true;

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
        print("Collided by " + other.tag);

        if (other.CompareTag("Player"))
        {
            //Apply damage to object if it has the Player tag and implements the PlayerStatus script.
            PlayerStatus statusScript = other.GetComponent<PlayerStatus>();
            if (statusScript != null) statusScript.takeDamage(damage);
            //Instantiate new explosion.
            GameObject newExplosion = PhotonNetwork.Instantiate(explosion.name, this.transform.position, new Quaternion(), 0);

            DestroyFireball();

        }
        else if (other.CompareTag("Shield"))
        {
            //Apply damage to the shield.
            Damageable damageScript = other.GetComponent<Damageable>();
            if (damageScript != null) damageScript.TakeDamage(damage);
            //Instantiate new explosion.
            GameObject newExplosion = PhotonNetwork.Instantiate(explosion.name, this.transform.position, new Quaternion(), 0);

            DestroyFireball();
        }
        else if (other.CompareTag("Spell"))
        {
            //Get the point between the two fireballs.
            //Vector3 midpoint = this.transform.position + ((other.transform.position - this.transform.position) * 0.5f);

            //Instantiate new explosion.
            GameObject newExplosion = PhotonNetwork.Instantiate(explosion.name, this.transform.position, new Quaternion(), 0);

            //Delete this game object.
            DestroyFireball();

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        print("Triggered (heh) by " + other.tag);
        if (other.CompareTag("SpellHitter"))
        {
            //Create reflected fireball if it was hit hard enough by the spell hitter.
            Rigidbody otherBody = other.GetComponent<Rigidbody>();

            print("VELOCITY: " + otherBody.velocity.magnitude + " | ANGULAR V: " + otherBody.angularVelocity.magnitude);
            if (otherBody.velocity.magnitude > minLinearVelocity || otherBody.angularVelocity.magnitude > minAngularVelocity)
            {
                print("Invert Fireball!");
                //this.transform.rotation = Quaternion.LookRotation(this.transform.forward * -1, this.transform.up * -1);
                //this.GetComponent<Rigidbody>().velocity *= -1;
                //StartRecovery();
                //GameObject reflectedFireball = PhotonNetwork.Instantiate("Fireball", this.transform.position, Quaternion.LookRotation(otherBody.transform.forward, otherBody.transform.up), 0);
                fbCollider.enabled = false;
                GameObject reflectedFireball = PhotonNetwork.Instantiate("Fireball", this.transform.position + this.transform.forward * -1, Quaternion.LookRotation(this.transform.forward * -1, this.transform.up * -1), 0);
                DestroyFireball();
                if (deflectAudio != null) audioSource.PlayOneShot(deflectAudio);
            }
            else
            {
                //Hurt asshole player.
                this.transform.parent.GetComponentInChildren<PlayerStatus>().takeDamage(damage);
            }
        }

    }
    private void StartRecovery()
    {
        activeTimer = startup;
        fbCollider.enabled = false;
    }
    private void DestroyFireball()
    {
        //Destroy game object.
        PhotonNetwork.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }

}
