using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarMove : MonoBehaviour {


    private float m_horizontal_in;
    private float m_vertical_in;
    private float m_steering_angle;
    private float maxMotorForce;
    private bool break_force;
    private bool smoked = false; //treu fum
    private Rigidbody car_rigidBody;

    public WheelCollider frontRWheel, frontLWheel;
    public WheelCollider rearRWheel, rearLWheel;
    public Transform frontRWheelT, frontLWheelT;
    public Transform rearRWheelT, rearLWheelT;
    public float maxSteerAngle = 40;
    public float motorForce = 200;
    public float maxVelocity = 1000;
    public float breakPower = 900;
    public float life = 200;

    public AudioSource[] sounds;
    public AudioSource hit_sound_1;
    public AudioSource hit_sound_2;
    public AudioSource hit_sound_3;

    public GameObject spark;
    public GameObject smoke;

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

    private void Status()
    {
        if (life <= 0) motorForce = 0;
        else if (life > 0 && life <= 25) motorForce = maxMotorForce * 1 / 4;
        else if (life > 25 && life <= 50)
        {
            motorForce = maxMotorForce * 2 / 4;
            if (!smoked)
            {
                Quaternion rot = new Quaternion();
                rot.SetLookRotation(Vector3.up, Vector3.up);
                Instantiate(smoke, transform.position, rot,transform);
                smoked = true;
            }
        }
        else if (life > 50 && life <= 75) motorForce = maxMotorForce * 3 / 4;
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
        if (car_rigidBody.velocity.magnitude < maxVelocity)
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
        if (collision.transform != transform && collision.contacts.Length > 0)
        {
            if (collision.relativeVelocity.magnitude > 2)
            {
                for (int i = 0; i < collision.contacts.Length; ++i)
                {
                    //Agafo la posicio de l'objecte amb el que es xoca per definir la direccio cap a la 
                    //qual les particules han de dirigir-se
                    Vector3 relativePos = collision.gameObject.transform.position - transform.position;
                    Quaternion rot = Quaternion.LookRotation(relativePos, Vector3.up);
                    //Instancio espurnes cada cop que es xoca amb suficient força en la posició del xoc i 
                    //en la direccio del objecte amb el qual es xoca
                    Instantiate(spark, collision.contacts[i].point, rot); 
                }
            }
            if (collision.relativeVelocity.magnitude > 2 && collision.relativeVelocity.magnitude < 5) hit_sound_2.Play();
            else if (collision.relativeVelocity.magnitude > 5 && collision.relativeVelocity.magnitude < 8) hit_sound_1.Play();
            else if (collision.relativeVelocity.magnitude > 8) hit_sound_3.Play();
            life -= collision.relativeVelocity.magnitude; //Reduïm la vida del cotxe
        }
    }

    private void Start()
    {
        car_rigidBody = GetComponent<Rigidbody>();
        maxMotorForce = motorForce;
        sounds = GetComponents<AudioSource>();
        hit_sound_1 = sounds[0];
        hit_sound_2 = sounds[1];
        hit_sound_3 = sounds[2];
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Status();
        Accelerate();
        UpdateWheelMovement();
    }
}
