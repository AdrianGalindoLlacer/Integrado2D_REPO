using UnityEngine;
using TMPro;

public class EnemyCounter : MonoBehaviour
{

    GameObject[] enemies;
    public TMP_Text enemyCountText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCountText.text = "Enemies Left: " + enemies.Length.ToString();
    }
}
