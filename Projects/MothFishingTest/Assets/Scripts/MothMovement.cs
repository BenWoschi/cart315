using UnityEngine;

public class MothMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Animator animator;
    public Transform camTransform; // Drag your Main Camera here in the Inspector

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 1. Get Camera directions
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;

        // 2. Flatten them so the moth doesn't fly into the ground/sky
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 3. Create the move direction RELATIVE to the camera
        Vector3 move = (camForward * v) + (camRight * h);

        // Move character
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

        // Rotate toward movement direction
        if (move.magnitude > 0.1f)
        {
            transform.forward = move;
        }

        // Send speed to animator
        animator.SetFloat("Speed", move.magnitude);
    }
}
