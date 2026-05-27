using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public Image healthFill;
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Damage / Knockback")]
    public float damageOnHit = 20f;
    public float knockbackStrength = 8f;
    public float knockbackDuration = 0.4f;

    private CharacterController cc;
    private bool isKnocked = false;

    void Start()
    {
        currentHealth = maxHealth;
        cc = GetComponent<CharacterController>();

        if (healthFill != null)
        {
            // Auto-fix Image type if not set correctly
            if (healthFill.type != Image.Type.Filled)
                healthFill.type = Image.Type.Filled;

            healthFill.fillAmount = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            TakeDamage(damageOnHit);

            Vector3 dir = (transform.position - other.transform.position).normalized;
            dir.y = 0;

            if (!isKnocked)
                StartCoroutine(Knockback(dir, knockbackStrength, knockbackDuration));

            PlayerAudioManager.instance?.PlayFireHitSound();
        }
    }

    IEnumerator Knockback(Vector3 direction, float strength, float duration)
    {
        isKnocked = true;
        float timer = 0f;

        while (timer < duration)
        {
            cc.Move(direction * strength * (1f - timer / duration) * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isKnocked = false;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthFill != null)
        {
            // Smoothly update the UI fill
            healthFill.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            PlayerAudioManager.instance?.StopAllSounds();
            PlayerAudioManager.instance?.PlayDeathSound();

            // Stop player movement instantly
            ThirdPersonController tpc = GetComponent<ThirdPersonController>();
            if (tpc != null)
                tpc.canMove = false;

            // Notify GameManager
            GameManager gm = FindObjectOfType<GameManager>();
            gm?.TriggerGameOver();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        if (healthFill != null)
            healthFill.fillAmount = 1f;
    }
}
