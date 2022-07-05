using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Unity Inventory", order = 0)]
public class Item : ScriptableObject {
    public int ID;
    public new string name;
    public Sprite icon;

    public int maxStack;
}
