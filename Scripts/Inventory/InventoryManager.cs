using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform inventorySlotHolder;
    public Transform inventoryHotbarSlotHolder;
    public GameObject InventoryCanvas;
    private bool InventoryToggle;

    public GameObject player;

    public Transform cursor;
    public Vector3 offset;

    public List<bool> isFull;
    public List<Transform> slots;
    public List<Transform> hotbarSlots;

    public int currentSlot;

    private void Start() 
    {
        InventoryCanvas.SetActive(false);
        InitializeInventory();
        CheckSlots();
        SetSlotIDS();
        player.GetComponent<GameObject>();
    }

    private void Update() 
    {
        // Open,close inventory and set cursor to visible, invisible
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InventoryToggle = !InventoryToggle;
            if (InventoryToggle)
            {
                InventoryCanvas.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else{
                InventoryCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        //Make item follow mouse when inventory is active
        if (InventoryCanvas.activeSelf == true)
        {
            cursor.position = Input.mousePosition + offset;
            player.GetComponent<PlayerLook>().enabled = false;
        } else {
            player.GetComponent<PlayerLook>().enabled = true;
        }

        //Only makes the item appear on the cursor when theres one
        if (cursor.childCount > 0)
        {
            cursor.gameObject.SetActive(true);
        } else {
            cursor.gameObject.SetActive(false);
        }
    }

    void InitializeInventory()
    {
        // Set slots
        for (int i = 0; i < inventorySlotHolder.childCount; i++)
        {
            slots.Add(inventorySlotHolder.GetChild(i));
            isFull.Add(false);
        }
        for (int i = 0; i < inventoryHotbarSlotHolder.childCount; i++)
        {
            slots.Add(inventoryHotbarSlotHolder.GetChild(i));
            hotbarSlots.Add(inventoryHotbarSlotHolder.GetChild(i));
            isFull.Add(false);
        }
    }

    void SetSlotIDS()
    {
        for(int i = 0; i < slots.Count; i++) 
        {
            if (slots[i].GetComponent<Slot>() != null)
            {
                slots[i].GetComponent<Slot>().ID = i;
            }
        }
    }

    void CheckSlots()
    {
        // Check if slots are full
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount > 0)
            {
                isFull[i] = true;
            } else {
                isFull[i] = false;
            }
        }
    }

    public void CraftItem(int[] IDs, int[] IDsAmounts, GameObject outcome, int outcomeAmount)
    {
        //Colleting info weather or not item can be crafted
        bool[] collected = new bool[IDs.Length];
        Transform[] collectedSlots = new Transform[IDs.Length];
        for (int x = 0; x < IDs.Length; x++)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (isFull[i] == true)
                {
                    if (slots[i].GetChild(0).GetComponent<InventoryItem>().itemData.ID == IDs[x] && slots[i].GetChild(0).GetComponent<InventoryItem>().amount >= IDsAmounts[x])
                    {
                        collected[x] = true;
                        collectedSlots[x] = slots[i].GetChild(0);
                    }
                }
            }
        }
        for (int i = 0; i < collected.Length; i++)
        {
            if (collected[i] == false)
            {
                return;
            }
        }

        for (int i = 0; i < collectedSlots.Length; i++)
        {
            collectedSlots[i].GetComponent<InventoryItem>().amount -= IDsAmounts[i];
        }

        for (int i = 0; i < outcomeAmount; i++)
        {
            AddItem(outcome);
        }
    }

    void AddItem(GameObject item)
    {
        for(int x = 0; x < slots.Count; x++) 
        {
            if (isFull[x] == false)
            {
                //Add the item
                Instantiate(item, slots[x]);
                CheckSlots();
                return;
            } else {
                Debug.Log("Slot is full");
            }
        }
        Debug.Log("All slots are full");
    }

    public void PickupDropInventory()
    {
        if(slots[currentSlot].childCount > 0 && cursor.childCount < 1)
        {
            //Put inside cursor
            Instantiate(slots[currentSlot].GetChild(0).gameObject, cursor);
            Destroy(slots[currentSlot].GetChild(0).gameObject);
        } else if (slots[currentSlot].childCount < 1 && cursor.childCount > 0){
            Instantiate(cursor.GetChild(0).gameObject, slots[currentSlot]);
            Destroy(cursor.GetChild(0).gameObject);
        } else if (slots[currentSlot].childCount > 0 && cursor.childCount > 0)
        {
            if (slots[currentSlot].GetChild(0).GetComponent<InventoryItem>().itemData.ID == cursor.GetChild(0).GetComponent<InventoryItem>().itemData.ID)
            {
                if (slots[currentSlot].GetChild(0).GetComponent<InventoryItem>().amount <= cursor.GetChild(0).GetComponent<InventoryItem>().itemData.maxStack - slots[currentSlot].GetChild(0).GetComponent<InventoryItem>().amount)
                {
                    slots[currentSlot].GetChild(0).GetComponent<InventoryItem>().amount += cursor.GetChild(0).GetComponent<InventoryItem>().amount;
                    Destroy(cursor.GetChild(0).gameObject);
                }
            }
        }
        CheckSlots();
    }
}
