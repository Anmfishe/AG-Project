using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR;
using Edwon.VR.Gesture;

public class SpellcastingGestureRecognition : MonoBehaviour {

    public GameObject fireball;
    public Gradient fireballGradient;
    public float fireballCooldown = 2f;

    public GameObject shield;
    public Gradient shieldGradient;
    public float shieldCooldown = 6f;

    public GameObject heal;
    public GameObject swipeLeft;
    public GameObject swipeRight;
    public AudioClip cast_success;
    public AudioClip cast_failure;
    public Transform wand;
    public Transform book;
    [HideInInspector]
    public Targeting target;
    public Transform avatar;

    //public AudioClip spell_deflected;
    //--->Private Vars<---//
    Camera mainCam;
    float triggerL;
    float triggerR;
    SpellLogic spellLogic;
    bool triggerUsed = false;
    private AudioSource audioSource;

    public bool hasSpell;
    public GameObject currentSpell;
    public string currentSpellName;
    public Gradient currentSpellGradient;
    public float spellCooldown = 3f;
    private float spellTimer = 0;
    private bool isCoolingDown = false;

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        spellLogic = GetComponent<SpellLogic>();
        spellLogic.mainCam = mainCam;
        audioSource = GetComponent<AudioSource>();
        target = GetComponentInChildren<Targeting>();
    }

    void OnEnable()
    {
        GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
        GestureRecognizer.GestureRejectedEvent += OnGestureRejected;
    }

    void OnDisable()
    {
        GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
        GestureRecognizer.GestureRejectedEvent -= OnGestureRejected;
    }


    private void Update()
    {
        //Check if we're cooling down.
        if (isCoolingDown)
        {
            //If fireball timer is still active.
            if (spellTimer > 0)
            {
                spellTimer -= Time.deltaTime;
            }
            else
            {
                isCoolingDown = false;
                GetComponent<VRGestureRig>().enabled = true;
                if (wand != null)
                {
                    wand.Find("tip").Find("spark").GetComponent<ParticleSystem>().Play();
                    audioSource.PlayOneShot(cast_success);

                    //wand.Find("tip").Find("spark").Find("dust").GetComponent<ParticleSystem>().Play();
                }

            }
        }
        if (hasSpell && Input.GetKeyDown("joystick button 15"))
        {
            CastSpell();
        }
    }

    void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble)
    {
        //string confidenceString = confidence.ToString().Substring(0, 4);
        //Debug.Log("detected gesture: " + gestureName + " with confidence: " + confidenceString);

        switch (gestureName)
        {
            case "Fire":
                currentSpell = fireball;
                currentSpellName = "fire";
                currentSpellGradient = fireballGradient;
                hasSpell = true;
                {
                    ParticleSystem wandParticle = wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>();
                    wandParticle.Stop();
                    var wandParticleModule = wandParticle.colorOverLifetime;
                    wandParticleModule.color = currentSpellGradient;
                    wandParticle.Play();
                }
                GetComponent<VRGestureRig>().enabled = false;
                //Transform t = null;
                //t.position = mainCam.transform.position;
                //t.LookAt(target.target);
                //GameObject fb = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0,.3f, 0),mainCam.transform.rotation, 0);

                // GameObject fb = Instantiate(fireball, mainCam.transform.position, mainCam.transform.rotation);
                audioSource.PlayOneShot(cast_success);
                break;
            case "Shield":
                currentSpell = shield;
                currentSpellName = "shield";
                currentSpellGradient = shieldGradient;
                hasSpell = true;

                //Check if wand exists.
                if (wand != null)
                {
                    //Update flame sparks with correct colors and play.
                    ParticleSystem wandParticle = wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>();
                    wandParticle.Stop();
                    var wandParticleModule = wandParticle.colorOverLifetime;
                    wandParticleModule.color = currentSpellGradient;
                    wandParticle.Play();
                }
                GetComponent<VRGestureRig>().enabled = false;
                /*
                if (target!=null && target.target != null)
                {
                    Transform t = mainCam.transform;
                    t.position = mainCam.transform.position;
                    t.LookAt(target.target.position + new Vector3(0, 0.5f, 0));
                    GameObject fb2 = PhotonNetwork.Instantiate(shield.name, wand.Find("tip").position, wand.Find("tip").rotation, 0);
                }
                else
                {
                    Transform t = mainCam.transform;
                    GameObject fb3 = PhotonNetwork.Instantiate(shield.name, wand.Find("tip").position, wand.Find("tip").rotation, 0);
                }
                */
                audioSource.PlayOneShot(cast_success);
                break;
            case "Heal":
                currentSpellName = "heal";
                break;
            case "SwipeLeft":
                //   GameObject fb2 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), mainCam.transform.rotation, 0);
                spellLogic.Deflect();
                //audioSource.PlayOneShot(cast_success);
                break;
            case "SwipeRight":
                if (target != null && target.target != null)
                {
                    Transform t = mainCam.transform;
                t.position = mainCam.transform.position;
                t.LookAt(target.target.position + new Vector3(0, 0.5f, 0));
                GameObject fb3 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), t.rotation, 0);
                }
                else
                {
                    Transform t = mainCam.transform;
                    GameObject fb3 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), t.rotation, 0);
                }
                audioSource.PlayOneShot(cast_success);
                //spellLogic.Deflect();
                //GameObject fb3 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), mainCam.transform.rotation, 0);
                break;
        }
    }

    void OnGestureRejected(string error, string gestureName = null, double confidenceValue = 0)
    {
        audioSource.PlayOneShot(cast_failure);
    }

    public void SetAvatar(Transform _avatar)
    {
        avatar = _avatar;
        wand = avatar.Find("Right Hand").Find("MagicWand");
        book = avatar.Find("Left Hand").Find("SpellBook");
    }

    private void CastSpell()
    {
        GameObject spellInstance;
        Transform wandTip = wand.Find("tip");

        switch (currentSpellName)
        {
            case "fire":
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position, wandTip.rotation, 0);
                spellTimer = fireballCooldown;
                break;
            case "shield":
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position + wandTip.forward, Camera.main.transform.rotation, 0);
                //spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position + wandTip.forward, wandTip.rotation, 0);
                //spellInstance.transform.SetParent(wandTip);
                spellTimer = shieldCooldown;
                break;
            case "heal":
                break;
            default:
                spellTimer = spellCooldown;
                break;
        }

        if (wand != null)
        {
            wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>().Stop();
            ParticleSystem ps = wand.Find("tip").Find("smoke").GetComponent<ParticleSystem>();
            var main = ps.main;
            ps.Stop();
            main.duration = spellTimer;
            ps.Play();
        }

        isCoolingDown = true;
        hasSpell = false;
        currentSpell = null;
        currentSpellName = "";
    }
}


