using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public Transform buildPoint;
    public Transform playerTransform;

    public List<buildObjects> objects = new List<buildObjects> ();
    public buildObjects currentObject;
    private Vector3 currentPos;
    public Transform currentPreview;
    public GameObject curPrev;
    public Quaternion objectOrientation;
    private RaycastHit hit;
    private RaycastHit hit2;
    public float offset = 1.0f;
    public float gridSize = 1.0f;
    public bool isBuilding;

    private void Start() 
    {
        currentObject = objects [0];
        ChangeCurrentBuilding();        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("b pressed");
            isBuilding = !isBuilding;

        }
        if (isBuilding)
        {
            StartPreview();
            if (Input.GetButtonDown("Fire1"))
            {
                Build();
            }
        }
        if (!isBuilding)
        {
            curPrev.SetActive(false);
        } else { curPrev.SetActive(true); }
    }

    public void StartPreview()
    {
        if (Physics.Raycast(buildPoint.position, buildPoint.forward, out RaycastHit hit, 15) && hit.transform.tag == "Walkable" || hit.transform.tag == "Buildable")
        {
            if (hit.transform != this.transform)
            {
                ShowPreview(hit);
            }
        }
    }

    public void ShowPreview(RaycastHit hit2)
    {
        if (hit2.transform.tag == "Buildable")
        {
            Debug.Log("Hitting block");
            currentPos = new Vector3(hit2.transform.position.x + hit2.normal.x, hit2.transform.position.y + hit2.normal.y, hit2.transform.position.z + hit2.normal.z);
            Debug.Log(hit2.normal);
            currentPreview.position = currentPos;
            currentPreview.rotation = hit2.transform.rotation;
        } 
        else 
        {
            currentPos = new Vector3(hit2.point.x, hit2.point.y, hit2.point.z);
            currentPreview.position = currentPos;
            currentPreview.rotation = playerTransform.rotation;
        }
    }

    public void Build()
    {
        // PreviewObject PO = currentPreview.GetComponent<PreviewObject>();
        // if (PO.Buildable)
        // {
            Instantiate(currentObject.prefab, currentPos, currentPreview.rotation);
        // }
    }

    public void ChangeCurrentBuilding()
    {
        //Quaternion objectOrientation = playerTransform.rotation;
        curPrev = Instantiate (currentObject.preview, currentPos, Quaternion.identity) as GameObject;
        currentPreview = curPrev.transform;
    }
}




[System.Serializable]
public class buildObjects
{
    public string name;
    public GameObject prefab;
    public GameObject preview;
    public int gold;
}