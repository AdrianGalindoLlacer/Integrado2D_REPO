using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float speed = 2f;
    public float enemyMaxHealth = 3f;
    public int enemyDamage = 1;

    float enemyHealth;
    Transform player;

    [SerializeField] HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        enemyHealth = enemyMaxHealth;
        healthBar.UpdateHealthBar(enemyHealth, enemyMaxHealth);

        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("No se encontr� ning�n objeto con el tag 'Player'");
        }
    }

    void Update()
    {
        if (player == null) return;

        EnemyMovement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHurt();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;
        healthBar.UpdateHealthBar(enemyHealth, enemyMaxHealth);
        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    void EnemyDeath()
    {

        Destroy(gameObject);

    }

    void PlayerHurt()
    {
        // Aseg�rate de que GameManager existe
        GameManager.Instance.playerHealth -= enemyDamage;
    }

    void EnemyMovement()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        // Rotaci�n opcional (qu�tala si no quieres que rote)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }
}
