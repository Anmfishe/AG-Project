using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heal spell
// We need to heal every instance of player avatar.

public class HealSpell: MonoBehaviour
{
   // int times_hit = 0;
    public int healthAdded = 20;
    public GameObject target;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
