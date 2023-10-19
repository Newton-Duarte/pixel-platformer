using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.75f;

    Player player;
    int playerLives = 3;

    Coroutine gameOverRoutine;
    Coroutine resetLevelRoutine;

    GameManager instance;

    void Awake()
    {
        ManageSingleton();
        player = FindAnyObjectByType<Player>();
    }

    void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
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
}
