using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject enemyBullet;
    public Transform bulletPosition;

    private GameObject player;
    private float shootTimer;

    // Configuración del disparo
    private int bulletCount = 3;
    private float spreadAngle = 20f;
    private float timeBetweenShots = 2.5f;
    private float timeBetweenShotguns = 0.4f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 6f)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= timeBetweenShots)
            {
                shootTimer = 0f;
                StartCoroutine(DoubleShotgun());
            }
        }
    }

    IEnumerator DoubleShotgun()
    {
        FireShotgun();
        yield return new WaitForSeconds(timeBetweenShotguns);
        FireShotgun();
    }

    void FireShotgun()
    {
        Vector2 baseDirection =
            (player.transform.position - bulletPosition.position).normalized;

        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 shootDirection =
                Quaternion.AngleAxis(angle, Vector3.forward) * baseDirection;

            GameObject bullet =
                Instantiate(enemyBullet, bulletPosition.position, Quaternion.identity);

            bullet.GetComponent<EnemyBulletScript>().direction = shootDirection;
        }
    }
}
