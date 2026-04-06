using UnityEngine;
public class TargetZoneController : MonoBehaviour
{
    public RectTransform targetZone;
    public float startingWidth = 200f;
    public float minWidth = 50f;
    public float maxWidth = 500f;

    public float currentWidth = 150f;

    public void SetWidth(float width)
    {
        currentWidth = Mathf.Clamp(width, minWidth, maxWidth);
        targetZone.sizeDelta = new Vector2(currentWidth, targetZone.sizeDelta.y);
    }
}