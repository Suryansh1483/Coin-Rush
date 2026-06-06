using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public Image healthFill;
    public float maxHealth = 100f;

    [Header("Damage")]
    public float damageOnHit = 20f;

    [Header("Knockback")]
    public float knockbackStrength = 8f;
    public float knockbackDuration = 0.4f;

    private float currentHealth;
    private CharacterController controller;
    private bool isKnockedBack;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        currentHealth = maxHealth;

        UpdateHealthUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fire"))
            return;

        TakeDamage(damageOnHit);

        GameManager.instance?.AddPenaltyTime(2f);

        AudioManager.instance?.PlayHurt();

        Vector3 knockDirection =
            (transform.position - other.transform.position).normalized;

        knockDirection.y = 0f;

        if (!isKnockedBack)
        {
            StartCoroutine(
                Knockback(
                    knockDirection,
                    knockbackStrength,
                    knockbackDuration
                )
            );
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );

        UpdateHealthUI();

        if (currentHealth <= 0f)
        {
            GameManager.instance?.TriggerGameOver();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount =
                currentHealth / maxHealth;
        }
    }

    private IEnumerator Knockback(
        Vector3 direction,
        float strength,
        float duration)
    {
        isKnockedBack = true;

        float timer = 0f;

        while (timer < duration)
        {
            controller.Move(
                direction *
                strength *
                (1f - timer / duration) *
                Time.deltaTime
            );

            timer += Time.deltaTime;

            yield return null;
        }

        isKnockedBack = false;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}