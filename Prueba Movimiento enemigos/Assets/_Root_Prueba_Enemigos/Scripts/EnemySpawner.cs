using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] float spawnRate = 1f;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] bool canSpawn = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (true)
        {
            yield return wait;
            int rand = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[rand];
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }
}
