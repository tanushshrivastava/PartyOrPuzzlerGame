using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] TurnManager turnManager;
    [SerializeField] GameObject playerUIPrefab;
    [SerializeField] RectTransform playerUIContainer;
    [SerializeField] Text winner;
    List<GameObject> playerUIs;
    List<int> eliminatedPlayers = new List<int>();
    string[] playerNames = { "Player 1", "Player 2", "Player 3" };
    public InputField[] playerInputFields;

    private void Start()
    {
        playerUIs = new List<GameObject>();
    }

    public void Restart()
    {
        eliminatedPlayers.Clear();
        foreach (GameObject player in playerUIs)
        {
            player.GetComponent<Image>().color = Color.white;
            player.GetComponentInChildren<Text>().color = Color.white;
        }
        SetPlayerTurn(0);
    }

    public void EliminatePlayer(int index)
    {
        GameObject player = playerUIs[index];
        player.GetComponent<Image>().color = Color.gray;
        player.GetComponentInChildren<Text>().color = Color.gray;
        eliminatedPlayers.Add(index);
    }

    public void SetPlayerTurn(int index)
    {
        for (int i=0; i < playerUIs.Count; i++)
        {
            GameObject player = playerUIs[i];
            if(!(eliminatedPlayers.Contains(i)))
            {
                player.GetComponent<Image>().color = Color.white;
            }
        }
        playerUIs[index].GetComponent<Image>().color = Color.green;
    }

    public void InstantiatePlayerIcons()
    {
        int numPlayers = turnManager.numberOfPlayers;
        for (int i = 0; i < numPlayers; i++)
        {
            GameObject icon = Instantiate(playerUIPrefab, playerUIContainer);
            playerUIs.Insert(0, icon);
            RectTransform iconTransform = icon.GetComponent<RectTransform>();
            iconTransform.anchorMax = new Vector2(0.5f, 0f);
            iconTransform.pivot = new Vector2(0.5f, 0f);
            iconTransform.anchoredPosition = new Vector2(0f, iconTransform.anchoredPosition.y + i * 100);
            icon.GetComponentInChildren<Text>().text = playerNames[(playerNames.Length)-i-1];
        }
        SetPlayerTurn(0);
    }

    public void SetPlayer1Name()
    {
        playerNames[0] = playerInputFields[0].text;
    }

    public void SetPlayer2Name()
    {
        playerNames[1] = playerInputFields[1].text;
    }

    public void SetPlayer3Name()
    {
        playerNames[2] = playerInputFields[2].text;
    }

    public void SetWinner()
    {
        List<int> indices = new List<int>{0, 1, 2 };
        for (int index = 0; index < eliminatedPlayers.Count; index++)
        {
            indices.Remove(eliminatedPlayers[index]);
        }
        string winnerPerson = playerNames[indices[0]];
        winner.text = winnerPerson + " Wins!";
    }

}
