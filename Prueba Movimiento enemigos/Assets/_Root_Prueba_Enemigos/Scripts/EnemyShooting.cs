using UnityEngine;

public class EnemyShooting : MonoBehaviour
{

    public GameObject enemyBullet;
    public Transform bulletPosition;
    private GameObject player;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 6)
        {
            timer += Time.deltaTime;

            if (timer > 1)
            {
                timer = 0;
                EnemyShoot();
            }
        }

        
    }

    public void EnemyShoot()
    {
        Instantiate(enemyBullet, bulletPosition.position, Quaternion.identity);
    }
}
