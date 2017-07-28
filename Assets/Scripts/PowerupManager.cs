using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

    public GameObject[] redPlatforms;
    public GameObject[] bluePlatforms;
    
    public float frequency;
    public GameObject page;

    public float timer;
    bool[] redPowerups;
    bool[] bluePowerups;
    public int numPowerups;

    // Use this for initialization
    void Start () {
        timer = frequency;
        redPowerups = new bool[redPlatforms.Length];
        bluePowerups = new bool[bluePlatforms.Length];
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0 && numPowerups < 6)
        {
            int randomPlatform;
            if (HasSpace(redPlatforms, redPowerups))
            {
                randomPlatform = Random.Range(0, redPlatforms.Length);
                while (redPowerups[randomPlatform] || redPlatforms[randomPlatform].GetComponent<PlatformNeighbors>().hasPlayer)
                {
                    randomPlatform = Random.Range(0, redPlatforms.Length);
                }
                PhotonNetwork.Instantiate(page.name, redPlatforms[randomPlatform].transform.position + new Vector3(0, 1, 0), new Quaternion(45, 0, 45, 0), 0).GetComponent<Powerup>().SetPowerupProperties(false, randomPlatform);
                redPowerups[randomPlatform] = true;
                numPowerups++;
            }

            if (HasSpace(bluePlatforms, bluePowerups))
            {
                randomPlatform = Random.Range(0, bluePlatforms.Length);
                while (bluePowerups[randomPlatform] || bluePlatforms[randomPlatform].GetComponent<PlatformNeighbors>().hasPlayer)
                {
                    randomPlatform = Random.Range(0, bluePlatforms.Length);
                }
                PhotonNetwork.Instantiate(page.name, bluePlatforms[randomPlatform].transform.position + new Vector3(0, 1, 0), new Quaternion(45, 0, 45, 0), 0).GetComponent<Powerup>().SetPowerupProperties(true, randomPlatform);
                bluePowerups[randomPlatform] = true;
                numPowerups++;
            }

            ResetTimer();
        }
	}

    bool HasSpace(GameObject[] platforms, bool[] powerups)
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            if (! platforms[i].GetComponent<PlatformNeighbors>().hasPlayer && ! powerups[i])
            {
                return true;
            }
        }

        return false;
    }

    public void ResetTimer()
    {
        timer = frequency;
    }

    public void DecrementPowerUp(bool isBlue, int platformIndex)
    {
        numPowerups--;

        if (isBlue)
        {
            bluePowerups[platformIndex] = false;
        }
        else
        {
            redPowerups[platformIndex] = false;
        }
    }
}