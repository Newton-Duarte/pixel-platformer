using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    [SerializeField] float loadLevelDelay = 0.75f;
    [SerializeField] AudioClip exitClip;

    Player player;

    void Awake()
    {
        player = FindAnyObjectByType<Player>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.isAlive) return;

        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        AudioManager.GetInstance().PlaySFX(exitClip);

        yield return new WaitForSeconds(loadLevelDelay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex++;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}
