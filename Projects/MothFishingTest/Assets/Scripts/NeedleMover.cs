using UnityEngine;

public class NeedleMover : MonoBehaviour
{
    public RectTransform needle;
    public float speed = 2f;
    public float range = 400f; // half width of bar

    private float t;

    void Update()
    {
        t += Time.deltaTime * speed;

        float x = Mathf.Sin(t) * range;
        needle.anchoredPosition = new Vector2(x, needle.anchoredPosition.y);
    }
}