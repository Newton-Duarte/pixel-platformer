using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.75f;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] AudioClip bonusLivesClip;

    Player player;
    int playerLives = 3;
    int coins = 0;

    Coroutine gameOverRoutine;
    Coroutine resetLevelRoutine;

    void Awake()
    {
        ManageSingleton();
        player = FindAnyObjectByType<Player>();
    }

    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        livesText.text = $"x{playerLives}";
        coinsText.text = $"x{coins}";
    }

    public void AddCoins(int value)
    {
        coins += value;

        if (coins >= 100)
        {
            playerLives++;
            coins = 0;
            PlaySFX(bonusLivesClip);
        }

        UpdateUI();
    }

    void ManageSingleton()
    {
        int numOfGameManager = FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length;
        if (numOfGameManager > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            playerLives--;
            ResetLevel();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (gameOverRoutine != null)
        {
            StopCoroutine(LoadGameOver());
        }

        gameOverRoutine = StartCoroutine(LoadGameOver());
    }

    IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene("GameOver");
    }

    void ResetLevel()
    {
        if (resetLevelRoutine != null)
        {
            StopCoroutine(LoadResetLevel());
        }

        resetLevelRoutine = StartCoroutine(LoadResetLevel());
    }

    IEnumerator LoadResetLevel()
    {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PlaySFX(AudioClip clip)
    {
        AudioManager.GetInstance().PlaySFX(clip);
    }
}
