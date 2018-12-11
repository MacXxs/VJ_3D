using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_water : MonoBehaviour {

    private bool collided = false;

    public GameObject fountain;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided && collision.contacts.Length > 0)
        {
            if (collision.relativeVelocity.magnitude > 2)
            {
                Quaternion rot = new Quaternion();
                //el primer valor es el la direccio en la que ha de mirar i el segon es el up
                //com que per defecte el forward d'aquesta particula està mirant cap a les 
                //y negatives, he tingut que invertir aquestes per a que es generés correctament
                rot.SetLookRotation(Vector3.down, Vector3.up);
                Instantiate(fountain, transform.position, rot);
                collided = true;
            }
        }
    }
}
