﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR;
using Edwon.VR.Gesture;

public class SpellcastingGestureRecognition : MonoBehaviour {

    public Gradient baseGradient;

    public GameObject fireball;
    public string fireballGesture;
    public Gradient fireballGradient;
    public float fireballCooldown = 2f;

    public GameObject shield;
    public string shieldGesture;
    public Gradient shieldGradient;
    public float shieldCooldown = 6f;

    public GameObject heal;
    public string healGesture;
    public Gradient healGradient;
    public float healCooldown = 1f;

    public GameObject vines;
    public string vinesGesture;
    public Gradient vinesGradient;
    public float vinesCooldown = 2f;

    public GameObject iceball;
    public string iceballGesture;
    public Gradient iceballGradient;
    public float iceballCooldown = 2f;

    public GameObject meteor;
    public string meteorGesture;
    public Gradient meteorGradient;
    public float meteorCooldown = 2f;
    
    public GameObject pongShield;
    public string pongShieldGesture;
    public Gradient pongShieldGradient;
    public float pongShieldCooldown = 2f;

    public GameObject platformSteal;
    public string platformStealGesture;
    public Gradient platformStealGradient;
    public float platformStealCooldown = 2f;
    
    public GameObject lightBlade;
    public string lightBladeGesture;
    public Gradient lightBladeGradient;
    public float lightBladeCooldown = 2f;

    public GameObject disenchant;
    public string disenchantGesture;
    public Gradient disenchantGradient;
    public float disenchantCooldown = 2f;

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
        mainCam = Camera.main;
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

    public void SetRandomSpell()
    {
        int random = Random.Range(0, 9);

        switch (random)
        {
            case 0:
                currentSpell = fireball;
                currentSpellName = "fireball";
                currentSpellGradient = fireballGradient;
                break;
            case 1:
                currentSpell = shield;
                currentSpellName = "shield";
                currentSpellGradient = shieldGradient;
                break;
            case 2:
                currentSpell = heal;
                currentSpellName = "heal";
                currentSpellGradient = healGradient;
                break;
            case 3:
                currentSpell = vines;
                currentSpellName = "vines";
                currentSpellGradient = vinesGradient;
                break;
            case 4:
                currentSpell = iceball;
                currentSpellName = "iceball";
                currentSpellGradient = iceballGradient;
                break;
            case 5:
                currentSpell = meteor;
                currentSpellName = "meteor";
                currentSpellGradient = meteorGradient;
                break;
            case 6:
                currentSpell = pongShield;
                currentSpellName = "pongShield";
                currentSpellGradient = pongShieldGradient;
                break;
            case 7:
                currentSpell = platformSteal;
               
                currentSpellName = "platformSteal";
                currentSpellGradient = platformStealGradient;
                break;
            case 8:
                currentSpell = lightBlade;
                currentSpellName = "lightBlade";
                currentSpellGradient = lightBladeGradient;
                break;
            default:
                break;
        }

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
			    if (playerStatus.playerClass == PlayerClass.attack || playerStatus.playerClass == PlayerClass.all || noHats == true) {
				    SetSpell (fireball, "fire", fireballGradient);
			    }
                    break;
		    case "Shield":
			    if (playerStatus.playerClass == PlayerClass.support || playerStatus.playerClass == PlayerClass.all || noHats == true) {
				    SetSpell (shield, "shield", shieldGradient);
			    }
                    break;
		    case "Heal":
			    if (playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true) {
				    SetSpell (heal, "heal", healGradient);
			    }
                    break;
            case "Spring":
                if (playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true)
                {
                    SetSpell(vines, "vines", vinesGradient);
                }
                break;
            case "Diamond":
                if (playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true)
                {
                    SetSpell(iceball, "iceball", iceballGradient);
                }
                break;
            case "Wave":
                if (playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true)
                {
                    SetSpell(meteor, "meteor", meteorGradient);
                }
                break;
            case "OpenFrame":
                if (playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true)
                {
                    SetSpell(pongShield, "pongShield", pongShieldGradient);
                }
                break;
            case "Star":
                if (playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true)
                {
                    SetSpell(platformSteal, "platformSteal", platformStealGradient);
                }
                break;
            case "Zed":
                if (playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true)
                {
                    SetSpell(lightBlade, "lightBlade", lightBladeGradient);
                }
                break;
            case "Elle":
                if (playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true)
                {
                    SetSpell(disenchant, "disenchant", disenchantGradient);
                }
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
        GameObject spellInstance = null;
        Transform wandTip = wand.Find("tip");
        Quaternion spellRotation;
        BaseSpellClass baseSpellClass;
        switch (currentSpellName)
        {
            case "fire":
                spellRotation = target.result != null && target.result.CompareTag("Player") ? Quaternion.LookRotation(target.result.position - wandTip.transform.position) : wandTip.rotation;
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position, spellRotation, 0);
                spellTimer = fireballCooldown;
                //if (baseSpellClass = spellInstance.GetComponent<BaseSpellClass>())
                //{
                //    SetSpellOwner(baseSpellClass);
                //}
                break;
            case "iceball":
                spellRotation = wandTip.rotation;
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position, spellRotation, 0);
                spellTimer = iceballCooldown;
                break;
            case "shield":
                //spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position + wandTip.forward, Camera.main.transform.rotation, 0);
                //spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position + wandTip.forward, wandTip.rotation, 0);
                //spellInstance.transform.SetParent(wandTip);
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, book.position + book.forward, book.rotation, 0);
                spellInstance.GetComponent<Shield>().SetBook(book);
//                spellInstance.transform.SetParent(book);
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
            case "vines":
                //Check if target is a platform, otherwise don't do anything.
                if (target == null || target.result == null)
                {
                    return;
                }

                if (target.result.tag == "BluePlatform" || target.result.tag == "RedPlatform")
                {
                    spellInstance = PhotonNetwork.Instantiate(vines.name, target.result.position, new Quaternion(), 0);
                }
                else
                {
                    return;
                }
                break;
            case "pongShield":
                spellRotation = new Quaternion();
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position, spellRotation, 0);
                spellTimer = pongShieldCooldown;
                break;
		case "meteor":
			spellRotation = wandTip.rotation;
			spellInstance = PhotonNetwork.Instantiate (currentSpell.name, wandTip.position, spellRotation, 0);
			spellInstance.GetComponent<MeteorSpell> ().blue = avatar.GetComponent<TeamManager>().blue;
                spellTimer = meteorCooldown;
                break;
            case "platformSteal":
                //Check if target is a platform, otherwise don't do anything.
                if (target == null || target.result == null)
                {
                    Debug.Log("target for platform steal is null");
                    return;
                }
                if (target.result.tag == "BluePlatform" || target.result.tag == "RedPlatform")
                {
                    spellInstance = PhotonNetwork.Instantiate(platformSteal.name, target.result.position, new Quaternion(), 0);
                    target.result.GetComponent<PhotonView>().RPC("ChangeColor", PhotonTargets.AllBuffered, null);
                    spellTimer = platformStealCooldown;
                }
                else
                {
                    return;
                }

                break;
            case "lightBlade":
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position, wandTip.rotation, 0);
				spellInstance.GetComponent<LightBlade>().SetBlue(avatar.GetComponent<TeamManager>().blue);
				spellInstance.GetComponent<LightBlade>().SetWand(wandTip);
                spellTimer = lightBladeCooldown;
                break;
            case "disenchant":
                if (target != null && target.result != null && target.result.CompareTag("Curse"))
                {
                    spellInstance = PhotonNetwork.Instantiate(currentSpell.name, target.result.position, new Quaternion(), 0);
                    spellTimer = disenchantCooldown;
                    //target.result.GetComponent<VineTrap>().DestroyVines();
                    target.result.GetComponent<PhotonView>().RPC("DestroyVines", PhotonTargets.AllBuffered, null);
                }
                else
                {
                    spellTimer = disenchantCooldown;
                }
                break;
            default:
                spellTimer = spellCooldown;
                break;
        }
        
        if (wand != null)
        {
            wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>().Stop();
            ParticleSystem ps = wand.Find("tip").Find("smoke").GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = ps.main;
            ps.Stop();
            main.duration = spellTimer;
            ps.Play();
        }

        isCoolingDown = true;
        hasSpell = false;
        currentSpell = null;
        //target.currentSpellName = "";
        currentSpellName = "";
        
    }
    void SetSpellOwner(BaseSpellClass bsp)
    {
        if(playerStatus.photonView.isMine)
        {
            bsp.SetOwner(avatar.gameObject);
        }
    }
}


