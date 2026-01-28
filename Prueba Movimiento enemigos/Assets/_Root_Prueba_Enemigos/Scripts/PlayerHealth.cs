using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]float maxHealth = 10f;
    float currentHealth = 0f;
    bool invencibility = false;
    [SerializeField] float invecibilityDuration;
    [SerializeField] Slider barraVida;
    SpriteRenderer spriteRenderer;
    float blinkDuration = 0.2f;
    

    void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void TakeDamage(float playerDamage)
    {
        if (invencibility)
        {
            return;
        }
        currentHealth -= playerDamage;
        currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);
        Debug.Log(currentHealth);
        barraVida.value = currentHealth/maxHealth;
        if(Mathf.Approximately(currentHealth, 0))
        {
            Die();
            return;
        }

        StartInvencibility(invecibilityDuration);
    }

    void Die()
    {
        gameObject.SetActive(false);
        Invoke(nameof(SceneReload),2f);
    }

    void StartInvencibility(float duration)
    {
        if(invencibility)
        {
            return;
        }
        invencibility = true;
        StartCoroutine(InvencibilityCoroutine(duration));
        StartCoroutine(BlinkingCoroutine(blinkDuration));
        Debug.Log("me han pegado, ahora no me la devuelves");
    }
    IEnumerator InvencibilityCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        invencibility = false;
        Debug.Log("no me pegues porfa");
    }
    void SceneReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator BlinkingCoroutine(float duration)
    {
        while (invencibility)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(duration);
        }   
        spriteRenderer.enabled = true;
    }
}

