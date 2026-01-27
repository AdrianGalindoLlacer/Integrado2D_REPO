using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireForce = 20f;
    public float fireRate = 0.15f;

    float fireTimer;

    void Update()
    {
        fireTimer -= Time.deltaTime;
    }

    public void Fire()
    {
        if (fireTimer > 0) return;

        fireTimer = fireRate;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.up * fireForce;
    }
}
