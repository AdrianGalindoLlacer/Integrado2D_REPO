using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int enemiesPerWave = 5;  // Número inicial de enemigos por oleada
    public float waveInterval = 5f;  // Tiempo entre oleadas
    public float difficultyIncrease = 1.2f;  // Multiplicador de dificultad por oleada (salud y daño)
    public GameObject[] enemyPrefabs;  // Lista de Prefabs de enemigos
    public Transform[] spawnPoints;  // Lista de puntos de spawn
    public Transform player;  // Referencia al jugador
    public bool upgradeClicked = true;
    public GameObject upgradesPanel;

    [Header("Enemy Spawn Settings")]
    public float[] enemyProbabilities; // Probabilidades para cada enemigo (porcentaje)

    public int waveNumber = 0;
    private int remainingEnemies;

    [Header("Level Up References")]
    public LevelUpSystem levelUpSystem;

    void Start()
    {
        upgradeClicked = false;
        // Empieza la primera oleada
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

        // Esperar hasta que todos los enemigos de la oleada sean eliminados
        yield return new WaitUntil(() => remainingEnemies == 0);

        upgradeClicked = false;

        levelUpSystem.PausePlayer();

        upgradesPanel.SetActive(true);
        levelUpSystem.ShowUpgrades();
        Debug.Log("Espera");

        yield return new WaitUntil(() => upgradeClicked == true);
        Debug.Log("Fin Espera");

        upgradesPanel.SetActive(false);


        levelUpSystem.UnpausePlayer();



        // Esperar entre oleadas antes de comenzar la siguiente
        yield return new WaitForSeconds(waveInterval);

        // Llamar a la siguiente oleada
        StartCoroutine(SpawnWave());
    }

    void SpawnEnemy(float healthMultiplier, float damageMultiplier)
    {
        // Seleccionar un punto de spawn aleatorio
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Seleccionar un tipo de enemigo aleatorio basado en probabilidades
        GameObject selectedEnemyPrefab = SelectEnemyPrefab();

        // Instanciar el enemigo
        GameObject newEnemy = Instantiate(selectedEnemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            // Respetar las estadísticas base y multiplicarlas por los multiplicadores de dificultad
            enemyScript.SetStats(healthMultiplier, damageMultiplier);
        }
    }

    GameObject SelectEnemyPrefab()
    {
        float randomValue = Random.Range(0f, 100f);  // Valor aleatorio entre 0 y 100
        float cumulativeProbability = 0f;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            cumulativeProbability += enemyProbabilities[i];

            if (randomValue <= cumulativeProbability)
            {
                return enemyPrefabs[i];
            }
        }

        return enemyPrefabs[0];  // Si algo falla, devolvemos el primer enemigo por defecto
    }

    public void EnemyDefeated()
    {
        remainingEnemies--;
    }
}
