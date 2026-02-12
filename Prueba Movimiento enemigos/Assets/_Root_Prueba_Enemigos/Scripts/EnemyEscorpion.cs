using UnityEngine;
using UnityEngine.AI;

public class EnemyEscorpion : MonoBehaviour, IDamageable, IEnemyStats
{
    [Header("Enemy Stats")]
    public float speed = 2f;
    public float baseHealth = 3f;
    public float baseDamage = 1f;

    [Header("Attack Settings")]
    public float attackRange = 1f;        // Distancia a la que el escorpión ataca
    public float attackCooldown = 1f;     // Tiempo entre ataques

    [Header("References")]
    [SerializeField] private HealthBar healthBar;
    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;
    private WaveManager waveManager;

    [Header("State")]
    public bool isDead = false;
    [SerializeField] private bool isFacingRight = false;

    private float enemyHealth;
    private float currentHealth;
    private float currentDamage;
    private Vector2 enemyOrientation;
    private float lastAttackTime = 0f;

    #region Initialization

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        waveManager = FindObjectOfType<WaveManager>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentHealth = baseHealth;
        currentDamage = baseDamage;
        enemyHealth = currentHealth;

        healthBar.UpdateHealthBar(enemyHealth, currentHealth);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("No se encontró ningún objeto con el tag 'Player'");
    }

    public void SetStats(float healthMultiplier, float damageMultiplier)
    {
        baseHealth *= healthMultiplier;
        baseDamage *= damageMultiplier;

        currentHealth = baseHealth;
        currentDamage = baseDamage;
        enemyHealth = currentHealth;

        healthBar.UpdateHealthBar(enemyHealth, currentHealth);
    }

    #endregion

    #region Update Movement & Attack

    private void Update()
    {
        if (player == null || isDead) return;

        agent.SetDestination(player.position);

        // Animación de movimiento
        anim.SetBool("escorpionMovimiento", agent.velocity.magnitude > 0.1f);

        // Flip del sprite según posición del jugador
        enemyOrientation = player.position - transform.position;
        if (enemyOrientation.x > 0 && !isFacingRight) Flip();
        if (enemyOrientation.x < 0 && isFacingRight) Flip();

        // Ataque por proximidad
        if (Vector2.Distance(transform.position, player.position) <= attackRange &&
            Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    private void AttackPlayer()
    {
        if (player.TryGetComponent<IDamageable>(out IDamageable dmg))
        {
            dmg.TakeDamage(currentDamage);
            anim.SetTrigger("Attack");
        }
    }

    #endregion

    #region Damage / Death

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        AudioManager.Instance.PlaySFX(0);
        enemyHealth -= damageAmount;
        healthBar.UpdateHealthBar(enemyHealth, currentHealth);

        if (enemyHealth <= 0)
        {
            isDead = true;
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        agent.isStopped = true;
        agent.enabled = false;

        anim.SetTrigger("Death");

        GetComponent<Collider2D>().enabled = false;

        if (waveManager != null)
            waveManager.EnemyDefeated();

        Destroy(gameObject, 2f);
    }

    #endregion

    #region Flip Sprite

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    #endregion
}
