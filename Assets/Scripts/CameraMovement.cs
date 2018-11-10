using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraMovement : MonoBehaviour {
    private Vector3 pos;
    private bool defaultCameraMode;

    // Use this for initialization
    void Start () {
        defaultCameraMode = true;
    }

    // Update is called once per frame
    void LateUpdate () {
        defaultCameraMode = !Input.GetKey(KeyCode.C);
        if (defaultCameraMode)
        {
            gameObject.transform.position.Set(0, 0, 0);
            gameObject.transform.rotation.Set(90, 0, 0, 0);
        }
        else {
            gameObject.transform.rotation.Set(0, 0, 0, 0);
        }


    }
}
