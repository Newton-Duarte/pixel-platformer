using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float jumpForce = 0.01f; //TODO: The jumpForce is too strong, the player is being pushed to the top
    [SerializeField] Transform headPoint;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(0f, jumpForce);
            Die();
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
        Destroy(gameObject);
    }
}
