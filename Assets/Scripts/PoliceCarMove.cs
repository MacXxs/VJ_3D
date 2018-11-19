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
    public float breakPower = 1000;

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
            frontLWheel.brakeTorque = breakPower;
            frontRWheel.brakeTorque = breakPower;
        }
        else
        {
            frontLWheel.brakeTorque = 0;
            frontRWheel.brakeTorque = 0;
            frontRWheel.motorTorque = m_vertical_in * motorForce;
            frontLWheel.motorTorque = m_vertical_in * motorForce;
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

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelMovement();
    }
}
