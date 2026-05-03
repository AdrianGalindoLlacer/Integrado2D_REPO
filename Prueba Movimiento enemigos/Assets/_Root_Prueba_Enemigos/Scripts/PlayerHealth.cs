using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 10f;
    public float currentHealth = 0f;

    bool invencibility = false;

    [SerializeField] float invecibilityDuration = 1f;
    [SerializeField] Slider barraVida;

    SpriteRenderer spriteRenderer;
    float blinkDuration = 0.1f;

    public GameObject deathPanel;

    Coroutine invencibilityCoroutine;
    Coroutine blinkingCoroutine;

    void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float playerDamage)
    {
        if (invencibility)
            return;

        AudioManager.Instance.PlaySFX(1);

        currentHealth -= playerDamage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        barraVida.value = currentHealth / maxHealth;

        if (Mathf.Approximately(currentHealth, 0))
        {
            Die();
            return;
        }

        // 👉 Invencibilidad NORMAL (con blink)
        ActivateInvincibility(invecibilityDuration, true);
    }

    // 🔥 MÉTODO GENERAL
    public void ActivateInvincibility(float duration, bool useBlink)
    {
        if (invencibilityCoroutine != null)
            StopCoroutine(invencibilityCoroutine);

        if (blinkingCoroutine != null)
            StopCoroutine(blinkingCoroutine);

        invencibility = true;

        invencibilityCoroutine = StartCoroutine(InvencibilityCoroutine(duration));

        // 👉 Solo hace blink si queremos (NO en dash)
        if (useBlink)
        {
            blinkingCoroutine = StartCoroutine(BlinkingCoroutine());
        }
        else
        {
            spriteRenderer.enabled = true; // asegurar visible
        }
    }

    IEnumerator InvencibilityCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        invencibility = false;
    }

    IEnumerator BlinkingCoroutine()
    {
        while (invencibility)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkDuration);
        }

        spriteRenderer.enabled = true;
    }

    public void Die()
    {
        gameObject.SetActive(false);
        Time.timeScale = 0;
        deathPanel.SetActive(true);
    }

    public void SceneReload()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}