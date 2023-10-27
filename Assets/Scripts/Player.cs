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

    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip spikeClip;
    [SerializeField] AudioClip dieClip;

    [SerializeField] GameObject projectileObject;

    bool isGrounded;
    public bool isAlive { get; private set; } = true;

    Vector2 rawInput;
    [HideInInspector] public bool isHoldingJumpButton { get; private set; }
    Rigidbody2D rb;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    GameManager gameManager;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        if (!isAlive) return;

        Run();
        FlipSprite();
        IsGrounded();
    }

    void IsGrounded()
    {
        isGrounded = Physics2D.OverlapArea(groundCheckL.position, groundCheckR.position, groundLayers);
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

    void OnFire(InputValue value)
    {
        Instantiate(projectileObject, transform.position, projectileObject.transform.localRotation);
    }

    void OnJump(InputValue value)
    {
        isHoldingJumpButton = value.isPressed;
        if (isGrounded && value.isPressed)
        {
            rb.AddForceY(jumpForce);
            PlaySFX(jumpClip);
        }
    }

    void FlipSprite()
    {
        if (isPlayerMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), transform.localScale.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAlive) return;

        if (collision.gameObject.tag == "Hazard")
        {
            PlaySFX(spikeClip);
            Die();
        }
    }

    public void Die()
    {
        isAlive = false;
        bodyCollider.enabled = false;
        rb.velocity = new Vector2(10f, 10f);
        rb.velocity = new Vector2(0f, rb.velocity.y);
        animator.SetTrigger("isHurt");
        PlaySFX(dieClip);
        gameManager.ProcessPlayerDeath();
    }

    void PlaySFX(AudioClip clip)
    {
        AudioManager.GetInstance().PlaySFX(clip);
    }
}
