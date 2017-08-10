using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR;
using Edwon.VR.Gesture;

public class SpellcastingGestureRecognition : MonoBehaviour {

    public ParticleSystem drawEffect;

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
    [HideInInspector]
    public bool isCoolingDown = false;

	// Variables for targeting platforms
	private BeamTrail beamTrail;
    public GameObject reticle;
    private LineRenderer lineRend;
    public Gradient accurateTarget;
    public Gradient inaccurateTarget;

    [HideInInspector]
    SpellCooldowns cooldowns;
    [HideInInspector]
    public float fireCD, iceCD, swordCD, meteorCD, shieldCD, pongCD, vinesCD, healCD, blessingCD, flipCD;

    private void Start()
    {
        mainCam = Camera.main;
        audioSource = GetComponent<AudioSource>();
        target = GetComponent<Targeting>();
        //print(target.pointer);
        if (target.pointer.Find("BeamTrail").gameObject.GetActive() == false)
            target.pointer.Find("BeamTrail").gameObject.SetActive(true);

        beamTrail = target.pointer.GetComponentInChildren<BeamTrail> ();
        //print(beamTrail);
        lineRend = beamTrail.GetComponent<LineRenderer>();
		beamTrail.gameObject.SetActive (false);
        reticle.SetActive(false);
        cooldowns = GetComponent<SpellCooldowns>();
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
            //if (spellTimer > 0)
            //{
            //    spellTimer -= Time.deltaTime;
            //}

            if(fireCD > 0)
            {
                fireCD -= Time.deltaTime;
            }

            if (iceCD > 0)
            {
                iceCD -= Time.deltaTime;
            }

            if (swordCD > 0)
            {
                swordCD -= Time.deltaTime;
            }

            if (meteorCD > 0)
            {
                meteorCD -= Time.deltaTime;
            }

            if (shieldCD > 0)
            {
                shieldCD -= Time.deltaTime;
            }

            if (pongCD > 0)
            {
                pongCD -= Time.deltaTime;
            }

            if (vinesCD > 0)
            {
                vinesCD -= Time.deltaTime;
            }

            if (healCD > 0)
            {
                healCD -= Time.deltaTime;
            }

            if (blessingCD > 0)
            {
                blessingCD -= Time.deltaTime;
            }

            if (flipCD > 0)
            {
                flipCD -= Time.deltaTime;
            }

            if(fireCD <=0 && iceCD <= 0 && swordCD <= 0 && meteorCD <= 0 && shieldCD <= 0 && pongCD <= 0 && vinesCD <= 0 && healCD <= 0 && blessingCD <= 0 && flipCD <= 0)
            {
                isCoolingDown = false;
                //GetComponent<VRGestureRig>().enabled = true;
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
                drawEffect.Play();
        }
        if(Input.GetKeyUp("joystick button 15"))
        {
            drawEffect.Stop();
            GetComponent<VRGestureRig>().enabled = true;
            if (!hasSpell && !isCoolingDown && wand != null)
            {
                wand.Find("tip").Find("flames").gameObject.GetComponent<ParticleSystem>().Stop();
                
            }
        }

        // Check if targeting platform
        if (currentSpellName == "vines" || currentSpellName == "platformSteal")
        {
            if (target.result != null)
            {
                if (target.result.gameObject.layer == LayerMask.NameToLayer("BluePlatform") || target.result.gameObject.layer == LayerMask.NameToLayer("RedPlatform"))
                {
                    AccurateTarget();
                }
                else
                {
                    InaccurateTarget();
                }
            }
            else
            {
                InaccurateTarget();
            }
        }
        else if (currentSpellName == "disenchant")
        {
            if (target.result2 != null)
            {
                if (target.result2.tag == "Curse")
                {
                    AccurateTargetBlessing();
                }
                else
                {
                    InaccurateTargetBlessing();
                }
            }
            else
            {
                InaccurateTargetBlessing();
            }
        
        }
        else
        {
            beamTrail.gameObject.SetActive(false);
            reticle.SetActive(false);
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

        //GetComponent<VRGestureRig>().enabled = false;
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

        //GetComponent<VRGestureRig>().enabled = false;
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
		    case "Jay":
			    if ((playerStatus.playerClass == PlayerClass.attack || playerStatus.playerClass == PlayerClass.all || noHats == true) && fireCD <= 0) {
				    SetSpell (fireball, "fire", fireballGradient);
			    }
                    break;
		    case "Shield":
			    if ((playerStatus.playerClass == PlayerClass.support || playerStatus.playerClass == PlayerClass.all || noHats == true) && shieldCD <= 0) {
				    SetSpell (shield, "shield", shieldGradient);
			    }
                    break;
		    case "Heal":
			    if ((playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true) && healCD <= 0) {
				    SetSpell (heal, "heal", healGradient);
			    }
                    break;
            case "Spring":
                if ((playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true) && vinesCD <= 0)
                {
                    SetSpell(vines, "vines", vinesGradient);
                }
                break;
            case "Bolt":
                if ((playerStatus.playerClass == PlayerClass.attack || playerStatus.playerClass == PlayerClass.all || noHats == true) && iceCD <= 0)
                {
                    SetSpell(iceball, "iceball", iceballGradient);
                }
                break;
            case "Wave":
                if ((playerStatus.playerClass == PlayerClass.support || playerStatus.playerClass == PlayerClass.all || noHats == true) && meteorCD <= 0)
                {
                    SetSpell(meteor, "meteor", meteorGradient);
                }
                break;
            case "OpenFrame":
                if ((playerStatus.playerClass == PlayerClass.support || playerStatus.playerClass == PlayerClass.all || noHats == true) && pongCD <= 0)
                {
                    SetSpell(pongShield, "pongShield", pongShieldGradient);
                }
                break;
            case "Star":
                if ((playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true) && flipCD <= 0)
                {
                    SetSpell(platformSteal, "platformSteal", platformStealGradient);
                }
                break;
            case "Zed":
                if ((playerStatus.playerClass == PlayerClass.attack || playerStatus.playerClass == PlayerClass.all || noHats == true) && swordCD <= 0)
                {
                    SetSpell(lightBlade, "lightBlade", lightBladeGradient);
                }
                break;
            case "Hourglass":
                if ((playerStatus.playerClass == PlayerClass.heal || playerStatus.playerClass == PlayerClass.all || noHats == true) && swordCD <= 0)
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
                fireCD = cooldowns.fireCD;
                //spellTimer = fireballCooldown;
                //if (baseSpellClass = spellInstance.GetComponent<BaseSpellClass>())
                //{
                //    SetSpellOwner(baseSpellClass);
                //}
                break;
            case "iceball":
                spellRotation = wandTip.rotation;
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position, spellRotation, 0);
                spellInstance.GetComponent<IceBall_1>().blue = avatar.GetComponent<TeamManager>().blue;
                iceCD = cooldowns.iceCD;
                //spellTimer = iceballCooldown;
                break;
            case "shield":
                //spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position + wandTip.forward, Camera.main.transform.rotation, 0);
                //spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position + wandTip.forward, wandTip.rotation, 0);
                //spellInstance.transform.SetParent(wandTip);
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, book.position + book.forward, book.rotation, 0);
                spellInstance.GetComponent<Shield>().SetBook(book);
                spellInstance.GetComponent<Shield>().SetBlue(avatar.GetComponent<TeamManager>().blue);
                shieldCD = cooldowns.shieldCD;
                //                spellInstance.transform.SetParent(book);
                //spellTimer = shieldCooldown;
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
                    //print("self heal");
                    print(avatar);
                    spellInstance = PhotonNetwork.Instantiate(currentSpell.name, torso.transform.position + new Vector3(-1, 0, 0), currentSpell.transform.rotation, 0);
                }
                healCD = cooldowns.healCD;
                //spellTimer = healCooldown;
                break;
            case "vines":
                //Check if target is a platform, otherwise don't do anything.
                if (target == null || target.result == null)
                {
                    return;
                }

                if (target.result.gameObject.layer == LayerMask.NameToLayer("BluePlatform") || target.result.gameObject.layer == LayerMask.NameToLayer("RedPlatform"))
                {
                    spellInstance = PhotonNetwork.Instantiate(vines.name, target.result.position, new Quaternion(), 0);
                    vinesCD = cooldowns.vinesCD;
                }
                else
                {
                    return;
                }
                break;
            case "pongShield":
                spellRotation = new Quaternion();
                spellInstance = PhotonNetwork.Instantiate(currentSpell.name, wandTip.position, spellRotation, 0);
                spellInstance.GetComponent<Pong_Shield>().SetBlue(avatar.GetComponent<TeamManager>().blue);
                pongCD = cooldowns.pongCD;
                //spellTimer = pongShieldCooldown;
                break;
		case "meteor":
			spellRotation = wandTip.rotation;
			spellInstance = PhotonNetwork.Instantiate (currentSpell.name, wandTip.position, spellRotation, 0);
			spellInstance.GetComponent<MeteorSpell> ().blue = avatar.GetComponent<TeamManager>().blue;
            meteorCD = cooldowns.meteorCD;
                // spellTimer = meteorCooldown;
                break;
            case "platformSteal":
                //Check if target is a platform, otherwise don't do anything.
                if (target == null || target.result == null)
                {
                    Debug.Log("target for platform steal is null");
                    return;
                }
                if (target.result.gameObject.layer == LayerMask.NameToLayer("BluePlatform") || target.result.gameObject.layer == LayerMask.NameToLayer("RedPlatform"))
                {
                    spellInstance = PhotonNetwork.Instantiate(platformSteal.name, target.result.position, new Quaternion(), 0);
                    target.result.GetComponent<PhotonView>().RPC("ChangeColor", PhotonTargets.AllBuffered, null);
                    flipCD = cooldowns.flipCD;
                    //spellTimer = platformStealCooldown;
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
                swordCD = cooldowns.swordCD;
                //spellTimer = lightBladeCooldown;
                break;
            case "disenchant":
                if (target != null && target.result2 != null && target.result2.CompareTag("Curse"))
                {
                    spellInstance = PhotonNetwork.Instantiate(currentSpell.name, target.result2.position, new Quaternion(), 0);
                    blessingCD = cooldowns.blessingCD;
                    //spellTimer = disenchantCooldown;
                    //target.result.GetComponent<VineTrap>().DestroyVines();
                    target.result2.GetComponent<PhotonView>().RPC("DestroyVines", PhotonTargets.AllBuffered, null);
                }
                else
                {
                    //spellTimer = disenchantCooldown;
                }
                break;
            default:
                //spellTimer = spellCooldown;
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
        GetComponent<VRGestureRig>().enabled = false;

    }
    void SetSpellOwner(BaseSpellClass bsp)
    {
        if(playerStatus.photonView.isMine)
        {
            bsp.SetOwner(avatar.gameObject);
        }
    }

    // Successfully target a platform
    void AccurateTarget()
    {
        Physics.queriesHitTriggers = false;
        beamTrail.gameObject.SetActive(true);
        reticle.SetActive(true);
        //RaycastHit hit;
        //Physics.Raycast(target.pointer.position, target.pointer.forward, out hit, target.range, target.layers);
        beamTrail.destination = target.hit.point;
        lineRend.colorGradient = accurateTarget;
        reticle.transform.position = target.hit.point;
    }
    void AccurateTargetBlessing()
    {
        Physics.queriesHitTriggers = true;
        beamTrail.gameObject.SetActive(true);
        reticle.SetActive(true);
        //RaycastHit hit;
        //Physics.Raycast(target.pointer.position, target.pointer.forward, out hit, target.range, target.layers);
        beamTrail.destination = target.hit_blessing.point;
        lineRend.colorGradient = accurateTarget;
        reticle.transform.position = target.hit_blessing.point;
    }

    // Draw dotted line when not hitting platform
    void InaccurateTarget()
    {
        Physics.queriesHitTriggers = false;
        beamTrail.gameObject.SetActive(true);
        RaycastHit hit;
        if (Physics.Raycast(target.pointer.position, target.pointer.forward, out hit, 1000/*, target.layers*/))
        {
            beamTrail.destination = (hit.point);
            lineRend.colorGradient = inaccurateTarget;
            reticle.SetActive(true);
            reticle.transform.position = hit.point;
        }
        else
        {
            beamTrail.destination = (target.pointer.position + target.pointer.forward * 100);
            reticle.SetActive(true);
            reticle.transform.position = (target.pointer.position + target.pointer.forward * 100);
        }

        lineRend.colorGradient = inaccurateTarget;
    }
    void InaccurateTargetBlessing()
    {
        Physics.queriesHitTriggers = true;
        beamTrail.gameObject.SetActive(true);
        RaycastHit hit;
        if (Physics.Raycast(target.pointer.position, target.pointer.forward, out hit, 1000, target.blessing_layers))
        {
            beamTrail.destination = (hit.point);
            lineRend.colorGradient = inaccurateTarget;
            reticle.SetActive(true);
            reticle.transform.position = hit.point;
        }
        else
        {
            beamTrail.destination = (target.pointer.position + target.pointer.forward * 100);
            reticle.SetActive(true);
            reticle.transform.position = (target.pointer.position + target.pointer.forward * 100);
        }

        lineRend.colorGradient = inaccurateTarget;
    }
}


