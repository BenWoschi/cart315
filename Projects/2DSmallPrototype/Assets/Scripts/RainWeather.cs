using UnityEngine;

public class RainWeather : MonoBehaviour
{
    public static RainWeather Instance;

    public GameObject rainPrefab;
    private GameObject rainInstance;


    public float duration = 10f;
    public float fadeTime = 0.5f;
    public float slowIntensity = 0.4f;

    private Vector3 targetScale;
    private float timer = 0f;
    private bool active = false;
    private bool fadingIn = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        targetScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !active)
        {
            ActivateRain();
        }

        if (!active) return;

        timer += Time.deltaTime;

        // Fade in effect
        if (fadingIn)
        {
            float progress = timer / fadeTime;
            rainInstance.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, progress);

            if (progress >= 1f)
            {
                fadingIn = false;
                timer = 0f;
            }
        }
        else
        {
            // Active duration countdown
            if (timer >= duration)
            {
                DeactivateRain();
            }
        }
    }

    void ActivateRain()
    {
        if (rainInstance == null)
        {
            rainInstance = Instantiate(rainPrefab, Vector3.zero, Quaternion.identity);
        }

        active = true;
        fadingIn = true;
        timer = 0f;

        targetScale = rainInstance.transform.localScale;
        rainInstance.transform.localScale = Vector3.zero;
    }


    void DeactivateRain()
    {
        active = false;

        if (rainInstance != null)
        {
            Destroy(rainInstance);
            rainInstance = null;
        }
    }

    public float GetSlowMultiplier()
    {
        return active ? slowIntensity : 1f;
    }
}
