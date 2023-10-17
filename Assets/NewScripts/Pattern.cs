using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Pattern", order = 1)]
public class Pattern : ScriptableObject
{
    [SerializeField] Vector2[] cellLocations;
    public Vector2[] GetCellLocations()
    {
        return cellLocations;
    }
}
