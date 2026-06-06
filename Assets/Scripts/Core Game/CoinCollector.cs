using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    [Header("Coins")]
    public int coins;
    public int targetCoins = 25;

    [Header("UI")]
    public TextMeshProUGUI coinText;

    private void Start()
    {
        coins = 0;
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin"))
            return;

        AudioManager.instance?.PlayCoin();

        coins++;

        UpdateUI();

        Destroy(other.gameObject);

        if (coins >= targetCoins)
        {
            GameManager.instance.WinGame();
        }
    }

    public void ResetCoins()
    {
        coins = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "COINS: " + coins + "/" + targetCoins;
        }
    }

    public int GetCoinCount()
    {
        return coins;
    }
}