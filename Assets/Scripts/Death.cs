using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour
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
		Vector2 pos = transform.position; 
             
             pos.x = Mathf.RoundToInt(-67);
             pos.y = Mathf.RoundToInt(-5);
             
             
             transform.position = pos; 
		
		
        }
    }

    

}
