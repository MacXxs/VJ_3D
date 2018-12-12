//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;



//public class CarAndIA : MonoBehaviour
//{

//    public int Acceleration = 5000;
//    public int MaxSpeed = 20;
//    public int BrakePower = 20000;
//    public float Agility;                               //de moment no s'utilitza
//    public List<WheelCollider> Motors, Steerings;       //motors = rodes que emputxen   steerings = rodes que giren
//    public Transform CenterOfMass;
//    public Transform InitialPos;
//    public GameObject EndArea;
//    private AudioSource hornAudio;
//    private Rigidbody car;
//    private Queue<Movement> path = new Queue<Movement>();                       //trajecte que ha realitzat el jugador quan controlava aquest cotxe
//    private bool auto;                                  //true <--> el cotxe te un path i va sol
//    private bool end;                                   //true <--> ha anat auto i ha acabat la ruta
//    private Movement movement;
//    private float tPlayerBegin;                      //instant al que el player comença a controlar el cotxe
//    private float tAutoBeguin;                       //instant al que la IA comença a controlar el cotxe
//    private float tNextAction;                       //instant en que ha de canviar el input

//    private void Awake()
//    {
//    }
//    // Use this for initialization
//    void Start()
//    {
//        gameObject.GetComponent<Rigidbody>().centerOfMass = CenterOfMass.position;
//        hornAudio = GetComponent<AudioSource>();
//        car = GetComponent<Rigidbody>();
//        tPlayerBegin = Time.time;
        
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        car.constraints = RigidbodyConstraints.None;
//        car.isKinematic = false;
//        if (auto && !end)
//        {
//            float tIA = Time.time - tAutoBeguin;
//            if(tIA > tNextAction) //falta comprovar que no estigui buida
//            {
//                movement = path.Dequeue();
//                if (path.Count > 0) tNextAction = path.Peek().instant;
//                else end = true;
//            }
//        }
//        else
//        {
//            Movement anterior = movement;
//            movement.accel = Input.GetAxisRaw("Vertical");
//            movement.direction = Input.GetAxisRaw("Horizontal");
//            movement.brake = Input.GetKey(KeyCode.Space);
//            if (movement != anterior)
//            {
//                movement.instant = Time.time - tPlayerBegin;
//                path.Enqueue(movement);
//            }
//        }


//        foreach (WheelCollider wheel in Motors)
//        {
//            if (movement.brake)
//                wheel.brakeTorque = BrakePower;

//            else
//            {
//                wheel.brakeTorque = 0;
//                if (car.velocity.magnitude < MaxSpeed) wheel.motorTorque = movement.accel * Acceleration;
//                else wheel.motorTorque = 0;
//            }
//        }
//        foreach (WheelCollider wheel in Steerings)
//        {
//            wheel.steerAngle = 20 * movement.direction;
//        }
//        if (Input.GetKeyDown(KeyCode.H)) hornAudio.Play();
//    }

//    private void goInitialPos()
//    {
//        car.constraints = RigidbodyConstraints.FreezeAll;
//        car.position = InitialPos.position;
//        car.rotation = InitialPos.rotation;
//        car.velocity = Vector3.zero;
//        car.angularVelocity = Vector3.zero;
//        foreach (WheelCollider wheel in Motors)
//        {
//            wheel.brakeTorque = Mathf.Infinity;
//        }
//    }

//    void OnTriggerStay(Collider other)
//    {
//        if (other.transform == EndArea.transform && !end)
//        {

//            goInitialPos();
//            auto = true;
//            tNextAction = path.Peek().instant;
//            tAutoBeguin = Time.time;
//        }
//    }
//}