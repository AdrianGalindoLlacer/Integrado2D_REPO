using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    public float speed = 2f;
    public float enemyMaxHealth = 3f;
    public float enemyDamage = 1f;
    public bool isDead;

    [SerializeField] bool isFacingRight = false;

    float enemyHealth;
    Transform player;
    WaveManager waveManager;
    private NavMeshAgent agent;

    [SerializeField] HealthBar healthBar;
    [SerializeField] float baseHealth = 3f;
    [SerializeField] float baseDamage = 1f;

    float currentHealth;
    float currentDamage;

    Vector2 enemyOrientation;

    public void SetStats(float healthMultiplier, float damageMultiplier)
    {
        currentHealth = baseHealth * healthMultiplier;
        currentDamage = baseDamage * damageMultiplier;
        enemyHealth = currentHealth;
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        waveManager = FindObjectOfType<WaveManager>();
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
            player = playerObj.transform;
        else
            Debug.LogWarning("No se encontró ningún objeto con el tag 'Player'");
    }

    void Update()
    {
        if (player == null) return;

        agent.SetDestination(player.position);
        

        
        enemyOrientation = player.position - transform.position;

        if (enemyOrientation.x > 0 && !isFacingRight) Flip();
        if (enemyOrientation.x < 0 && isFacingRight) Flip();
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

        if (enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {
        if (waveManager != null)
            waveManager.EnemyDefeated();

        Destroy(gameObject);
    }

   

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 actualScale = transform.localScale;
        actualScale.x *= -1;
        transform.localScale = actualScale;
    }
}
