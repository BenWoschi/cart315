using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage = 10;
    public float cooldown = 1f;

    private float timer = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("DamageZone is touching the player");
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                timer = cooldown;
            }
        }
    }
}