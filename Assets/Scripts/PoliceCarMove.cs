using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarMove : MonoBehaviour {


    private float m_horizontal_in;
    private float m_vertical_in;
    private float m_steering_angle;
    private bool break_force;

    public WheelCollider frontRWheel, frontLWheel;
    public WheelCollider rearRWheel, rearLWheel;
    public Transform frontRWheelT, frontLWheelT;
    public Transform rearRWheelT, rearLWheelT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;
    public float maxVelocity = 400;
    public float breakPower = 3000;

    public AudioSource[] sounds;
    public AudioSource hit_sound_1;
    public AudioSource hit_sound_2;
    public AudioSource hit_sound_3;

    private void GetInput()
    {
        m_horizontal_in = Input.GetAxis("Horizontal");
        m_vertical_in = Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space)) break_force = true;
        else break_force = false;            
    }

    private void Steer()
    {
        m_steering_angle = maxSteerAngle * m_horizontal_in;
        frontRWheel.steerAngle = m_steering_angle;
        frontLWheel.steerAngle = m_steering_angle;
    }

    private void Accelerate()
    {
        if (break_force)
        {
            rearLWheel.brakeTorque = breakPower;
            rearRWheel.brakeTorque = breakPower;
        }
        else
        {
            rearLWheel.brakeTorque = 0;
            rearRWheel.brakeTorque = 0;
        }
        if (m_vertical_in * motorForce < maxVelocity)
        {
            frontRWheel.motorTorque = m_vertical_in * motorForce;
            frontLWheel.motorTorque = m_vertical_in * motorForce;
            rearRWheel.motorTorque = m_vertical_in * motorForce;
            rearLWheel.motorTorque = m_vertical_in * motorForce;
        }
        else
        {
            frontRWheel.motorTorque = 0;
            frontLWheel.motorTorque = 0;
            rearRWheel.motorTorque = 0;
            rearLWheel.motorTorque = 0;
        }
        
    }

    private void UpdateWheelMovement()
    {
        UpdateWheelPose(frontRWheel, frontRWheelT);
        UpdateWheelPose(frontLWheel, frontLWheelT);
        UpdateWheelPose(rearRWheel, rearRWheelT);
        UpdateWheelPose(rearLWheel, rearLWheelT);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation; //Quaternions are used to represent rotations

        collider.GetWorldPose(out pos, out quat);

        transform.position = pos;
        transform.rotation = quat;
    }

    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.relativeVelocity.magnitude < 2) hit_sound_2.Play();
        else if (collision.relativeVelocity.magnitude > 2 && collision.relativeVelocity.magnitude < 10) hit_sound_2.Play();
        else hit_sound_3.Play();
    }

    private void Start()
    {
        sounds = GetComponents<AudioSource>();
        hit_sound_1 = sounds[0];
        hit_sound_2 = sounds[1];
        hit_sound_3 = sounds[2];
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelMovement();
    }
}
