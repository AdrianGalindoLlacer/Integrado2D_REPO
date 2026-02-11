using UnityEngine;
using UnityEngine.AI;

public class EnemyCactus : MonoBehaviour, IDamageable, IEnemyStats
{
    [Header("Enemy Stats")]
    public float speed = 2f;
    public float baseHealth = 3f;
    public float baseDamage = 1f;

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

    // 🔥 ESCALADO POR OLEADAS (INTERFAZ)
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

    #region Update Movement

    private void Update()
    {
        if (player == null || isDead) return;

        agent.SetDestination(player.position);

        enemyOrientation = player.position - transform.position;
        if (enemyOrientation.x > 0 && !isFacingRight) Flip();
        if (enemyOrientation.x < 0 && isFacingRight) Flip();
    }

    #endregion

    #region Collision / Attack

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.TryGetComponent(out IDamageable player))
        {
            player.TakeDamage(currentDamage);
            anim.SetTrigger("Shot");
        }
    }

    #endregion

    #region Damage / Death

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

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
