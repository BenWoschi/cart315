using UnityEngine;
using TMPro;

public class RhythmNoteUI : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform circle;
    public RectTransform approachRing;
    public TextMeshProUGUI keyText;
    public GameObject missX;

    [Header("Timing")]
    public float fadeInDuration = 0.3f;          // how long the note fades in
    public bool delayApproachRingUntilFade = true; // if true, ring starts shrinking after fade-in

    private float hitTime;
    private float approachTime;
    private bool initialized = false;
    private CanvasGroup canvasGroup;
    private bool ringInitialized = false;

    [HideInInspector] public bool isDestroyed = false;

    public void Init(KeyCode key, float hitTime, float approachTime)
    {
        this.hitTime = hitTime;
        this.approachTime = approachTime;

        keyText.text = key.ToString();
        if (missX != null) missX.SetActive(false);

        // Add CanvasGroup if not already on prefab
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f; // start invisible

        // Only initialize ring immediately if delay is false
        if (!delayApproachRingUntilFade && circle != null && approachRing != null)
        {
            float startScale = (approachRing.rect.width / circle.rect.width) * 1.5f;
            approachRing.localScale = Vector3.one * startScale;
            ringInitialized = true;
        }

        initialized = true;
    }

    void Update()
    {
        if (!initialized || isDestroyed) return;

        // Fade-in the note
        if (canvasGroup != null)
        {
            canvasGroup.alpha = Mathf.Min(1f, canvasGroup.alpha + Time.deltaTime / fadeInDuration);

            // Initialize approach ring now if it was delayed and fade-in is complete
            if (!ringInitialized && canvasGroup.alpha >= 1f && circle != null && approachRing != null)
            {
                float startScale = (approachRing.rect.width / circle.rect.width) * 1.5f;
                approachRing.localScale = Vector3.one * startScale;
                ringInitialized = true;
            }
        }

        // Shrink the approach ring after fade-in (or immediately if not delayed)
        if (ringInitialized && circle != null && approachRing != null)
        {
            float timeToHit = hitTime - Time.time;
            float t = 1 - (timeToHit / approachTime);
            t = Mathf.Clamp01(t);

            float startScale = (approachRing.rect.width / circle.rect.width) * 1.5f;
            approachRing.localScale = Vector3.one * Mathf.Lerp(startScale, 1f, t);
            if (timeToHit < -0.3f)
            {
                isDestroyed = true;
                Destroy(gameObject);
            }
        }
    }

    public void ShowHit()
    {
        if (isDestroyed) return;

        // quick flash or scale pop
        transform.localScale = Vector3.one * 1.2f;
        Invoke(nameof(ResetScale), 0.1f);
    }

    void ResetScale()
    {
        transform.localScale = Vector3.one;
    }

    public void ShowMiss()
    {
        if (isDestroyed || missX == null) return;

        missX.SetActive(true);
        Invoke(nameof(HideMiss), 0.3f);
    }

    void HideMiss()
    {
        if (missX != null) missX.SetActive(false);
    }
}