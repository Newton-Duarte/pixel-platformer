using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float jumpForce = 600f;
    [SerializeField] Transform groundCheckL;
    [SerializeField] Transform groundCheckR;
    [SerializeField] LayerMask groundLayers;

    bool isGrounded;

    Vector2 rawInput;
    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Run();
        FlipSprite();
        IsGrounded();
    }

    void IsGrounded()
    {
        isGrounded = Physics2D.OverlapArea(groundCheckL.position, groundCheckR.position, groundLayers);
        Debug.Log(isGrounded);
        animator.SetBool("isGrounded", isGrounded);
    }

    bool isPlayerMoving => Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

    void Run()
    {
        rb.velocity = new Vector2(rawInput.x * moveSpeed, rb.velocity.y);
        animator.SetInteger("SpeedX", (int)rawInput.x);
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            rb.AddForceY(jumpForce);
        }
    }

    void FlipSprite()
    {
        if (isPlayerMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), transform.localScale.y);
        }
    }
}
