using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemies = 7;

    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -2f;
    public float maxY = 2f;

    public float minSpawnDistance = 1.2f;
    public int maxAttempts = 10;

    // Spawn Timer
    public float spawnDelay = 2f; // seconds between respawns
    private float spawnTimer = 0f;

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        int currentEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Only spawn if under max AND timer finished
        if (currentEnemies < maxEnemies && spawnTimer <= 0f)
        {
            if (SpawnEnemy())
            {
                spawnTimer = spawnDelay; // reset timer after successful spawn
            }
        }
    }

    bool SpawnEnemy()
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
                return true;
            }
        }

        return false;
    }
}
