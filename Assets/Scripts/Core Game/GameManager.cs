using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public Transform player;
    public Transform fixedSpawnPoint;

    [Header("HUD")]
    public TMP_Text timerText;

    [Header("Win Panel")]
    public GameObject winPanel;
    public TMP_Text winTimeText;
    public TMP_Text winCoinText;
    public GameObject highScoreText;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverTimeText;
    public TMP_Text gameOverCoinText;

    [Header("Coins")]
    public GameObject coinPrefab;
    public int coinCount = 25;
    public Vector3 spawnArea = new Vector3(20f, 1f, 20f);

    private float elapsedTime;
    private bool gameEnded;

    private readonly List<GameObject> spawnedCoins = new();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AudioManager.instance?.StopMenuMusic();
        AudioManager.instance?.PlayIdle();

        Time.timeScale = 1f;

        elapsedTime = 0f;
        gameEnded = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        if (highScoreText != null)
            highScoreText.SetActive(false);

        SpawnCoins();
    }

    private void Update()
    {
        if (gameEnded)
            return;

        elapsedTime += Time.deltaTime;

        if (timerText != null)
            timerText.text = "TIME: " + elapsedTime.ToString("F2") + "s";
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }

    public void AddPenaltyTime(float seconds)
    {
        elapsedTime += seconds;
    }

    public void WinGame()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        AudioManager.instance?.StopGameplayAudio();
        AudioManager.instance?.PlayWin();

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CoinCollector collector =
            player.GetComponent<CoinCollector>();

        if (winCoinText != null)
            winCoinText.text =
                "COINS: " +
                collector.coins +
                "/" +
                coinCount;

        if (winTimeText != null)
            winTimeText.text =
                "TIME: " +
                elapsedTime.ToString("F2") +
                "s";

        bool isHighScore =
            HighScoreManager.IsHighScore(elapsedTime);

        if (highScoreText != null)
            highScoreText.SetActive(isHighScore);

        HighScoreManager.SaveScore(elapsedTime);

        winPanel.SetActive(true);

        ThirdPersonController controller =
            player.GetComponent<ThirdPersonController>();

        if (controller != null)
            controller.canMove = false;
    }

    public void TriggerGameOver()
    {
        if (gameEnded)
            return;

        gameEnded = true;

        AudioManager.instance?.StopGameplayAudio();
        AudioManager.instance?.PlayGameOver();

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CoinCollector collector =
            player.GetComponent<CoinCollector>();

        if (gameOverCoinText != null)
            gameOverCoinText.text =
                "COINS: " +
                collector.coins +
                "/" +
                coinCount;

        if (gameOverTimeText != null)
            gameOverTimeText.text =
                "TIME: " +
                elapsedTime.ToString("F2") +
                "s";

        gameOverPanel.SetActive(true);

        ThirdPersonController controller =
            player.GetComponent<ThirdPersonController>();

        if (controller != null)
            controller.canMove = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        elapsedTime = 0f;
        gameEnded = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        winPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        if (highScoreText != null)
            highScoreText.SetActive(false);

        CharacterController cc =
            player.GetComponent<CharacterController>();

        if (cc != null)
            cc.enabled = false;

        player.position = fixedSpawnPoint.position;
        player.rotation = fixedSpawnPoint.rotation;

        if (cc != null)
            cc.enabled = true;

        ThirdPersonController controller =
            player.GetComponent<ThirdPersonController>();

        if (controller != null)
            controller.canMove = true;

        PlayerHealth health =
            player.GetComponent<PlayerHealth>();

        if (health != null)
            health.ResetHealth();

        CoinCollector collector =
            player.GetComponent<CoinCollector>();

        if (collector != null)
            collector.ResetCoins();

        foreach (GameObject coin in spawnedCoins)
        {
            if (coin != null)
                Destroy(coin);
        }

        spawnedCoins.Clear();

        SpawnCoins();

        AudioManager.instance?.PlayIdle();
    }

    public void LoadMainMenu()
    {
        AudioManager.instance?.StopAllAudio();

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f),
                spawnArea.y,
                Random.Range(-spawnArea.z / 2f, spawnArea.z / 2f)
            );

            GameObject coin =
                Instantiate(
                    coinPrefab,
                    randomPosition,
                    Quaternion.identity
                );

            spawnedCoins.Add(coin);
        }
    }
}