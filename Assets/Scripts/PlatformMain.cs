using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMain : MonoBehaviour
{

    public bool isBlue;
    public string currentColor;
    private string originalColor;

    private bool isFlipped = false; //Flag to tell if tile has been flipped, and used to activate timer.
    public float resetDuration = 30f; //How long will the tile stay flipped.
    private float resetTimer = 0; //

    public Material blueMaterial;
    public string blueTag = "BluePlatform";
    public string blueLayer = "BluePlatform";

    public Material redMaterial;
    public string redTag = "RedPlatform";
    public string redLayer = "RedPlatform";

    public Material grayMaterial;
    public string grayTag = "GrayPlatform";
    public string grayLayer = "GrayPlatform";

    // Use this for initialization
    void Start()
    {
        originalColor = currentColor;
    }

    // Update is called once per frame
    void Update()
    {

        //Only check timer if tile has been flipped.
        if (isFlipped)
        {
            if (resetTimer > 0)
            {
                //Decrease timer by passed time.
                resetTimer -= Time.deltaTime;
            }
            else
            {
                //Change color to its original one.
                ChangeColor(originalColor);
            }
        }

    }

    [PunRPC]
    public void ChangeColor()
    {
        ChangeColor(isBlue ? "red" : "blue");
    }

    [PunRPC]
    public void ChangeColorTo(bool blue)
    {
        ChangeColor(blue ? "red" : "blue");
    }

    public void ChangeColor(string color)
    {

        if (currentColor == color)
        {
            return;
        }
        else if (color == originalColor)
        {
            //Reset to unflipped.
            isFlipped = false;

            //Cancel timer.
            resetTimer = 0;
        }
        else
        {
            isFlipped = true;

            //Start timer to reset to the original color.
            resetTimer = resetDuration;
        }

        currentColor = color;

        switch (color)
        {

            case "blue":
                this.GetComponent<Renderer>().material = blueMaterial;
                this.gameObject.layer = LayerMask.NameToLayer(blueLayer);
                this.tag = blueTag;
                isBlue = true;
                GetComponent<PlatformNeighbors>().layerSave = LayerMask.NameToLayer(blueLayer);
                break;
            case "red":
                this.GetComponent<Renderer>().material = redMaterial;
                this.gameObject.layer = LayerMask.NameToLayer(redLayer);
                this.tag = redTag;
                isBlue = false;
                GetComponent<PlatformNeighbors>().layerSave = LayerMask.NameToLayer(redLayer);
                break;
            case "gray":
                this.GetComponent<Renderer>().material = grayMaterial;
                this.gameObject.layer = LayerMask.NameToLayer(grayLayer);
                this.tag = grayTag;
                isBlue = false;
                GetComponent<PlatformNeighbors>().layerSave = LayerMask.NameToLayer(grayLayer);
                break;
        }
    }

    void SetPlatform()
    {

    }
}