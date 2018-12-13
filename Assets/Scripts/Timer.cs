using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    private float start;
    private UnityEngine.UI.Text text;

    public int time; //time in seconds
    public GameObject EndMenu;
    
	// Use this for initialization
	void Start () {
        start = Time.time;
        text = gameObject.GetComponent<UnityEngine.UI.Text>();
        time++;
    }
	
	// Update is called once per frame
	void Update () {
        float t = time - (Time.time - start); 
        string minutes = Mathf.Floor(t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        if(t >= 0) text.text = minutes + ':' + seconds;
        else
        {
            //Time.timeScale = 0;
            EndMenu.SetActive(true);
        }
    }

    void IncrementaTemps(int t)
    {
        time += t;
    }
}
