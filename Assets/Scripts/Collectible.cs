using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] int score = 1;
    [SerializeField] AudioClip gemClip;

    GameManager gameManager;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Collect()
    {
        gameManager.AddCoins(score);
        AudioManager.GetInstance().PlaySFX(gemClip);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collect();
        }
    }
}
