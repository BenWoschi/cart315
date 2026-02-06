using UnityEngine;
using System.Collections.Generic;

public class EffectExpand : MonoBehaviour
{
    public float expandTime = 0.5f;
    public float stayTime = 5f;
    public float damagePerSecond = 60f;

    private Vector3 targetScale;
    private float timer = 0f;
    private bool expanded = false;

    private List<GameObject> enemiesInside = new List<GameObject>();

    void Start()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        // Expansion logic
        if (!expanded)
        {
            timer += Time.deltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, timer / expandTime);
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, progress);

            if (progress >= 1f)
            {
                expanded = true;
                timer = 0f;
            }
        }
        else
        {
            timer += Time.deltaTime;

            // Damage enemies over time
            for (int i = enemiesInside.Count - 1; i >= 0; i--)
            {
                GameObject enemy = enemiesInside[i];

                if (enemy == null)
                {
                    enemiesInside.RemoveAt(i);
                    continue;
                }

                Enemy enemyScript = enemy.GetComponent<Enemy>();

                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damagePerSecond * Time.deltaTime);
                }

            }

            if (timer >= stayTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemiesInside.Contains(other.gameObject))
            {
                enemiesInside.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInside.Remove(other.gameObject);
        }
    }
}