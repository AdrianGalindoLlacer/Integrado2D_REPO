using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{
    public WaveManager waveManager;
    GameObject[] enemies;
    public TMP_Text enemyCountText;
    public TMP_Text waveCounterText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCountText.text = "Enemies Left: " + enemies.Length.ToString();
        waveCounterText.text = "Wave: " + waveManager.waveNumber.ToString();
    }
}
