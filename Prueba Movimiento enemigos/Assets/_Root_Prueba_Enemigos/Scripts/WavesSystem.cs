using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int enemiesPerWave = 5;
    public float waveInterval = 5f;
    public float difficultyIncrease = 1.2f;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Transform player;
    public bool upgradeClicked = true;
    public GameObject upgradesPanel;

    [Header("Enemy Spawn Settings")]
    public float[] enemyProbabilities;

    public int waveNumber = 0;
    private int remainingEnemies;

    [Header("Level Up References")]
    public LevelUpSystem levelUpSystem;

    void Start()
    {
        upgradeClicked = false;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;
        enemiesPerWave = Mathf.FloorToInt(enemiesPerWave * 1.2f);
        remainingEnemies = enemiesPerWave;

        float healthMultiplier = Mathf.Pow(difficultyIncrease, waveNumber);
        float damageMultiplier = Mathf.Pow(difficultyIncrease, waveNumber);

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy(healthMultiplier, damageMultiplier);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitUntil(() => remainingEnemies == 0);

        upgradeClicked = false;

        levelUpSystem.PausePlayer();
        upgradesPanel.SetActive(true);
        levelUpSystem.ShowUpgrades();

        yield return new WaitUntil(() => upgradeClicked == true);

        upgradesPanel.SetActive(false);
        levelUpSystem.UnpausePlayer();

        yield return new WaitForSeconds(waveInterval);
        StartCoroutine(SpawnWave());
    }

    void SpawnEnemy(float healthMultiplier, float damageMultiplier)
    {
        Transform spawnPoint =
            spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject selectedEnemyPrefab = SelectEnemyPrefab();

        GameObject newEnemy = Instantiate(
            selectedEnemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        
        newEnemy.transform.localScale = Vector3.one;

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetStats(healthMultiplier, damageMultiplier);
        }
    }

    GameObject SelectEnemyPrefab()
    {
        float randomValue = Random.Range(0f, 100f);
        float cumulativeProbability = 0f;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            cumulativeProbability += enemyProbabilities[i];

            if (randomValue <= cumulativeProbability)
            {
                return enemyPrefabs[i];
            }
        }

        return enemyPrefabs[0];
    }

    public void EnemyDefeated()
    {
        remainingEnemies--;
    }
}
