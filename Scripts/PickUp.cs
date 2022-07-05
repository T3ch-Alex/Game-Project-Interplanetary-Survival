using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private LayerMask PickupMask;
    [SerializeField] private Camera Camera;
    [SerializeField] private Transform PickupTarget;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform PlayerFeet;
    [Space]
    [SerializeField] private float PickupRange;
    // public float distance;

    public bool isHolding = false;

    private float throwForce = 5f;
    public float distanceFromFeet;

    private Rigidbody CurrentObject;

    void Start()
    {
        CurrentObject.GetComponent<Rigidbody>();
        PickupTarget.GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (CurrentObject)
            {
                isHolding = false;
            }

            if (CurrentObject && isHolding == false)
            {
                CurrentObject.useGravity = true;
                CurrentObject.drag = 0f;
                CurrentObject.angularDrag = 0.05f;
                CurrentObject = null;
                return;
            }

            Ray CameraRay = Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(CameraRay, out RaycastHit HitInfo, PickupRange, PickupMask))
            {
                CurrentObject = HitInfo.rigidbody;
                CurrentObject.useGravity = false;
                CurrentObject.drag = 10f;
                CurrentObject.angularDrag = 10f;
                isHolding = true;
            }
        }

        if (isHolding == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CurrentObject.AddForce(PickupTarget.transform.forward * throwForce, ForceMode.Impulse);
                CurrentObject.useGravity = true;
                CurrentObject.drag = 0f;
                CurrentObject.angularDrag = 0.05f;
                CurrentObject = null;
                isHolding = false;
            }
        }

        float distance = Vector3.Distance(CurrentObject.transform.position, PickupTarget.transform.position);

        if (CurrentObject && distance >= 1f)
        {
            CurrentObject.useGravity = true;
            CurrentObject.drag = 0f;
            CurrentObject.angularDrag = 0.05f;
            CurrentObject = null;
            isHolding = false;
        }

        distanceFromFeet = Vector3.Distance(CurrentObject.transform.position, PlayerFeet.transform.position);

        if (CurrentObject && distanceFromFeet <= 1.5f)
        {
            CurrentObject.useGravity = true;
            CurrentObject.drag = 0f;
            CurrentObject.angularDrag = 0.05f;
            CurrentObject = null;
            isHolding = false;
        }
    }

    void FixedUpdate()
    {
        if (CurrentObject)
        {
            Vector3 DirectionToPoint = PickupTarget.position - CurrentObject.position;
            float DistanceToPoint = DirectionToPoint.magnitude;

            CurrentObject.velocity = DirectionToPoint * 12f * DistanceToPoint;
        }
    }
}
