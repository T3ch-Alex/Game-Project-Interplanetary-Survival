using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingButton : MonoBehaviour
{
    public Craft recipe;
    public InventoryManager manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<InventoryManager>();
    }

    public void CraftItem()
    {
        manager.CraftItem(recipe.IDs, recipe.IDsAmounts, recipe.outcome, recipe.outcomeAmount);
    }
}
