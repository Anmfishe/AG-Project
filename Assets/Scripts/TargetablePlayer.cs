using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetablePlayer : MonoBehaviour {

    public GameObject indicator;

    public void SetIndicator(bool on)
    {

        indicator.SetActive(on);
    }
}
