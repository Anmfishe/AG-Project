using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR;
using Edwon.VR.Gesture;

public class SpellcastingGestureRecognition : MonoBehaviour {

    public Gradient baseGradient;

    public GameObject fireball;
    public Gradient fireballGradient;
    public float fireballCooldown = 2f;

    public GameObject shield;
    public Gradient shieldGradient;
    public float shieldCooldown = 6f;

    public GameObject heal;
    public Gradient healGradient;
    public float healCooldown = 1f;

    public GameObject swipeLeft;
    public GameObject swipeRight;
    public AudioClip cast_success;
    public AudioClip cast_failure;
    public Transform wand;
    public Transform book;
    //[HideInInspector]
    public Targeting target;
    public Transform avatar;
    public Transform torso;

	public PlayerStatus playerStatus;

    public bool blue = false;

    public bool noHats = true;
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
        target = GetComponent<Targeting>();
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
        if (Input.GetKeyDown("joystick button 15"))
        {
            if (hasSpell)
                CastSpell();
            else if (!isCoolingDown)
                IgniteFlame(baseGradient);
        }
        if(Input.GetKeyUp("joystick button 15"))
        {
            if(!hasSpell && !isCoolingDown && wand != null)
                wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }

    //Sets spell properties.
    void SetSpell(GameObject spell, string spellName, Gradient spellGradient)
    {
        currentSpell = spell;
        currentSpellName = spellName;
        currentSpellGradient = spellGradient;

        hasSpell = true;

        IgniteFlame(currentSpellGradient);

        GetComponent<VRGestureRig>().enabled = false;
        audioSource.PlayOneShot(cast_success);

    }

    //Updates the color of the wand flame and restarts it.
    void IgniteFlame(Gradient flameGradient)
    {
        if (wand != null)
        {
            ParticleSystem wandParticle = wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>();
            wandParticle.Stop();
            var wandParticleModule = wandParticle.colorOverLifetime;
            wandParticleModule.color = flameGradient;
            wandParticle.Play();
        }
    }
    void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble)
    {
        switch (gestureName)
        {
		case "Fire":
			if ((playerStatus.playerClass == PlayerClass.attack) || noHats == true) {
				SetSpell (fireball, "fire", fireballGradient);
			}
                break;
		case "Shield":
			if (playerStatus.playerClass == PlayerClass.support || noHats == true) {
				SetSpell (shield, "shield", shieldGradient);
			}
                break;
		case "Heal":
			if (playerStatus.playerClass == PlayerClass.heal || noHats == true) {
				SetSpell (heal, "heal", healGradient);
			}
                break;
            case "SwipeLeft":
                //   GameObject fb2 = PhotonNetwork.Instantiate(fireball.name, mainCam.transform.position - new Vector3(0, .3f, 0), mainCam.transform.rotation, 0);
                //spellLogic.Deflect();
                //audioSource.PlayOneShot(cast_success);
                break;
            case "SwipeRight":
                break;
        }
    }

    void OnGestureRejected(string error, string gestureName = null, double confidenceValue = 0)
    {
        audioSource.PlayOneShot(cast_failure);
    }

    //Get avatar's wand and book.
    public void SetAvatar(Transform _avatar)
    {
        avatar = _avatar;
        torso = avatar.Find("Torso");
        wand = avatar.Find("Right Hand").Find("MagicWand");
        book = avatar.Find("Left Hand").Find("SpellBook");
		playerStatus = torso.GetComponent<PlayerStatus>();
    }

    //Casts selected spell.
    private void CastSpell()
    {
        GameObject spellInstance;
        Transform wandTip = wand.Find("tip");

        switch (currentSpellName)
        {
		case "fire":
			Quaternion spellRotation = target.result != null ? Quaternion.LookRotation (target.result.position - wandTip.transform.position) : wandTip.rotation;
				spellInstance = PhotonNetwork.Instantiate (currentSpell.name, wandTip.position, spellRotation, 0);
                spellTimer = fireballCooldown;
                break;
            case "shield":
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position + wandTip.forward, Camera.main.transform.rotation, 0);
                //spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position + wandTip.forward, wandTip.rotation, 0);
                //spellInstance.transform.SetParent(wandTip);
                spellTimer = shieldCooldown;
                break;
            case "heal":
                // Heal others
                if (target.result != null)
                {
                    spellInstance = PhotonNetwork.Instantiate(currentSpell.name, target.result.transform.position + new Vector3(-1,0,0), currentSpell.transform.rotation, 0);
                }

                // Self heal
                else
                {
                    print("self heal");
                    print(avatar);
                    spellInstance = PhotonNetwork.Instantiate(currentSpell.name, torso.transform.position + new Vector3(-1, 0, 0), currentSpell.transform.rotation, 0);
                }
                spellTimer = healCooldown;
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


