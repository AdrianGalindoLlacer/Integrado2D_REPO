using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    public float speed = 2f;
    public float enemyMaxHealth = 3f;  // Este valor representa la salud base del enemigo.
    public float enemyDamage = 1f;     // Este valor representa el daño base del enemigo.

    float enemyHealth;
    Transform player;
    WaveManager waveManager;  // Referencia al WaveManager

    [SerializeField] HealthBar healthBar;
    [SerializeField] float baseHealth = 3f;  // Este es el valor base de la salud en el prefab
    [SerializeField] float baseDamage = 1f;  // Este es el valor base del daño en el prefab

    float currentHealth;
    float currentDamage;

    // Este método ajusta las estadísticas basadas en los multiplicadores de dificultad.
    public void SetStats(float healthMultiplier, float damageMultiplier)
    {
        // Respetamos las estadísticas base y aplicamos el multiplicador.
        currentHealth = baseHealth * healthMultiplier;
        currentDamage = baseDamage * damageMultiplier;
        enemyHealth = currentHealth;  // Establecemos la salud inicial del enemigo
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        waveManager = FindObjectOfType<WaveManager>();  // Busca el WaveManager en la escena
    }

    void Start()
    {
        healthBar.UpdateHealthBar(enemyHealth, currentHealth);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag 'Player'");
        }
    }

    void Update()
    {
        if (player == null) return;
        EnemyMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out IDamageable player))
        {
            player.TakeDamage(currentDamage);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;
        healthBar.UpdateHealthBar(enemyHealth, currentHealth);
        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
        if (waveManager != null)
        {
            waveManager.EnemyDefeated();  // Notifica al WaveManager que este enemigo ha sido derrotado
        }

        Destroy(gameObject);
    }

    void EnemyMovement()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Rotación opcional (quítala si no quieres que rote)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }
}
