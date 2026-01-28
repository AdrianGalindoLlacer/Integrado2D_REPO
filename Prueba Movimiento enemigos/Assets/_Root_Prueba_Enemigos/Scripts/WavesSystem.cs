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

    [Header("Enemy Spawn Settings")]
    public float[] enemyProbabilities; // Probabilidades para cada enemigo (porcentaje)

    private int waveNumber = 0;
    private int remainingEnemies;

    void Start()
    {
        // Empieza la primera oleada
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        waveNumber++;
        remainingEnemies = enemiesPerWave;

        // Aumentar el número de enemigos cada oleada (por ejemplo, un 20% más por oleada)
        enemiesPerWave = Mathf.FloorToInt(enemiesPerWave * 1.2f);  // Aumenta un 20% por oleada

        // Aumentar la dificultad de los enemigos según el número de la oleada
        float healthMultiplier = Mathf.Pow(difficultyIncrease, waveNumber);
        float damageMultiplier = Mathf.Pow(difficultyIncrease, waveNumber);

        // Crear enemigos
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy(healthMultiplier, damageMultiplier);
            yield return new WaitForSeconds(0.5f);  // Un pequeño retraso entre la aparición de enemigos
        }

        // Esperar hasta que todos los enemigos de la oleada sean eliminados
        yield return new WaitUntil(() => remainingEnemies == 0);

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
