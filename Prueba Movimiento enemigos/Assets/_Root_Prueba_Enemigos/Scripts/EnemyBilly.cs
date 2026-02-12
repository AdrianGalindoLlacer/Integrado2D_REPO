using UnityEngine;
using UnityEngine.AI;

public class EnemyBilly : MonoBehaviour, IDamageable, IEnemyStats
{
    [Header("Stats")]
    public float speed = 2f;
    public float baseHealth = 5f;
    public float baseDamage = 1f;

    [Header("Shooting")]
    [SerializeField] private float shootCooldown = 2f;
    private float shootTimer;

    [Header("References")]
    [SerializeField] private HealthBar healthBar;

    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;
    private WaveManager waveManager;

    [Header("State")]
    private bool isDead = false;
    private bool isFacingRight = false;

    private float currentHealth;
    private float currentDamage;



    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        waveManager = FindObjectOfType<WaveManager>();
        healthBar = GetComponentInChildren<HealthBar>();
    }

    private void Start()
    {
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentHealth = baseHealth;
        currentDamage = baseDamage;

        healthBar.UpdateHealthBar(currentHealth, baseHealth);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

   
    public void SetStats(float healthMultiplier, float damageMultiplier)
    {
        baseHealth *= healthMultiplier;
        baseDamage *= damageMultiplier;

        currentHealth = baseHealth;
        currentDamage = baseDamage;
    }

  

    private void Update()
    {
        if (player == null || isDead) return;

        agent.SetDestination(player.position);
        HandleFlip();

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCooldown)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    private void Shoot()
    {
        anim.SetTrigger("Shoot");
    }


    public void TakeDamage(float damage)
    {
        if (isDead) return;

        AudioManager.Instance.PlaySFX(0);
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, baseHealth);

        anim.SetTrigger("Hurt");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;

        agent.isStopped = true;
        agent.enabled = false;

        anim.SetTrigger("Death");
        GetComponent<Collider2D>().enabled = false;

        if (waveManager != null)
            waveManager.EnemyDefeated();

        Destroy(gameObject, 2f);
    }

    private void HandleFlip()
    {
        float dir = player.position.x - transform.position.x;

        if (dir > 0 && !isFacingRight)
            Flip();
        else if (dir < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
