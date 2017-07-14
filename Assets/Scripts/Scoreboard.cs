using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour, IPunObservable {

	public GameObject[] red_score_display;
	public GameObject[] blue_score_display;

    [SerializeField]
	int red_score;

    [SerializeField]
	int blue_score;

	// Use this for initialization
	void Start () {
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Reset()
	{
		red_score_display [0].GetComponent<TextMesh> ().text = "0";
		red_score_display [1].GetComponent<TextMesh> ().text = "0";
		blue_score_display [0].GetComponent<TextMesh> ().text = "0";
		blue_score_display [1].GetComponent<TextMesh> ().text = "0";

		red_score = 0;
		blue_score = 0;
	}

	public void IncrementRedScore()
	{
		++red_score;
		red_score_display [0].GetComponent<TextMesh> ().text = "" + red_score;
		red_score_display [1].GetComponent<TextMesh> ().text = "" + red_score;
	}

	public void IncrementBlueScore()
	{
		++blue_score;
		blue_score_display [0].GetComponent<TextMesh> ().text = "" + blue_score;
		blue_score_display [1].GetComponent<TextMesh> ().text = "" + blue_score;
	}

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(red_score);
            stream.SendNext(blue_score);
        }
        else
        {
            this.red_score = (int)stream.ReceiveNext();
            this.blue_score = (int)stream.ReceiveNext();
            red_score_display[0].GetComponent<TextMesh>().text = "" + red_score;
            red_score_display[1].GetComponent<TextMesh>().text = "" + red_score;
            blue_score_display[0].GetComponent<TextMesh>().text = "" + blue_score;
            blue_score_display[1].GetComponent<TextMesh>().text = "" + blue_score;
        }
    }
}
