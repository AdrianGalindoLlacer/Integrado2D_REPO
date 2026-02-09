using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletSpeed = 5f;
    public Vector2 direction;

    float timeToDisappear;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * bulletSpeed;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    void Update()
    {
        timeToDisappear += Time.deltaTime;
        if (timeToDisappear > 10f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Destroy(gameObject);
    }
}
