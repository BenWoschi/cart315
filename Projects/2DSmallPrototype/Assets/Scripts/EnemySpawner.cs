using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    public int maxEnemies = 10;

    public BoxCollider2D spawnArea;

    public float minSpawnDistance = 1.7f;
    public int maxAttempts = 7;

    // Spawn Timer
    public float spawnDelay = 0.5f;
    private float spawnTimer = 0f;

    public Transform player;
    public float minPlayerDistance = 2f;


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
            Bounds bounds = spawnArea.bounds;

            float padding = 0.5f;

            Vector2 spawnPos = new Vector2(
                Random.Range(bounds.min.x + padding, bounds.max.x - padding),
                Random.Range(bounds.min.y + padding, bounds.max.y - padding)
            );


            bool validPosition = true;

            // Check distance from player first
            if (Vector2.Distance(spawnPos, player.position) < minPlayerDistance)
            {
                validPosition = false;
            }

            // Check distance from other enemies
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
                int roll = Random.Range(0, 100);

                GameObject chosenEnemy;

                if (roll < 80)
                {
                    chosenEnemy = enemyPrefabs[0]; // normal enemy
                }
                else
                {
                    chosenEnemy = enemyPrefabs[1]; // faster enemy
                }

                Instantiate(chosenEnemy, spawnPos, Quaternion.identity);

                return true;
            }
        }

        return false;
    }
}
