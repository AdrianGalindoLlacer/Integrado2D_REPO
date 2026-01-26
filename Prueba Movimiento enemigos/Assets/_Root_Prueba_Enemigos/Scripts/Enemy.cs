using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemies Principal Stats")]
    public float speed;
    private float health;
    public float maxHealth;
    public float attackDamage;

    [Header("Tracking Parameters")]
    private float distance;

    [Header("Player Parameters")]
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);


    }

    private void TakeDamage()
    {
       
        {
            health = -damage;
        }
    }

    private void EnemyDeath()
    {
        if (health <= 0)
        {
            
        }
    }
}
