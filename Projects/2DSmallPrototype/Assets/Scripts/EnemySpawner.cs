using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 1.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnRate);
    }

    void SpawnEnemy()
    {
        float x = Random.Range(-8f, 8f);
        float y = 6f; // above the screen

        Instantiate(enemyPrefab, new Vector2(x, y), Quaternion.identity);
    }
}
