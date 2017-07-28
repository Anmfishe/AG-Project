using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMain : MonoBehaviour {

    public bool isBlue;
    public string currentColor;

    public Material blueMaterial;
    public string blueTag = "BluePlatform";
    public string blueLayer = "BluePlatform";

    public Material redMaterial;
    public string redTag = "RedPlatform";
    public string redLayer = "RedPlatform";


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeColor()
    {
        ChangeColor(isBlue? "red" : "blue");
    }
    public void ChangeColor(string color)
    {
        print("BEFORE: " + LayerMask.NameToLayer(redLayer));
        if (currentColor == color) return;
        
        currentColor = color;
        
        switch (color)
        {
            
            case "blue":
                this.GetComponent<Renderer>().material = blueMaterial;
                this.gameObject.layer = LayerMask.NameToLayer(blueLayer);
                this.tag = blueTag;
                isBlue = true;
                break;
            case "red":
                this.GetComponent<Renderer>().material = redMaterial;
                this.gameObject.layer = LayerMask.NameToLayer(redLayer);
                this.tag = redTag;
                isBlue = false;
                break;
        }

        print("AFTER: " + currentColor);
    }

    void SetPlatform()
    {

    }
}
