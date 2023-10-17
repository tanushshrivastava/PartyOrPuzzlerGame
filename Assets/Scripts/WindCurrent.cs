using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCurrent : MonoBehaviour
{
    [SerializeField] float strength = 1f;
    [SerializeField] Vector2 direction = Vector2.up;

    void OnTriggerStay2D(Collider2D collision)
    {
        IBoostable boostable;
        bool isBoostable = collision.TryGetComponent(out boostable);
        if(isBoostable)
        {
            boostable.Boost(direction * strength, ForceMode2D.Force);
        }
    }
}
