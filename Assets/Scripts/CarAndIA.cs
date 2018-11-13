using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Movement
{
    public float instant, accel, direction;
    public bool brake;
    public static bool operator ==(Movement m1, Movement m2)
    {
        return m1.accel == m2.accel && m1.direction == m2.direction && m1.brake == m2.brake;
    }
    public static bool operator !=(Movement m1, Movement m2)
    {
        return !(m1 == m2);
    }

    public override bool Equals(object o) //nomes per eliminar warnings
    {
        return false;
    }

    public override int GetHashCode() //nomes per eliminar warnings
    {
        return -1;
    }
}

public class CarAndIA : MonoBehaviour
{

    public int Acceleration = 5000;
    public int MaxSpeed = 20;
    public int BrakePower = 20000;
    public float Agility;                               //de moment no s'utilitza
    public List<WheelCollider> Motors, Steerings;       //motors = rodes que emputxen   steerings = rodes que giren
    public Transform CenterOfMass;
    public Transform InitialPos;
    public GameObject EndArea;
    private AudioSource hornAudio;
    private Rigidbody car;
    private Queue<Movement> path = new Queue<Movement>();                       //trajecte que ha realitzat el jugador quan controlava aquest cotxe
    private bool auto;                                  //true <--> el cotxe te un path i va sol
    private Movement movement;
    private float tPlayerBegin;                      //instant al que el player comença a controlar el cotxe
    private float tAutoBeguin;                       //instant al que la IA comença a controlar el cotxe
    private float tNextAction;                       //instant en que ha de canviar el input

    private void Awake()
    {
    }
    // Use this for initialization
    void Start()
    {
       //gameObject.GetComponent<Rigidbody>().centerOfMass = CenterOfMass.position;
        hornAudio = GetComponent<AudioSource>();
        car = GetComponent<Rigidbody>();
        auto = false;
        goInitialPos();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        car.isKinematic = false;
        if (auto)
        {
            float tIA = Time.time - tAutoBeguin;
            if(tIA > tNextAction) //la primera vegada no te tNextAction, s'ha de inicialitzar quan es pasi a auto
            {
                movement = path.Dequeue();
                tNextAction = path.Peek().instant;
            }

        }
        else
        {
            Movement anterior = movement;
            movement.accel = Input.GetAxisRaw("Vertical");
            movement.direction = Input.GetAxisRaw("Horizontal");
            movement.brake = Input.GetKey(KeyCode.Space);
            if (movement != anterior)
            {
                movement.instant = Time.time - tPlayerBegin;
                path.Enqueue(movement);
            }
        }
        if(movement.accel > 0) //DEBUG
        {
            int debug = 2; 
        }
        foreach (WheelCollider wheel in Motors)
        {
            if (movement.brake)
                wheel.brakeTorque = BrakePower;

            else
            {
                wheel.brakeTorque = 0;
                if (car.velocity.magnitude < MaxSpeed) wheel.motorTorque = movement.accel * Acceleration;
                else wheel.motorTorque = 0;
            }
        }
        foreach (WheelCollider wheel in Steerings)
        {
            wheel.steerAngle = 20 * movement.direction;
        }
        if (Input.GetKeyDown(KeyCode.H)) hornAudio.Play();
    }

    private void goInitialPos()
    {
        car.position = InitialPos.position;
        car.rotation = InitialPos.rotation;
        car.velocity = Vector3.zero;
        car.angularVelocity = Vector3.zero;
        foreach (WheelCollider wheel in Motors)
        {
            wheel.brakeTorque = Mathf.Infinity;
        }
        car.isKinematic = true;
    }
}
