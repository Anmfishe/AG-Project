using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLogic : MonoBehaviour
{
    public Camera mainCam;

    public float deflectMinDist = 2;
    float lastDeflect;
    public float spellCooldown=1;
    public GameObject fireball_Spell;
    public bool debugKey = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Debug function
        if (debugKey == true && (Input.GetKeyDown("joystick button 16") || Input.GetKeyDown("joystick button 17")))
        {
            Deflect();
        }
    }

    public void Deflect()
    {
        print("deflect");
        GameObject[] spells = GameObject.FindGameObjectsWithTag("Spell");

        foreach (GameObject spell in spells)
        {
            // Particle[] particles = spell.GetComponent<ParticleSystem>().particles;
            ParticleSystem.Particle[] emittedParticles = new ParticleSystem.Particle[spell.GetComponent<ParticleSystem>().particleCount];
            spell.GetComponent<ParticleSystem>().GetParticles(emittedParticles);



            if ((emittedParticles.Length > 0) && Vector3.Distance(emittedParticles[0].position, mainCam.transform.position) < deflectMinDist && ((Time.time - lastDeflect > spellCooldown)))
            {
                Vector3 particlePos = emittedParticles[0].position;
                Vector3 origPos = spell.transform.position;

                GameObject spellType = spell;
                PhotonNetwork.Destroy(spell);

                spellType.transform.position = particlePos;
                spellType.transform.LookAt(origPos);
                GameObject spellType2 = PhotonNetwork.Instantiate(fireball_Spell.name, spellType.transform.position, spellType.transform.rotation, 0);
                lastDeflect = Time.time;
            }
        }
    }
}
