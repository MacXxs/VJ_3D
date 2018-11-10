using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    private float start;
    private UnityEngine.UI.Text text;
    
	// Use this for initialization
	void Start () {
        start = Time.time;
        text = gameObject.GetComponent<UnityEngine.UI.Text>();

    }
	
	// Update is called once per frame
	void Update () {
        float t = Time.time - start; 
        string minutes = Mathf.Floor(t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        text.text = minutes + ':' + seconds;
    }
}
