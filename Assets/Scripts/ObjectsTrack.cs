using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsTrack : MonoBehaviour
{
    [SerializeField] GameObject object1;
    [SerializeField] GameObject object2;
    private void Update()
    {
        Vector2 newPos = CalculateMidpoint();
        transform.position = newPos;
    }

    private Vector2 CalculateMidpoint()
    {
        Vector2 pos1 = object1.transform.position;
        Vector2 pos2 = object2.transform.position;
        float x = (pos1.x + pos2.x) / 2;
        float y = (pos1.y + pos2.y) / 2;
        return new Vector2(x, y);
    }
}
