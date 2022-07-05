using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour
{
    public bool Foundation;
    public List<Collider> col = new List<Collider>();
    public Material red;
    public Material green;
    public bool Buildable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Buildable" && Foundation)
        {
            col.Add (other);
            Debug.Log("col added");
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Buildable" && Foundation)
        {
            col.Remove (other);
            Debug.Log("col removed");
        }
    }

    public void ChangeColor()
    {
        if (col.Count == 0)
        {
            Buildable = true;
        } else { Buildable = false; }

        if (Buildable)
        {
            // foreach(Transform child in this.transform)
            // {
                this.GetComponent<Renderer>().material = green;
            // }
        } else {
            // foreach(Transform child in this.transform)
            // {
                this.GetComponent<Renderer>().material = red;
        //     }
        }
    }
}
