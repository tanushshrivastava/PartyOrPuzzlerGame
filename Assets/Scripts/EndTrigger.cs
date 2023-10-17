using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndTrigger : MonoBehaviour
{
    [SerializeField] Team team;
    [SerializeField] UnityEvent OnPlayerContact;
    int players = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player;
        bool isPlayer = collision.gameObject.TryGetComponent(out player);
        if(isPlayer)
        {
            players++;
            if (players == 2)
            {
                GameManager.EndLevel(team);
                OnPlayerContact?.Invoke();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Player player;
        bool isPlayer = collision.gameObject.TryGetComponent(out player);
        if(isPlayer)
        {
            players -= 1;
        }
    }

}
