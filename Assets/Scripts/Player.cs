using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IBoostable
{
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] LayerMask collisionMask;
    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D rgbd;
    Vector2 moveVector;
    bool canMove = true;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(6, 6, true);
        Physics2D.IgnoreLayerCollision(7, 7, true);
    }

    void Update()
    {
        ProcessInput();
        if(canMove)
        {
            Move();
        }
    }

    void ProcessInput()
    {
        bool leftPressed = Input.GetKey(left);
        bool rightPressed = Input.GetKey(right);
        moveVector = Vector2.zero;
        if (leftPressed)
        {
            moveVector.x -= 1;
        }

        if (rightPressed)
        {
            moveVector.x += 1;
        }
        moveVector.Normalize();
    }

    void Move()
    {
        Vector2 currentPos = transform.position;
        Vector2 newPos = currentPos + moveVector * moveSpeed * Time.deltaTime;
        transform.position = newPos;
    }

    public void Freeze()
    {
        rgbd.simulated = false;
        //GetComponent<Collider2D>().enabled = false;
        canMove = false;
        GetComponent<Animator>().enabled = false;
    }

    public void Unfreeze()
    {
        rgbd.simulated = true;
        //GetComponent<Collider2D>().enabled = true;
        canMove = true;
        GetComponent<Animator>().enabled = true;
    }

    public void Boost(Vector2 force, ForceMode2D forceMode)
    {
        rgbd.AddForce(force,forceMode);
    }
}
