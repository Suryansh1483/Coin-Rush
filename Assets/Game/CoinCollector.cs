using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public int coins = 0;
    public TextMeshProUGUI coinText;

    void Start()
    {
        UpdateUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            PlayerAudioManager.instance?.PlayCoinSound();
            coins++;
            UpdateUI();
            Destroy(other.gameObject);
        }
    }

    public void ResetCoins()
    {
        coins = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = "COINS: " + coins;
        else
            Debug.LogWarning("CoinCollector: coinText not assigned!");
    }
}
