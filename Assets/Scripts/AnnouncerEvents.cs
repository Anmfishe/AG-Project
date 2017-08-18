using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncerEvents : MonoBehaviour {

    public AudioClip intro, countdown, roundStart, knockOut, vanquished;
    private AudioSource speaker;
	// Use this for initialization
	void Start () {
        speaker = this.GetComponent<AudioSource>();		
	}
    private void Awake()
    {
        if(speaker == null) speaker = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PlaySound(string soundName)
    {
        AudioClip audioClip = null;
        switch (soundName)
        {
            case "intro":
                audioClip = intro;
                break;
            case "countdown":
                audioClip = countdown;
                break;
            case "roundStart":
                audioClip = roundStart;
                break;
            case "knockOut":
                audioClip = knockOut;
                break;
            case "vanquished":
                audioClip = vanquished;
                break;
        }

        if (audioClip != null)
        {
            speaker.Stop();
            speaker.clip = audioClip;
            speaker.Play();
        }
    }

    [PunRPC]
    public void PlaySoundToOthers(AudioClip audioClip)
    {
        
    }
}
