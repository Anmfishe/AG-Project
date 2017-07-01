using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLogic : MonoBehaviour
{
    public Camera mainCam;

    float deflectMinDist = 3;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Deflect()
    {
        GameObject[] spells = GameObject.FindGameObjectsWithTag("Spell");

        foreach (GameObject spell in spells)
        {
            // Particle[] particles = spell.GetComponent<ParticleSystem>().particles;
            ParticleSystem.Particle[] emittedParticles = new ParticleSystem.Particle[spell.GetComponent<ParticleSystem>().particleCount];
            spell.GetComponent<ParticleSystem>().GetParticles(emittedParticles);

            //print(Vector3.Distance(emittedParticles[0].position, mainCam.transform.position));
            //Vector3 cool = GetComponent<ParticleSystem>().Particle[0].position;

            //print(Vector3.Distance(particles[0].position, mainCam.transform.position));
            //  print(mainCam);
            //print("Particle Pos: " + emittedParticles[0].position  +"  |  Cam Pos: " + mainCam.transform.position);


            if ((emittedParticles.Length> 0) && Vector3.Distance(spell.transform.position + emittedParticles[0].position, mainCam.transform.position) < deflectMinDist)
            {
                //Vector3 particlePos = emittedParticles[0].position;
                //Vector3 particleVel = emittedParticles[0].velocity;
                Vector3 particlePos = spell.transform.position + emittedParticles[0].position;
                GameObject spellType = spell;
                spellType.GetComponent<ParticleSystem>().startSpeed = -10;
                //Destroy(spell);
                //spellType.transform.rotation = new Quaternion(spellType.transform.rotation.x * -1.0f,
                //                            spellType.transform.rotation.y,
                //                            spellType.transform.rotation.z,
                //                            spellType.transform.rotation.w * -1.0f);

                spellType.transform.position = particlePos;
                //spellType.GetComponent<ParticleSystem>().velocity
               Instantiate(spellType);
               // print(spellType.transform.position);
                //, new Vector3(0,0,0));
                //print(emittedParticles[0].velocity);
               // emittedParticles[0].velocity = -emittedParticles[0].velocity;
               // print(emittedParticles[0].velocity);
            }
        }
    }
}
