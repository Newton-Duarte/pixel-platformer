using UnityEngine;

public class Collectible : MonoBehaviour, ICollectible
{
    [SerializeField] ECollectibleType type;
    [SerializeField] int value = 1;
    [SerializeField] AudioClip pickupClip;

    GameManager gameManager;


    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    public void Collect()
    {
        switch (type)
        {
            case ECollectibleType.Coin:
            case ECollectibleType.Gem:
                CollectCoin();
                break;
            case ECollectibleType.Key:
                CollectKey();
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Collect();
        }
    }

    void CollectCoin()
    {
        gameManager.AddCoins(value);
        AudioManager.GetInstance().PlaySFX(pickupClip);
        Destroy(gameObject);
    }

    void CollectKey()
    {
        gameManager.AddKeys(value);
        AudioManager.GetInstance().PlaySFX(pickupClip);
        Destroy(gameObject);
    }
}
