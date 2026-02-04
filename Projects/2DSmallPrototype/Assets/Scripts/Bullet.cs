using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    private Vector2 direction;

    // This gets called by the Player when the bullet is spawned
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Rotate bullet to face the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
