using UnityEngine;

public class MothMovement : MonoBehaviour
{
    public EnergyManager energyManager;
    public float moveSpeed = 5f;
    public Animator animator;
    public Transform camTransform;
    public RhythmGameManager rhythmManager;

    void Start()
    {
        energyManager = EnergyManager.Instance;
    }

    void Update()
    {
        // Stop movement during rhythm/fishing
        if (rhythmManager != null && rhythmManager.isActive)
            return;

        // Get input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Camera directions
        Vector3 camForward = camTransform.forward;
        Vector3 camRight = camTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Movement relative to camera
        Vector3 move = (camForward * v) + (camRight * h);
        float inputSpeed = move.magnitude;  // 0–1 based on input
        move = move.normalized;

        // Scale actual movement by energy
        float scaledSpeed = inputSpeed * Mathf.Clamp(energyManager.energy, 0.1f, 1f);

        // Move character
        transform.Translate(move * moveSpeed * scaledSpeed * Time.deltaTime, Space.World);

        // Rotate toward movement
        if (scaledSpeed > 0.01f)
            transform.forward = move;

        // Update Blend Tree parameters
        if (animator != null && energyManager != null)
        {
            animator.SetFloat("speed", scaledSpeed);               // X-axis
            animator.SetFloat("energy", energyManager.energy);     // Y-axis
        }
    }
}