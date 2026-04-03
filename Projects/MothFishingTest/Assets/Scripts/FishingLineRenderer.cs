using UnityEngine;

public class FishingLineController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform rodTip; // empty at end of rod
    public Transform bobber; // the invisible bobber

    void Update()
    {
        if (lineRenderer != null && rodTip != null && bobber != null)
        {
            // Update line positions every frame
            lineRenderer.SetPosition(0, rodTip.position);
            lineRenderer.SetPosition(1, bobber.position);
        }
    }
}