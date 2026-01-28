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
    

    void Awake()
    {
        currentHealth = maxHealth;
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
}

