using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] Transform headPoint;
    [SerializeField] AudioClip hitClip;

    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        float newScaleX = Mathf.Sign(moveSpeed) * Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector2(-newScaleX, transform.localScale.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        float headPointY = headPoint.position.y;
        float collisionPointY = collision.GetContact(0).point.y;
        bool collidesOnHead = collisionPointY > headPointY;

        if (collidesOnHead)
        {
            var collisionRb = collision.gameObject.GetComponent<Rigidbody2D>();
            collisionRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            Die();
        }
        else
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.isAlive)
            {
                collision.gameObject.GetComponent<Player>().Die();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) return;

        moveSpeed = -moveSpeed;
        Flip();
    }

    void Die()
    {
        moveSpeed = 0;
        animator.SetTrigger("Die");
        AudioManager.GetInstance().PlaySFX(hitClip);
        Destroy(gameObject, 0.25f);
    }
}
