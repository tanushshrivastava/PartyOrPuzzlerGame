using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombineButton : MonoBehaviour
{
    [SerializeField] UnityEvent OnButtonActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "CombinedCollider")
        {
            Debug.Log("PRESSED");
            OnButtonActivate?.Invoke();
        }
    }
}
