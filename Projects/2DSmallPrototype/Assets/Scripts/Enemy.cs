using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float fallSpeed = 2f;

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

}
