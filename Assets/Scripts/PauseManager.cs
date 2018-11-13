using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {
    public bool paused;
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
            if (pauseMenu.activeSelf)
            {
                Unpause(pauseMenu);
            }
            else
            {
                Pause(pauseMenu);
            }
        }
		
	}

    public void Unpause(GameObject pauseMenu)
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        paused = false;
    }

    public void Pause(bool pause)
    {
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
        paused = true;
    }
}
