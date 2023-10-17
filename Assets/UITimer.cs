using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField] Timer LinkedTimer;
    [SerializeField] Text textBox;
    [SerializeField] bool printLabel = true;
    private void Awake()
    {
        LinkedTimer.OnTimerUpdate += UpdateTextBox;
    }

    private void UpdateTextBox(float currentTime)
    {
        if(printLabel)
        {
            textBox.text = "Time Left: " + currentTime.ToString("F1");
        }
        else
        {
            textBox.text = currentTime.ToString("F1");
        }
    }
}
