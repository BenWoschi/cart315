using UnityEngine;

public class FishingTriggerArea : MonoBehaviour
{
    public CameraFollowRotation cam;
    public Transform focusPoint; // Assign in inspector (empty GameObject near fishing spot)

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