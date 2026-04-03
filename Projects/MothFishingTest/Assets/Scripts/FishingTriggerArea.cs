using UnityEngine;

public class FishingTriggerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var fishing = other.GetComponent<FishingInteractable>();
            if (fishing != null)
                fishing.PlayerEnterRange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var fishing = other.GetComponent<FishingInteractable>();
            if (fishing != null)
                fishing.PlayerExitRange();
        }
    }
}