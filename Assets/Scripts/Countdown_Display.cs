using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown_Display : MonoBehaviour {

    public TextMesh red_timer;
    public TextMesh blue_timer;
    public Vector3 location;
    public float countdown_timer_max = 5.0f;
    public float y_acceleration = 9.78f;
    private float y_vel = 0;
    private float countdown_timer = 0;
    private TextMesh countdown_red_text;
    private TextMesh countdown_blue_text;
    private bool countdown_flag = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (countdown_flag)
        {
            Tick();
        }
    }

    void OnEnable()
    {
        this.transform.position = location;
        countdown_flag = true;
        countdown_timer = 0;
    }

    private void Tick()
    {
        // Update countdown
        countdown_timer += Time.deltaTime;
        if (countdown_timer > 1.25 * countdown_timer_max)
        {
            countdown_flag = false;
        }
        else if (countdown_timer > countdown_timer_max)
        {
            y_vel += y_acceleration * Time.deltaTime;
            this.transform.position += new Vector3(0, y_vel, 0);
        }
        else
        {
            int temp = Mathf.CeilToInt(countdown_timer_max - countdown_timer);
            float scale = 1.5f * (Mathf.Ceil(countdown_timer) - countdown_timer);
            red_timer.text = "" + temp;
            red_timer.characterSize = scale;
            blue_timer.text = "" + temp;
            blue_timer.characterSize = scale;
        }
        this.gameObject.SetActive(countdown_flag);
    }
}
