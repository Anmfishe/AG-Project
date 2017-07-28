using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : MonoBehaviour
{

	private Edwon.VR.VRGestureRig spellcastingGesture;
	private PhotonView photonView;
	private LineRenderer line;
	public GameObject reticle;
	GameObject reticleInstance;
	private Vector3 steeringDirection;
	private Vector3[] points = new Vector3[2];
	public bool mine = false;
	public GameObject explosion;


	public LayerMask targettable;
	public GameObject wand;
	private int damage = 40;
	private float castDist = 10;
	private float magCap = 10;
	private float skyCap = 10;
	private float groundCap = 40;
	public float duration = 12f;
	public Vector3 direction; //Sets the direction to where the fireball is traveling to.
	public float speed = 10f;
	public float startup = 0.25f;
	public float minLinearVelocity = 5f;
	public float minAngularVelocity = 20f;
	private float steeringForceSky = 10000;
	private float steeringForceGround = 15000;
	public Rigidbody rb;

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
		photonView = GetComponent<PhotonView> ();
		if (photonView.isMine) 
		{
			reticleInstance = Instantiate (reticle, transform.position, transform.rotation) as GameObject;
			mine = true;
			wand = GameObject.Find ("Controller (right)");
			spellcastingGesture = Camera.main.transform.parent.GetComponent<Edwon.VR.VRGestureRig> ();
			spellcastingGesture.enabled = false;
			rb = GetComponent<Rigidbody> ();
			rb.AddForce (transform.forward * 1000);
			line = GetComponent<LineRenderer> ();


		}
		//Destroy(this.gameObject, duration);
		if (startup > 0)
			activeTimer = startup;
		if (fbCollider == null)
			fbCollider = this.GetComponent<SphereCollider> ();
		if (audioSource == null)
			audioSource = this.GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update()
	{
		if (mine == true) {

		}

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
		// If this fireball belongs to me
		if (mine == true)
		{
			points [0] = transform.position;
			points [1] = wand.transform.position + wand.transform.forward * .15f;
			line.SetPositions (points);

			Vector3 fwd = wand.transform.TransformDirection(Vector3.forward);
			RaycastHit hit;

			//Disable back faces so it doesn't collide with itself.
			//Physics.queriesHitBackfaces = false;

			//Debug.DrawRay(wand.transform.position, wand.transform.forward * 1000, Color.red, 0.01f);
			//Get raycast results.

			if (Physics.Raycast (wand.transform.position, wand.transform.forward, out hit, 1000,targettable) && Vector3.Distance(wand.transform.position, hit.point) < 40) {

				print(Vector3.Distance(wand.transform.position, hit.point));
				if (hit.transform.gameObject.tag == ("Spell")) 
				{
					return;
				}
				Quaternion rotation = Quaternion.LookRotation (hit.point - transform.position);
				steeringDirection = hit.point - transform.position;
				magCap = groundCap;
				rb.AddForce (steeringDirection.normalized * steeringForceGround * Time.smoothDeltaTime);
				reticleInstance.transform.position = hit.point;
			}
			else
			{
				steeringDirection = (wand.transform.position + (wand.transform.forward * 20 - transform.position));
				magCap = skyCap;
				rb.AddForce (steeringDirection.normalized * steeringForceSky * Time.smoothDeltaTime);
				reticleInstance.transform.position = wand.transform.position + (wand.transform.forward * 20);
			}
			rb.velocity = clampVector (rb.velocity);

			Debug.DrawRay (wand.transform.position, fwd * 1000, Color.green);
		}
	}

	Vector3 clampVector(Vector3 myVector)
	{
		if (myVector.magnitude > magCap) 
		{
			float multiplier = magCap / myVector.magnitude;
			myVector.x = myVector.x * multiplier;
			myVector.y = myVector.y * multiplier;
			myVector.z = myVector.z * multiplier;
		}

		return myVector;
	}

	private void OnCollisionEnter(Collision collision)
	{
		GameObject other = collision.gameObject;
		print("Collided by " + other.name);

		if (other.CompareTag ("Player")) {
			print ("hit the body");
			//Apply damage to object if it has the Player tag and implements the PlayerStatus script.
			PlayerStatus statusScript = other.GetComponent<PlayerStatus> ();
			if (statusScript != null)
				statusScript.takeDamage (damage);
			//Instantiate new explosion.
			GameObject newExplosion = PhotonNetwork.Instantiate (explosion.name, this.transform.position, new Quaternion (), 0);

			DestroyFireball ();

		} else if (other.CompareTag ("put")) {
			print ("hit on head");
			//Apply damage to object if it has the Player tag and implements the PlayerStatus script.
			PlayerStatus statusScript = other.transform.parent.GetComponentInChildren<PlayerStatus> ();
			if (statusScript != null)
				statusScript.takeDamage (damage);
			//Instantiate new explosion.
			GameObject newExplosion = PhotonNetwork.Instantiate (explosion.name, this.transform.position, new Quaternion (), 0);

			DestroyFireball ();
		} else if (other.CompareTag ("Shield")) {
			print ("hit on shield");
			//Apply damage to the shield.
			Damageable damageScript = other.GetComponent<Damageable> ();
			if (damageScript != null)
				damageScript.TakeDamage (damage);
			//Instantiate new explosion.
			GameObject newExplosion = PhotonNetwork.Instantiate (explosion.name, this.transform.position, new Quaternion (), 0);

			DestroyFireball ();
		} else if (other.CompareTag ("Spell")) {
			print ("hit on spell");
			//Get the point between the two fireballs.
			//Vector3 midpoint = this.transform.position + ((other.transform.position - this.transform.position) * 0.5f);

			//Instantiate new explosion.
			GameObject newExplosion = PhotonNetwork.Instantiate (explosion.name, this.transform.position, new Quaternion (), 0);

			//Delete this game object.
			DestroyFireball ();

		} 
		else if (other.CompareTag ("BluePlatform") || other.CompareTag ("RedPlatform") || other.CompareTag ("Stage")) 
		{
			//Instantiate new explosion.
			GameObject newExplosion = PhotonNetwork.Instantiate (explosion.name, this.transform.position, new Quaternion (), 0);

			//Delete this game object.
			DestroyFireball ();

		}
	}

	private void OnCollisionStay(Collision collision)
	{
		GameObject other = collision.gameObject;
		if (other.CompareTag ("BluePlatform") || other.CompareTag ("RedPlatform") || other.CompareTag ("Stage")) 
		{
			//Instantiate new explosion.
			GameObject newExplosion = PhotonNetwork.Instantiate (explosion.name, this.transform.position, new Quaternion (), 0);

			//Delete this game object.
			DestroyFireball ();

		}
	}


	private void OnTriggerEnter(Collider collider)
	{
		GameObject other = collider.gameObject;
		print("Triggered (heh) by " + other.name);
		if (other.CompareTag("SpellHitter"))
		{
			print("triggered spellhitter");
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
		//        PhotonNetwork.Destroy(this.gameObject);

		Collider[] hits;
		hits = Physics.OverlapSphere(transform.position, 5);
		foreach (Collider hit in hits)
		{
			if (hit.transform.tag == "Player")
			{
				hit.transform.GetComponent<PlayerStatus>().takeDamage(damage);
			}
		}



		Destroy(reticleInstance);
		PhotonNetwork.Destroy(this.GetComponent<PhotonView>());
	}
}