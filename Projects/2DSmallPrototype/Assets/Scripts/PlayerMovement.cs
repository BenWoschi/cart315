using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public float speed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 1f;
    public float dashCooldown = 1f;
    bool isDashing;
    bool canDash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canDash = true;
    }


    void Update()
    {
        if (isDashing)
        {
            return;
        }

        // Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!isDashing)
        {
            rb.linearVelocity = moveDirection * speed;
        }
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.linearVelocity = moveDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
