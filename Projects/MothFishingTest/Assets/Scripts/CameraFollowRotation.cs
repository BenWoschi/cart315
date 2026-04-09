using UnityEngine;

public class CameraFollowRotation : MonoBehaviour
{
    private float currentFocusStrength = 0f;
    public Transform target;
    public Vector3 lookOffset = new Vector3(0, 1.5f, 0);

    [Header("FOV Settings")]
    public float minFOV = 5f;
    public float maxFOV = 35f;
    public EnergyManager energyManager;

    [Header("Focus Settings")]
    public Transform focusTarget;
    public float focusFOVBoost = 5f;

    private bool isFocusing = false;
    private Vector3 initialPosition;

    void Start()
    {
        if (target == null) return;
        initialPosition = transform.position;

        if (energyManager == null) energyManager = EnergyManager.Instance;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Keep camera in place
        Vector3 shake = ScreenShake.Instance != null ? ScreenShake.Instance.shakeOffset : Vector3.zero;
        transform.position = initialPosition + shake;

        Vector3 playerPoint = target.position + lookOffset;

        // Determine target focus strength
        float targetStrength = (isFocusing && focusTarget != null) ? 0.3f : 0f;

        // Smoothly blend in/out
        currentFocusStrength = Mathf.Lerp(currentFocusStrength, targetStrength, Time.deltaTime * 3f);

        // Blend between player and object
        Vector3 lookPoint = playerPoint;

        if (focusTarget != null)
        {
            lookPoint = Vector3.Lerp(playerPoint, focusTarget.position, currentFocusStrength);
        }

        transform.LookAt(lookPoint);

        // Base FOV from energy
        float energy = energyManager != null ? Mathf.Clamp01(energyManager.energy) : 1f;
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, energy);

        // Add boost when focusing
        if (isFocusing)
        {
            targetFOV += focusFOVBoost;
        }

        GetComponent<Camera>().fieldOfView = targetFOV;
    }

    // --- Called by trigger ---
    public void SetFocus(Transform newTarget)
    {
        focusTarget = newTarget;
        isFocusing = true;
    }

    public void ClearFocus()
    {
        isFocusing = false;
        focusTarget = null;
    }
}