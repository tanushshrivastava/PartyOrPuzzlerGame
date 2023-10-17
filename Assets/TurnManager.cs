using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public int numberOfPlayers;
    [SerializeField] Timer turnTimer;
    [SerializeField] Timer pauseTimer;
    [SerializeField] GridManager grid;
    [SerializeField] PlayerUIManager playerUIManager;
    [SerializeField] UnityEvent OnGameEnd;

    float timerLength;
    float currentTimerLength;
    int currentPlayer;
    List<int> eliminatedPlayers = new List<int>();

    private void Awake()
    {
        timerLength = turnTimer.GetTimerLength();
        currentTimerLength = timerLength;
    }

    private void Start()
    {
        //StartGame();
    }

    public void StartGame()
    {
        grid.paused = false;
        grid.StartGame();
        turnTimer.ChangeTimerLength(timerLength);
        turnTimer.RestartTimer();
    }

    public void PatternSolvedEndTurn()
    {
        pauseTimer.RestartTimer();
        grid.paused = true;
        turnTimer.PauseTimer();
        pauseTimer.OnTimerEnd.AddListener(EndTurn);
    }

    public void TimerFinishedEndTurn()
    {
        eliminatedPlayers.Add(currentPlayer);
        playerUIManager.EliminatePlayer(currentPlayer);
        if(eliminatedPlayers.Count >= numberOfPlayers - 1)
        {
            grid.paused = true;
            OnGameEnd?.Invoke();
        }
        else
        {
            pauseTimer.RestartTimer();
            grid.paused = true;
            turnTimer.PauseTimer();
            pauseTimer.OnTimerEnd.AddListener(EndTurn);
        }
    }

    void EndTurn()
    {
        currentPlayer++;
        List<int> playerIndices = new List<int> {0, 1, 2};
        foreach (int index in eliminatedPlayers)
        {
            playerIndices.Remove(index);
        }
        int highest = -1;
        for (int index = 0; index < playerIndices.Count; index++)
        {
            if(playerIndices[index] > highest)
            {
                highest = playerIndices[index];
            }
        }
        if (currentPlayer > highest && currentTimerLength > 10)
        {
            currentTimerLength -= 4;
            turnTimer.ChangeTimerLength(currentTimerLength);
            currentPlayer %= numberOfPlayers;
        }

        while (eliminatedPlayers.Contains(currentPlayer))
        {
            currentPlayer++;
            currentPlayer %= numberOfPlayers;
        }

        playerUIManager.SetPlayerTurn(currentPlayer);
        turnTimer.RestartTimer();
        pauseTimer.OnTimerEnd.RemoveAllListeners();
        grid.StartGame();
        grid.paused = false;
    }

    public void RestartGame()
    {
        currentTimerLength = timerLength;
        eliminatedPlayers.Clear();
        currentPlayer = 0;
        StartGame();
    }
}
