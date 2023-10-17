using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
    [SerializeField] UnityEvent OnSceneStart;

    private void Start()
    {
        OnSceneStart?.Invoke();
    }
}
