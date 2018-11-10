using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour {
    public int Acceleration = 5000;
    public int maxSpeed;
    public int BrakePower = 20000;
    public float agility; //de moment no s'utilitza
    public List<WheelCollider> motors, steerings;
    public Transform centerOfMass;
    private AudioSource hornAudio;
    private Rigidbody car;

    private float accel;
    private float direction;

    private void Awake()
    {
    }
    // Use this for initialization
    void Start () {
        //gameObject.GetComponent<Rigidbody>().centerOfMass.Set(0, -massCenter, 0);
        hornAudio = GetComponent<AudioSource>();
        car = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        accel = Input.GetAxis("Vertical");
        direction = Input.GetAxis("Horizontal");
        foreach (WheelCollider wheel in motors)
        {
            if (Input.GetKey(KeyCode.Space))
                wheel.brakeTorque =  BrakePower;
            else 
            {
                wheel.brakeTorque = 0;
                if (car.velocity.magnitude < maxSpeed) wheel.motorTorque = accel * Acceleration;
                else wheel.motorTorque = 0;
            }
        }
        foreach(WheelCollider wheel in steerings)
        {
            wheel.steerAngle = 45 * direction;
        }
        if (Input.GetKeyDown(KeyCode.H)) hornAudio.Play();
	}
}
