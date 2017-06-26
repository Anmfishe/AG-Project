using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour {
	public float sensitivity = 100;

	float loudness = 0;

	AudioSource audio_source;

	void Start() {
		audio_source = GetComponent<AudioSource> ();
		audio_source.clip = Microphone.Start(Microphone.devices [0], true, 10, 44100);

		audio_source.loop = true; // Set the AudioClip to loop
//		audio_source.mute = true; // Mute the sound, we don't want the player to hear it
		while (!(Microphone.GetPosition(Microphone.devices [0]) > 0)){} // Wait until the recording has started
		audio_source.Play(); // Play the audio source!
	}

	void Update(){
		loudness = GetAveragedVolume() * sensitivity;
	}

	float GetAveragedVolume()
	{ 
		float[] data = new float[256];
		float a = 0;
		audio_source.GetOutputData(data,0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/256;
	}
}