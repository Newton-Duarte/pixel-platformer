using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    [SerializeField] Transform jumpArea;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float jumpForceMultiplier = 1.25f;
    [SerializeField] AudioClip jumpClip;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float jumpAreaY = jumpArea.position.y;
        float collisionPointY = collision.GetContact(0).point.y;
        bool collidesFromAbove = collisionPointY > jumpAreaY;

        if (collidesFromAbove)
        {
            float forceY = jumpForce;

            if (Input.GetButton("Jump"))
            {
                forceY *= jumpForceMultiplier;
            }

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * forceY, ForceMode2D.Impulse);
            AudioManager.GetInstance().PlaySFX(jumpClip);
            animator.SetTrigger("Activate");
        }
    }
}
