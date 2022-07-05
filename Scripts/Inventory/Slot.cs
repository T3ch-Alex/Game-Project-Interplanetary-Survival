using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int ID;
    public InventoryManager manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<InventoryManager>();
    }

    private void Update() 
    {
        if (transform.childCount > 0)
        {
            if (transform.GetChild(0).GetComponent<InventoryItem>().amount <= 0)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }

    public void SetID()
    {
        manager.currentSlot = ID;
        manager.PickupDropInventory();
    }
}
