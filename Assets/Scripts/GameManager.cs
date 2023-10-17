using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    static int currentLevel = 1;
    static int team1Wins = 0;
    static int team2Wins = 0;

    public static void EndLevel(Team team)
    {
        if(team == Team.teamOne)
        {
            team1Wins++;
        }
        else if(team == Team.teamTwo)
        {
            team2Wins++;
        }
        else
        {
            Debug.LogError("NO TEAM DETECTED! Received: " + team);
        }
        currentLevel++;
    }
}
