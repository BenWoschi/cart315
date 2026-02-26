using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    public float speed = 3f;
    public float socialDistancing = 1.2f;

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

        float distance = Vector2.Distance(transform.position, player.position);
        float slow = RainWeather.Instance.GetSlowMultiplier();

        if (distance > socialDistancing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed * slow;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
