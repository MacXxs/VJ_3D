using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    private bool paused;
    public GameObject pauseMenu;
	// Use this for initialization
	void Start () {
        paused = false;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //parem el temps
            if (!paused) Time.timeScale = 0;
            else Time.timeScale = 1;
            paused = !paused;

            //obrim menu de pausa
            pauseMenu.gameObject.SetActive(true);
        }
		
	}

    public void unpause()
    {
        Time.timeScale = 1;
    }
}
