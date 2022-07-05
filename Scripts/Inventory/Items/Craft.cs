using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Craft", menuName = "Craft", order = 1)]
public class Craft : ScriptableObject 
{
    public int[] IDs;
    public int[] IDsAmounts;
    public GameObject outcome;
    public int outcomeAmount;
}
