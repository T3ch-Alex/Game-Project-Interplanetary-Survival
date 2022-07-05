using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public Transform PlanetStarterCenter;
    public Transform MoonStarterCenter;
    public Transform currentGravityCenter;


    Vector3 gravityDirection;
    public float gravityAcceleration;
    public float spaceAcceleration = 0f;
    public float gravityPlanetStarter = 9.81f;
    public float gravityMoonStarter = 1.62f;



    // public float xLocalVelocity;
    // public float yLocalVelocity;
    // public float zLocalVelocity;


    public bool bodyIsOnPlanetStarter;
    public bool bodyIsOnMoonStarter;
    public bool gravitySwitch;


    public Rigidbody body;




    private void Start() 
    {
        body = GetComponent<Rigidbody>();
        currentGravityCenter = null;
        MoonStarterCenter = GameObject.Find("Moon Starter Center").GetComponent<Transform>();
        gravitySwitch = true;
    }
    private void FixedUpdate() 
    {
        ApplyGravity();
    }
    private void Update() 
    {
        // xLocalVelocity = transform.InverseTransformDirection(body.velocity).x;
        // yLocalVelocity = transform.InverseTransformDirection(body.velocity).y;
        // zLocalVelocity = transform.InverseTransformDirection(body.velocity).z;
    }




    private void OnTriggerEnter(Collider collision) 
    {
        //Planet Starter gravity sphere collision
        if (collision.gameObject.tag == "Planet Starter Gsphere" && gravitySwitch == true)
        {
            PlanetStarterCenter = GameObject.FindGameObjectWithTag("Planet Starter").GetComponent<Transform>();
            body.velocity = Vector3.zero;
            bodyIsOnPlanetStarter= true;
            currentGravityCenter = PlanetStarterCenter;
            gravityAcceleration = gravityPlanetStarter;
        } else { bodyIsOnPlanetStarter = false; }

        //Moon Starter gravity sphere collision
        if (collision.gameObject.tag == "Moon Starter")
        {
            body.velocity = Vector3.zero;
            bodyIsOnMoonStarter = true;
            gravityAcceleration = gravityMoonStarter;
            currentGravityCenter = MoonStarterCenter;
        } else { bodyIsOnMoonStarter = false; }
    }
    private void OnTriggerExit(Collider collision) 
    {
        //Planet Starter gravity sphere collision
        if (collision.gameObject.tag == "Planet Starter")
        {
            body.velocity = Vector3.zero;
            bodyIsOnPlanetStarter= false;
            currentGravityCenter = null;
            gravityAcceleration = 0;
        }

        //Moon Starter gravity sphere collision
        if (collision.gameObject.tag == "Moon Starter")
        {
            body.velocity = Vector3.zero;
            bodyIsOnMoonStarter = false;
            currentGravityCenter = null;
            gravityAcceleration = 0;
        }
    }




    private void ApplyGravity()
    {
        gravityDirection = Vector3.zero;
        gravityDirection = (body.transform.position - currentGravityCenter.transform.position);
        body.AddForce(-gravityDirection.normalized * gravityAcceleration, ForceMode.Acceleration);
    }




    public void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(body.transform.position, currentGravityCenter.transform.position);
    }
}
