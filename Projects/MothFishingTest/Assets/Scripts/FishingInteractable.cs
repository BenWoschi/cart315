using UnityEngine;

public class FishingInteractable : MonoBehaviour
{
    public GameObject fishingUI; // Assign the Canvas UI here

    void OnMouseDown()
    {
        // Show the fishing UI when the sphere is clicked
        if (fishingUI != null)
            fishingUI.SetActive(true);
    }
}