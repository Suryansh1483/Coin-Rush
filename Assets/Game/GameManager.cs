using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 30f;
    private float timeLeft;
    public TMP_Text timerText;

    [Header("UI References")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public int coinCount = 20;
    public Vector3 spawnArea = new Vector3(20, 1, 20);

    [Header("Player & Spawn")]
    public Transform player;
    public Transform fixedSpawnPoint;

    private bool gameActive = true;
    private List<GameObject> spawnedCoins = new List<GameObject>();

    void Start()
    {
        timeLeft = totalTime;
        gameOverPanel.SetActive(false);
        SpawnCoins();
    }

    void Update()
    {
        if (gameActive)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.CeilToInt(timeLeft);

            if (timeLeft <= 0)
                TriggerGameOver();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
                RestartGame();
        }
    }

    public void TriggerGameOver()
    {
        if (!gameActive) return;

        gameActive = false;
        gameOverPanel.SetActive(true);
        gameOverText.text = "GAME OVER\nPress 'R' to Restart";

        ThirdPersonController tpc = player.GetComponent<ThirdPersonController>();
        if (tpc != null)
            tpc.canMove = false;
    }

    void RestartGame()
    {
        gameOverPanel.SetActive(false);
        timeLeft = totalTime;

        // Disable CharacterController before moving the player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Move player to fixed spawn point
        player.position = fixedSpawnPoint.position;
        player.rotation = fixedSpawnPoint.rotation;

        // Re-enable controller after repositioning
        if (cc != null) cc.enabled = true;

        // Re-enable movement
        ThirdPersonController tpc = player.GetComponent<ThirdPersonController>();
        if (tpc != null)
            tpc.canMove = true;

        // Reset player state
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.ResetHealth();

        CoinCollector ccScript = player.GetComponent<CoinCollector>();
        if (ccScript != null)
            ccScript.ResetCoins();

        // Respawn coins
        foreach (GameObject coin in spawnedCoins)
        {
            if (coin != null)
                Destroy(coin);
        }
        spawnedCoins.Clear();
        SpawnCoins();

        gameActive = true;
    }

    public void SpawnCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f),
                spawnArea.y,
                Random.Range(-spawnArea.z / 2f, spawnArea.z / 2f)
            );

            GameObject newCoin = Instantiate(coinPrefab, randomPos, Quaternion.identity);
            spawnedCoins.Add(newCoin);
        }
    }
}
