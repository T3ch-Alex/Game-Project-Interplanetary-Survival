using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    public LayerMask whatIsGround;

    [Header("Celestial Bodies Drag")]
    public float planetStarterGroundDrag;
    public float planetStarterAirDrag;
    public float planetStarterAngularDrag;
    public float moonStarterGroundDrag;
    public float moonStarterAirDrag;
    public float moonStarterAngularDrag;


    [Header("Body Data")]
    public float yLocalVelocity;
    public float bodyHeight;


    [Header("Body State")]
    public bool bodyIsOnPlanetStarter;
    public bool bodyIsOnMoonStarter;
    public bool bodyIsOnSpace;
    public bool bodyIsFreeFalling;
    public bool bodyIsGoingUp;
    public bool bodyIsGrounded;


    [Header("Rigid Bodies and Colliders")]
    public Rigidbody body;
    RaycastHit hit;



    private void Start() 
    {
        body = GetComponent<Rigidbody>();
        bodyHeight = body.transform.lossyScale.y;
    }

    private void FixedUpdate() 
    {
        ApplyDrag();
    }

    private void Update() 
    {
        bodyIsGrounded = Physics.Raycast(body.transform.position, -body.transform.up, out hit, bodyHeight / 2 + 0.2f, whatIsGround);

        //Stoping yLocalVelocity from ramdomly changing small values while grounded
        if (bodyIsGrounded && yLocalVelocity < 0)
        {
            yLocalVelocity = 0;
        }
    }




    // private void OnCollisionEnter(Collision collision) 
    // {
    //     if (collision.gameObject.tag == "Ground")
    //     {
    //         bodyIsGrounded = true;
    //     }
    // }

    // private void OnCollisionExit(Collision collision) 
    // {
    //     if (collision.gameObject.tag == "Ground")
    //     {
    //         bodyIsGrounded = false;
    //     }
    // }




    private void OnTriggerEnter(Collider collision) 
    {
        //Planet Starter gravity sphere collision
        if (collision.gameObject.tag == "Planet Starter")
        {
            bodyIsOnPlanetStarter= true;
        } else { bodyIsOnPlanetStarter = false; }

        //Moon Starter gravity sphere collision
        if (collision.gameObject.tag == "Moon Starter")
        {
            bodyIsOnMoonStarter = true;
        } else { bodyIsOnMoonStarter = false; }
    }
    private void OnTriggerExit(Collider collision) 
    {
        //Planet Starter gravity sphere collision
        if (collision.gameObject.tag == "Planet Starter")
        {
            bodyIsOnPlanetStarter = false;
            bodyIsOnSpace = true;
        }

        //Moon Starter gravity sphere collision
        if (collision.gameObject.tag == "Moon Starter")
        {
            bodyIsOnMoonStarter = false;
            bodyIsOnSpace = true;
        }
    }


    void ApplyDrag()
    {
        yLocalVelocity = transform.InverseTransformDirection(body.velocity).y;

        //Planet Starter Drag
        if (bodyIsOnPlanetStarter == true)
        {
            if (yLocalVelocity < 0)
            {
                bodyIsFreeFalling = true;
                body.drag = planetStarterAirDrag;
                body.angularDrag = planetStarterAngularDrag;
            } else { bodyIsFreeFalling = false; } 

            if (yLocalVelocity > 0)
            {
                bodyIsGoingUp = true;
                body.drag = planetStarterAirDrag;
                body.angularDrag = planetStarterAngularDrag;
            } else { bodyIsGoingUp = false; }

            if (bodyIsGrounded)
            {
                bodyIsFreeFalling = false;
                body.drag = planetStarterGroundDrag;
                body.angularDrag = planetStarterAngularDrag;
            }
        }

        //Moon Starter Drag
        if (bodyIsOnMoonStarter == true)
        {
            if (yLocalVelocity < 0)
            {
                bodyIsFreeFalling = true;
                body.drag = moonStarterAirDrag;
                body.angularDrag = moonStarterAngularDrag;
            } else { bodyIsFreeFalling = false; } 

            if (yLocalVelocity > 0)
            {
                bodyIsGoingUp = true;
                body.drag = moonStarterAirDrag;
                body.angularDrag = moonStarterAngularDrag;
            } else { bodyIsGoingUp = false; }

            if (bodyIsGrounded)
            {
                bodyIsFreeFalling = false;
                body.drag = moonStarterGroundDrag;
                body.angularDrag = moonStarterAngularDrag;
            }
        }

        //Space Drag
        if (bodyIsOnSpace == true)
        {
            bodyIsFreeFalling = true;
            bodyIsGrounded = false;
            body.drag = 0f;
            body.angularDrag = 0f;
        }
    }
}