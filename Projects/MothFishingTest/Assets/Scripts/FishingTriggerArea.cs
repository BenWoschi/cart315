using UnityEngine;

public class FishingTriggerArea : MonoBehaviour
{
    public Transform focusPoint; // Assign in inspector (empty GameObject near fishing spot)

    private CameraFollowRotation cam;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraFollowRotation>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var fishing = other.GetComponent<FishingInteractable>();
            if (fishing != null)
                fishing.PlayerEnterRange();

            if (cam != null)
                cam.SetFocus(focusPoint != null ? focusPoint : transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var fishing = other.GetComponent<FishingInteractable>();
            if (fishing != null)
                fishing.PlayerExitRange();

            if (cam != null)
                cam.ClearFocus();
        }
    }
}