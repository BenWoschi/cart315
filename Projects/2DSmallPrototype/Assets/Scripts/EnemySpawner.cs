using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 7;

    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -2f;
    public float maxY = 2f;

    public float minSpawnDistance = 1.2f; // tweak based on enemy size
    public int maxAttempts = 10; // prevents infinite loops

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            bool validPosition = true;

            foreach (GameObject enemy in enemies)
            {
                if (Vector2.Distance(spawnPos, enemy.transform.position) < minSpawnDistance)
                {
                    validPosition = false;
                    break;
                }
            }

            if (validPosition)
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                return;
            }
        }
    }
}
