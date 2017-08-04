using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetablePlayer : MonoBehaviour
{

    public Shader baseShader;
    public Shader selectedShader;
    Renderer materialRenderer;
    Texture texture;

    public GameObject head;
    public GameObject torso;

   

    void Start()
    {
        materialRenderer = GetComponent<Renderer>();
        baseShader = materialRenderer.material.shader;
        selectedShader = Shader.Find("Custom/Outline and ScreenSpace texture");
        texture = materialRenderer.material.mainTexture;
        
    }
    public void SetIndicator(bool on)
    {
        //Change shaders
        if (on)
        {
            head.GetComponent<Renderer>().material.shader = selectedShader;
            head.GetComponent<Renderer>().material.SetFloat("_OutlineVal", 0.125f);
            head.GetComponent<Renderer>().material.SetColor("_OutlineCol", Color.cyan);

            torso.GetComponent<Renderer>().material.shader = selectedShader;
            torso.GetComponent<Renderer>().material.SetFloat("_OutlineVal", 0.125f);
            torso.GetComponent<Renderer>().material.SetColor("_OutlineCol", Color.cyan);
         
        }
        else
        {
            head.GetComponent<Renderer>().material.shader = baseShader;
            torso.GetComponent<Renderer>().material.shader = baseShader;
            
        }
        head.GetComponent<Renderer>().material.mainTexture = texture;
        torso.GetComponent<Renderer>().material.mainTexture = texture;
    }
}