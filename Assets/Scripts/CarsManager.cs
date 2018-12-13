using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarsManager : MonoBehaviour {
    private int actual;
	// Use this for initialization
	void Start () {
        transform.GetChild(0).gameObject.SetActive(true);
        actual = 1;
    }

    // Update is called once per frame
    public void NextCar()
    {
        if (actual < transform.childCount)
        {
            for (int i = 0; i < actual; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetChild(0).gameObject.SendMessage("GoAuto");
            }
            transform.GetChild(actual).gameObject.SetActive(true);
            actual++;
        }
    }
}
