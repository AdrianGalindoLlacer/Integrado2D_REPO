using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    public float speed = 2f;
    public float enemyMaxHealth = 3f;  // Este valor representa la salud base del enemigo.
    public float enemyDamage = 1f;     // Este valor representa el daño base del enemigo.
    public bool isDead;

    float enemyHealth;
    Transform player;
    WaveManager waveManager;  // Referencia al WaveManager
    private NavMeshAgent agent;

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
        agent = GetComponent<NavMeshAgent>();
        isDead = false;
    }

    void Start()
    {
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
        agent.SetDestination(player.position);
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
        if (enemyHealth <= 0 && isDead == false )
        {
            isDead = true;
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
        if (waveManager != null)
        {
            waveManager.EnemyDefeated();   //enemigo ha sido derrotado
        }

        Destroy(gameObject);
    }

    void EnemyMovement()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }
}
