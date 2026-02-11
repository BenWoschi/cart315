using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 3f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float slow = RainWeather.Instance.GetSlowMultiplier();
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * speed * slow;
    }
}
