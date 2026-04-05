using UnityEngine;
using UnityEngine.UI;

public class FishingController : MonoBehaviour
{
    public FishingInteractable fishingInteractable;
    private bool isGameOver = false;

    [Header("Player Settings")]
    public RectTransform playerBar;
    public RectTransform bigBar;

    private float minX;
    private float maxX;

    [Header("Fish Settings")]
    public RectTransform fishRect;
    public float fishSpeed = 150f;
    private float targetX;
    private float waitTimer = 0f;
    public float waitTime = 0.5f;

    [Header("Exhaustion Settings")]
    public Slider exhaustionMeter;
    public float gainRate = 0.3f; // rate per second when fish is inside
    public float lossRate = 0.2f; // rate per second when fish is outside

    void Start()
    {
        if (playerBar == null || bigBar == null || fishRect == null || exhaustionMeter == null)
        {
            enabled = false;
            return;
        }

        // Player bar boundaries
        float halfBigBar = bigBar.rect.width / 2f;
        float halfPlayerBar = playerBar.rect.width / 2f;

        minX = -halfBigBar + halfPlayerBar;
        maxX = halfBigBar - halfPlayerBar;

        // Fish initial target
        SetRandomTarget();

        exhaustionMeter.value = 0.75f;
    }

    void Update()
    {
        // --- Player bar movement ---
        float mouseX = Input.mousePosition.x / Screen.width;
        float edgeFactor = Mathf.SmoothStep(0f, 1f, mouseX);
        float newPlayerX = Mathf.Lerp(minX, maxX, edgeFactor);
        playerBar.anchoredPosition = new Vector2(newPlayerX, playerBar.anchoredPosition.y);

        // --- Fish movement ---
        float newFishX = Mathf.MoveTowards(
    fishRect.anchoredPosition.x,
    targetX,
    fishSpeed * Time.deltaTime
);

        fishRect.anchoredPosition = new Vector2(newFishX, fishRect.anchoredPosition.y);

        if (Mathf.Abs(newFishX - targetX) < 0.1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                SetRandomTarget();
                waitTimer = 0f;
            }
        }

        // --- Exhaustion meter ---
        bool isCaught = RectOverlap(playerBar, fishRect);

        if (isCaught)
            exhaustionMeter.value += gainRate * Time.deltaTime;
        else
            exhaustionMeter.value -= lossRate * Time.deltaTime;

        exhaustionMeter.value = Mathf.Clamp01(exhaustionMeter.value);

        if (exhaustionMeter.value >= 1f)
        {
            EndMinigame(true);
        }
        else if (exhaustionMeter.value <= 0f)
        {
            EndMinigame(false);
        }
    }

    void SetRandomTarget()
    {
        float halfBigBar = bigBar.rect.width / 2f;
        float halfFish = fishRect.rect.width / 2f;
        targetX = Random.Range(-halfBigBar + halfFish, halfBigBar - halfFish);
    }

    bool RectOverlap(RectTransform a, RectTransform b)
    {
        float aMin = a.anchoredPosition.x - a.rect.width / 2f;
        float aMax = a.anchoredPosition.x + a.rect.width / 2f;

        float bMin = b.anchoredPosition.x - b.rect.width / 2f;
        float bMax = b.anchoredPosition.x + b.rect.width / 2f;

        return (bMax >= aMin && bMin <= aMax);
    }
    void EndMinigame(bool success)
    {
        if (fishingInteractable != null)
        {
            fishingInteractable.EndFishing(); // you’ll create this
        }

        if (isGameOver) return;
        isGameOver = true;

        if (success)
            Debug.Log("Success!");
        else
            Debug.Log("Line broke!");

        // Stop updates
        enabled = false;

        // Hide UI (optional)
        gameObject.SetActive(false);
    }
}