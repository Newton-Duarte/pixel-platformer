using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] AudioClip clip;

    Player player;
    Rigidbody2D rb;
    float xSpeed;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        rb = GetComponent<Rigidbody2D>();
        xSpeed = player.transform.localScale.x * speed;
    }

    void Update()
    {
        rb.velocity = new Vector2(xSpeed, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
