using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public LayerMask whatIsGround;


    [Header("Celestial Bodies Drag")]
    public float planetStarterGroundDrag;
    public float planetStarterAirDrag;
    public float planetStarterAngularDrag;
    public float moonStarterGroundDrag;
    public float moonStarterAirDrag;
    public float moonStarterAngularDrag;


    [Header("player Data")]
    public float yLocalVelocity;
    public float playerHeight;


    [Header("player State")]
    public bool playerOnPlanetStarter;
    public bool playerOnMoonStarter;
    public bool playerOnSpace;
    public bool playerIsFreeFalling;
    public bool playerIsGoingUp;
    public bool playerIsGrounded;


    [Header("Rigid Bodies and Colliders")]
    public Rigidbody playerRb;
    public MeshCollider playerMeshCollider;
    RaycastHit hit;


    private void Start() 
    {
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        playerMeshCollider = GameObject.Find("Player").GetComponent<MeshCollider>();
        playerHeight = playerRb.transform.lossyScale.y;
    }

    private void FixedUpdate() 
    {
        ApplyDrag();
    }

    private void Update() 
    {
        playerIsGrounded = Physics.Raycast(playerRb.transform.position, -playerRb.transform.up, out hit, playerHeight / 2 + 0.6f, whatIsGround);

        //Stoping yLocalVelocity from ramdomly changing small values while grounded
        if (playerIsGrounded && yLocalVelocity < 0)
        {
            yLocalVelocity = 0;
        }
        if (playerIsGrounded && yLocalVelocity > 0 && yLocalVelocity < 0.001)
        {
            yLocalVelocity = 0;
        }
    }




    // private void OnCollisionEnter(Collision collision) 
    // {
    //     if (collision.gameObject.tag == "Ground")
    //     {
    //         playerIsGrounded = true;
    //     }
    // }

    // private void OnCollisionExit(Collision collision) 
    // {
    //     if (collision.gameObject.tag == "Ground")
    //     {
    //         playerIsGrounded = false;
    //     }
    // }




    private void OnTriggerEnter(Collider collision) 
    {
        //Planet Starter gravity sphere collision
        if (collision.gameObject.tag == "Planet Starter")
        {
            playerOnPlanetStarter= true;
        } else { playerOnPlanetStarter = false; }

        //Moon Starter gravity sphere collision
        if (collision.gameObject.tag == "Moon Starter")
        {
            playerOnMoonStarter = true;
        } else { playerOnMoonStarter = false; }
    }
    private void OnTriggerExit(Collider collision) 
    {
        //Planet Starter gravity sphere collision
        if (collision.gameObject.tag == "Planet Starter")
        {
            playerOnPlanetStarter = false;
            playerOnSpace = true;
        }

        //Moon Starter gravity sphere collision
        if (collision.gameObject.tag == "Moon Starter")
        {
            playerOnMoonStarter = false;
            playerOnSpace = true;
        }
    }


    void ApplyDrag()
    {
        yLocalVelocity = transform.InverseTransformDirection(playerRb.velocity).y;

        //Planet Starter Drag
        if (playerOnPlanetStarter == true)
        {
            if (yLocalVelocity < 0)
            {
                playerIsFreeFalling = true;
                playerRb.drag = planetStarterAirDrag;
                playerRb.angularDrag = planetStarterAngularDrag;
            } else { playerIsFreeFalling = false; } 

            if (yLocalVelocity > 0)
            {
                playerIsGoingUp = true;
                playerRb.drag = planetStarterAirDrag;
                playerRb.angularDrag = planetStarterAngularDrag;
            } else { playerIsGoingUp = false; }

            if (playerIsGrounded)
            {
                playerIsFreeFalling = false;
                playerRb.drag = planetStarterGroundDrag;
                playerRb.angularDrag = planetStarterAngularDrag;
            }
        }

        //Moon Starter Drag
        if (playerOnMoonStarter == true)
        {
            if (yLocalVelocity < 0)
            {
                playerIsFreeFalling = true;
                playerRb.drag = moonStarterAirDrag;
                playerRb.angularDrag = moonStarterAngularDrag;
            } else { playerIsFreeFalling = false; } 

            if (yLocalVelocity > 0)
            {
                playerIsGoingUp = true;
                playerRb.drag = moonStarterAirDrag;
                playerRb.angularDrag = moonStarterAngularDrag;
            } else { playerIsGoingUp = false; }

            if (playerIsGrounded)
            {
                playerIsFreeFalling = false;
                playerRb.drag = moonStarterGroundDrag;
                playerRb.angularDrag = moonStarterAngularDrag;
            }
        }

        //Space Drag
        if (playerOnSpace == true)
        {
            playerIsFreeFalling = true;
            playerIsGrounded = false;
            playerRb.drag = 0f;
            playerRb.angularDrag = 0f;
        }
    }
}
