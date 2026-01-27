using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemies Principal Stats")]
    public float speed;
    [SerializeField] float enemyHealth, enemyMaxHealth = 3f;
    public int enemyDamage;


    [Header("Tracking Parameters")]
    private float distance;

    [Header("Player Parameters")]
    public GameObject player;

    
    void Start()
    {
        enemyHealth = enemyMaxHealth;
    }


    void Update()
    {
        EnemyMovement();
    }

    private void OnCollisonEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))  
        {
            PlayerHurt();
        }
    }

    private void TakeDamage(float damageAmount)
    {
       
        {
            enemyHealth -= damageAmount;
        }
    }

    private void EnemyDeath()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerHurt()
    {
        GameManager.Instance.playerHealth =- enemyDamage;
    }

    private void EnemyMovement()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
