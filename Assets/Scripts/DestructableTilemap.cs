using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DestructableTilemap : MonoBehaviour
{
    Tilemap tilemap;
    GameManager gameManager;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && gameManager.GetKeys() > 0)
        {
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in  collision.contacts)
            {
                hitPosition.x = hit.point.x + 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y + 0.01f * hit.normal.y;
                tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                gameManager.AddKeys(-1);
            }
        }
    }
}
