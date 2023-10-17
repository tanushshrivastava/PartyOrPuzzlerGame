using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] UnityEvent OnStartTimer;
    public UnityEvent OnTimerEnd;
    [SerializeField] float timerLength = 30;
    public event Action<float> OnTimerUpdate;
    float currentTime;
    bool timerStarted = false;
    bool paused = false;

    void StartTimer()
    {
        if(timerStarted)
        {
            return;
        }
        paused = false;
        currentTime = timerLength;
        timerStarted = true;
        OnStartTimer?.Invoke();
    }

    private void Update()
    {
        if(timerStarted && !paused)
        {
            currentTime -= Time.deltaTime;
            OnTimerUpdate?.Invoke(currentTime);
            if(currentTime <= 0)
            {
                currentTime = 0;
                OnTimerUpdate?.Invoke(currentTime);
                TimerEnd();
            }
        }
    }

    private void TimerEnd()
    {
        timerStarted = false;
        OnTimerEnd?.Invoke();
    }

    public void ChangeTimerLength(float newTime)
    {
        timerLength = newTime;
    }

    public float GetTimerLength()
    {
        return timerLength;
    }

    public void RestartTimer()
    {
        timerStarted = false;
        StartTimer();
    }

    public void PauseTimer()
    {
        paused = true;
    }

    public void UnPauseTimer()
    {
        paused = false;
    }

}
