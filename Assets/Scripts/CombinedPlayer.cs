using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedPlayer : MonoBehaviour
{
    [SerializeField] Player leftPlayer;
    [SerializeField] Player rightPlayer;
    [SerializeField] GameObject combinedCollider;
    [SerializeField] KeyCode combine;
    [SerializeField] KeyCode uncombine;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float combineDistance = 1f;
    [SerializeField] float uncombineForce = 0.2f;

    Rigidbody2D rgbd;

    Vector2 moveVector = Vector2.zero;
    bool combined = false;

    private void Awake()
    {
        rgbd = combinedCollider.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ProcessInput();
        if (combined)
        {
            Move();
        }
    }

    private void ProcessInput()
    {
        bool isLeftKeyPressed = Input.GetKey(left);
        bool isRightKeyPressed = Input.GetKey(right);
        bool combinePressed = Input.GetKey(combine);
        bool uncombinePressed = Input.GetKey(uncombine);
        if(combinePressed && !combined)
        {
            bool leftIsLeftAndRightIsRight = leftPlayer.transform.position.x <= rightPlayer.transform.position.x;
            bool canCombine = Vector2.Distance(leftPlayer.gameObject.transform.position, rightPlayer.gameObject.transform.position) 
                                                <= combineDistance && leftIsLeftAndRightIsRight;
            if(canCombine){ Combine(); }
        }
        else if(uncombinePressed && combined)
        {
            Uncombine();
        }

        moveVector = Vector2.zero;
        if(isLeftKeyPressed)
        {
            moveVector += Vector2.left;
        }
        if(isRightKeyPressed)
        {
            moveVector += Vector2.right;
        }
    }

    void Combine()
    {
        combined = true;

        leftPlayer.Freeze();
        rightPlayer.Freeze();

        float newY = (leftPlayer.transform.position.y + rightPlayer.transform.position.y) / 2;

        combinedCollider.transform.position = new Vector2(leftPlayer.transform.position.x, newY);

        Vector2 rightPos = new Vector2(leftPlayer.transform.position.x + 0.75f, newY);
        Vector2 leftPos = new Vector2(leftPlayer.transform.position.x - 0.75f, newY);
        rightPlayer.transform.position = rightPos;
        leftPlayer.transform.position = leftPos;

        leftPlayer.transform.SetParent(combinedCollider.transform);
        rightPlayer.transform.SetParent(combinedCollider.transform);

        rgbd.simulated = true;

    }

    void Uncombine()
    {
        combined = false;

        rgbd.simulated = false;

        leftPlayer.transform.SetParent(transform);
        rightPlayer.transform.SetParent(transform);

        leftPlayer.Unfreeze();
        rightPlayer.Unfreeze();

        Vector2 upLeft = (Vector2.up + Vector2.up + Vector2.left).normalized * uncombineForce;
        leftPlayer.Boost(upLeft, ForceMode2D.Impulse);

        Vector2 upRight = (Vector2.up + Vector2.up + Vector2.right).normalized * uncombineForce;
        rightPlayer.Boost(upRight, ForceMode2D.Impulse);
    }

    void Move()
    {
        transform.position = (Vector2)transform.position +  moveVector * Time.deltaTime * moveSpeed;
    }

}
