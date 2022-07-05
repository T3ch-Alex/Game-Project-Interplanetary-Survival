using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{  
    [Header("Player Movement")]
    public float mouseSensitivityX = 1;
	public float mouseSensitivityY = 1;
	public float walkSpeed = 8;
	public float jumpForce = 220;
    public float jumpMovMultiplier = 1;
    public float speedModifier = 1;
    Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
    Vector3 moveDir;
    Vector3 limitedVel;
    float horizontalInput;
    float verticalInput;
	float verticalLookRotation;
	public Transform cameraTransform;
    public Transform orientation;
	public Rigidbody playerRb;




    [Header("Player Location")]
    public bool playerOnPlanetStarter;
    public bool playerOnMoonStarter;
    public bool playerOnSpace;
    public bool playerIsFreeFalling;
    public bool playerIsGoingUp;
    public bool playerJumped;




    [Header("Player Gravity")]
    public GameObject PlanetStarter;
    public Transform PlanetStarterCenter;
    public Transform MoonStarterCenter;
    public Transform currentGravityCenter;
    Vector3 gravityDirection;
    public float gravityAcceleration;
    public float spaceAcceleration = 0f;
    public float gravityPlanetStarter = 9.81f;
    public float gravityMoonStarter = 1.62f;
    public bool autoOrient = false;
    public float autoOrientSpeed = 10f;
    public bool gravitySwitch;




    [Header("Ground Check")]
    public bool grounded;
    public bool onMovingBody;
    public float groundDistance;
	public LayerMask whatIsGround;
    public LayerMask whatIsPickupable;
    public float playerHeight;
    public float yLocalVelocity;
    public float yLocalPosition;
	



    [Header("Player Drag")]
    public float planetStarterGroundDrag;
    public float planetStarterAirDrag;
    public float planetStarterAngularDrag;
    public float moonStarterGroundDrag;
    public float moonStarterAirDrag;
    public float moonStarterAngularDrag;




    [Header("Slope Management")]
    [SerializeField] public float slopeForce;
    [SerializeField] public float slopeForceRayLength;
    [SerializeField] public bool playerOnSlope;
    public float slopeAngle;
    RaycastHit slopeNormal;




	void Start() 
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		cameraTransform = Camera.main.transform;

        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
		playerRb = GetComponent<Rigidbody>();
        playerHeight = playerRb.transform.lossyScale.y;

        currentGravityCenter = null;
        gravitySwitch = true;
	}
	
	void Update() 
    {        
        //Look rotation:
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation,-60,60);
		cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;




        //Movement:
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded) 
        {
            playerRb.AddForce(moveDir.normalized * walkSpeed, ForceMode.Force);
        }

        if (!grounded)
        {
            playerRb.AddForce(moveDir.normalized * walkSpeed * jumpMovMultiplier, ForceMode.Force);
        }




        //Ground check:
        if (Physics.CheckSphere(playerRb.position, 1 + 0.5f, whatIsGround))
        {
            grounded = true;
        } else if (Physics.CheckSphere(playerRb.position, 1 + 0.5f, whatIsPickupable)) { grounded = true; onMovingBody = true; } else { grounded = false; onMovingBody = false; }
        Debug.DrawRay(playerRb.transform.position, -gravityDirection, Color.green);





        //Stoping yLocalVelocity from ramdomly changing small values while grounded
        if (grounded && yLocalVelocity < 0)
        {
            yLocalVelocity = 0;
        }

        if (Input.GetButton("Jump")) {
			if (grounded) {
                grounded = false;
				playerRb.AddForce(transform.up * jumpForce);
                playerJumped = true;
			}
            else { playerJumped = false; }
		}



        //Velocity Control
        Vector3 flatVel = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        if (flatVel.magnitude > walkSpeed)
        {
            limitedVel = flatVel.normalized * walkSpeed;
            playerRb.velocity = new Vector3(limitedVel.x, playerRb.velocity.y, limitedVel.z);
        }
	}
	



	void FixedUpdate() 
    {
        ApplyDrag();
        ApplyGravity();




        //Slope check
        if (OnSlope() && playerIsGoingUp)
        {
            playerRb.AddForce(GetSlopeMoveDirection() * walkSpeed * slopeForce, ForceMode.Force);
        }




        //Trying to get a local Y height from the center of the planet to the player (no success) :( keeps getting same odd value like 1.5 
        RaycastHit yLocalPlanetHeight;
        Ray yPlayerHeight = new Ray(playerRb.transform.position, currentGravityCenter.transform.position);
        if(Physics.Raycast(yPlayerHeight, out yLocalPlanetHeight))
        {
            yLocalPosition = yLocalPlanetHeight.distance;
        }
	}



    //Slope Management
    private bool OnSlope()
    {
        if (playerJumped && playerIsGoingUp)
        {
            playerOnSlope = false;
            return false;
        }

        if (Physics.Raycast(playerRb.transform.position, -playerRb.transform.up, out slopeNormal, (1 + 0.2f) * slopeForceRayLength, whatIsGround))
        {
            if (slopeNormal.normal  != -playerRb.transform.up)
            {
                playerOnSlope = true;
                return true;
            }
        }
        playerOnSlope = false;
        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeNormal.normal).normalized;
    }



    //Gravity wells
    private void OnTriggerEnter(Collider collision) 
    {
        //Planet Starter gravity sphere collision
        if (collision.gameObject.tag == "Planet Starter Gsphere" && gravitySwitch == true)
        {
            PlanetStarterCenter = GameObject.FindGameObjectWithTag("Planet Starter").GetComponent<Transform>();
            playerOnPlanetStarter= true;
            playerRb.velocity = Vector3.zero;
            currentGravityCenter = PlanetStarterCenter;
            gravityAcceleration = gravityPlanetStarter;
        } else { 
            playerOnPlanetStarter = false;}

        //Moon Starter gravity sphere collision
        if (collision.gameObject.tag == "Moon Starter")
        {
            //MoonStarterCenter = GameObject.FindGameObjectWithTag("Moon Starter").GetComponent<Transform>();
            playerOnMoonStarter = true;
            playerRb.velocity = Vector3.zero;
            gravityAcceleration = gravityMoonStarter;
            currentGravityCenter = MoonStarterCenter;
        } else { 
            playerOnMoonStarter = false; }
    }
    private void OnTriggerExit(Collider collision) 
    {
        //Planet Starter gravity sphere collision
        if (collision.gameObject.tag == "Planet Starter Gsphere" && gravitySwitch == true)
        {
            playerOnPlanetStarter = false;
            playerOnSpace = true;
            playerRb.velocity = Vector3.zero;
            currentGravityCenter = null;
            gravityAcceleration = 0;
        }

        //Moon Starter gravity sphere collision
        if (collision.gameObject.tag == "Moon Starter")
        {
            playerOnMoonStarter = false;
            playerOnSpace = true;
            playerRb.velocity = Vector3.zero;
            currentGravityCenter = null;
            gravityAcceleration = 0;
        }
    }

    


    //Drag Control
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

            if (grounded)
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

            if (grounded)
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
            grounded = false;
            playerRb.drag = 0f;
            playerRb.angularDrag = 0f;
        }
    }




    //Gravity Direction
    private void ApplyGravity()
    {
        gravityDirection = Vector3.zero;
        gravityDirection = (playerRb.transform.position - currentGravityCenter.transform.position);
        playerRb.AddForce(-gravityDirection.normalized * gravityAcceleration * 2f, ForceMode.Acceleration);

        if (autoOrient)
        {
            AutoOrient(-gravityDirection);
        }
    }
    private void AutoOrient(Vector3 down)
    {
        Quaternion orientationDirection = Quaternion.FromToRotation(-playerRb.transform.up, down) * playerRb.transform.rotation;
        playerRb.transform.rotation = Quaternion.Slerp(playerRb.transform.rotation, orientationDirection, autoOrientSpeed * Time.deltaTime);
    }
}
