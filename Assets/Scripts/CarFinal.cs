using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Movement //utilitzat per enmagatzemar el input del jugador
{
    public Vector3 position;
    public Quaternion rotation;
    public Quaternion wheels_rotation;
    public bool smoking;
}

public class CarFinal : MonoBehaviour
{

    private Rigidbody car;
    private float m_horizontal_in;
    private float m_vertical_in;
    private float m_steering_angle;
    private float maxMotorForce;
    private bool break_force;
    private bool smoked = false; //treu fum
    private bool auto;                                  //true <--> el cotxe te un path i va sol
    private bool end;                                   //true <--> player ha chocat amb cotxe anant en auto
    private Movement actual;
    private List<Movement> path = new List<Movement>();                       //trajecte que ha realitzat el jugador quan controlava aquest cotxe
    private int frameNumber;
    private GameObject smokeInstance;

    public WheelCollider frontRWheel, frontLWheel;
    public WheelCollider rearRWheel, rearLWheel;
    public Transform frontRWheelT, frontLWheelT;
    public Transform rearRWheelT, rearLWheelT;
    public Transform InitialPos, EndArea;
    public float maxSteerAngle = 30;
    public float motorForce;
    public float maxVelocity = 400;
    public float breakPower = 3000;
    public float life = 100;

    public AudioSource[] sounds;
    public AudioSource hit_sound_1;
    public AudioSource hit_sound_2;
    public AudioSource hit_sound_3;

    public GameObject spark;
    public GameObject smoke;


    private void Start()
    {
        if (!auto) gameObject.tag = "Player";
        frameNumber = 0;
        car = GetComponent<Rigidbody>();
        auto = end = false;
        goInitialPos();
        maxMotorForce = motorForce;
        sounds = GetComponents<AudioSource>();
        hit_sound_1 = sounds[0];
        hit_sound_2 = sounds[1];
        hit_sound_3 = sounds[2];
    }

    private void GetInput()
    {
        m_horizontal_in = Input.GetAxis("Horizontal");
        m_vertical_in = Input.GetAxis("Vertical");
        if (m_vertical_in != 0)
        {
            int debug = 2;
        }
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
                smokeInstance = Instantiate(smoke, transform.position, rot, transform);
                smoked = true;
            }
        }
        else if (life > 50 && life <= 75) motorForce = maxMotorForce * 3 / 4;
    }

    private void MakeSmoke()
    {
        Quaternion rot = new Quaternion();
        rot.SetLookRotation(Vector3.up, Vector3.up);
        Instantiate(smoke, transform.position, rot, transform);
        smoked = true;
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
        if (car.velocity.magnitude < maxVelocity)
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
            if (auto && collision.gameObject.tag == "Player")
            {
                end = true;
                MakeSmoke();
            }

            else if (collision.relativeVelocity.magnitude > 2)
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

   

    private void FixedUpdate()
    {
        if (auto)
        {
            if (frameNumber < path.Count && !end) CopyFrameStatus();
        }
        else
        {
            GetInput();
            Steer();
            Accelerate();
            UpdateWheelMovement();
            SaveFrameStatus();
            Status(); //ho moc aqui perque aixi puc posar primer frame smoked a false sempre i aqui sobreescriure amb true si cal
        }
        frameNumber++;
    }

    private void CopyFrameStatus()
    {
        actual = path[frameNumber];
        //else transform.parent.gameObject.SetActive(false);
        car.position = actual.position;
        car.rotation = actual.rotation;
        frontRWheelT.rotation = actual.wheels_rotation;
        frontLWheelT.rotation = actual.wheels_rotation;
        if (actual.smoking && !smoked) MakeSmoke();

    }

    private void SaveFrameStatus()
    {
        actual.position = car.transform.position;
        actual.rotation = car.transform.rotation;
        actual.wheels_rotation = frontRWheelT.rotation;
        actual.smoking = smoked;
        path.Add(actual);

    }

    private void goInitialPos()
    {
        car.position = InitialPos.position;
        car.rotation = InitialPos.rotation;
        car.angularVelocity = Vector3.zero;
        rearLWheel.brakeTorque = Mathf.Infinity;
        rearRWheel.brakeTorque = Mathf.Infinity;
    }

    private void GoAuto()
    {
        frameNumber = 0;
        smoked = false;
        auto = true;
        Destroy(smokeInstance);
        life = 100;
        goInitialPos();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform == EndArea)
        {
            transform.parent.gameObject.SetActive(false);
            gameObject.tag = "Untagged";
            if (!auto)
            {
                transform.parent.parent.gameObject.SendMessage("NextCar");
            }
        }

        //else if (other.tag == "Player")
        //{
        //    end = true;
        //}

        else if (other.tag == "bonus")
        {
            GameObject.Find("Temporitzador").SendMessage("IncrementaTemps", 30);
            other.gameObject.SetActive(false);
        }
    }
}


