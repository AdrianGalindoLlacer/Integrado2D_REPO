using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] float timeBetweenWaves = 5f;

    [Header("Wave Settings")]
    [SerializeField] int enemiesBasePerWave = 5;
    [SerializeField] float spawnRate = 1f;

    [Header("Difficulty Scaling")]
    [SerializeField] float healthMultiplierIncrease = 0.2f;
    [SerializeField] float damageMultiplierIncrease = 0.15f;

    int currentWave = 1;

    void Start()
    {
        StartCoroutine(WaveSpawner());
    }

    private IEnumerator WaveSpawner()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnWave());
            currentWave++;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private IEnumerator SpawnWave()
    {
        int enemiesToSpawn = enemiesBasePerWave + currentWave * 2;

        float healthMultiplier = 1 + (currentWave - 1) * healthMultiplierIncrease;
        float damageMultiplier = 1 + (currentWave - 1) * damageMultiplierIncrease;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemy = Instantiate(
                enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                transform.position,
                Quaternion.identity
            );

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SetStats(healthMultiplier, damageMultiplier);
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }
}

