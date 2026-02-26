using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class GestureRecognizer : MonoBehaviour
{
    public Image drawingOverlay;
    public Camera gameCamera;       // Main camera
    public float minDistance = 0.1f; // Min distance between points
    public float cooldown = 1f;      // Time between gesture recognition
    public float lineWidth = 0.05f;  // Line width
    public int smoothSteps = 5;      // Number of interpolation steps between points for smoothing

    private List<Vector3> rawPoints = new List<Vector3>(); // Raw input points
    private List<Vector3> smoothedPoints = new List<Vector3>(); // Smoothed points for LineRenderer
    private LineRenderer lineRenderer;
    private bool isDrawing = false;
    private float lastRecognitionTime = 0f;
    bool gestureCompleted = false;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        if (gameCamera == null)
            gameCamera = Camera.main;
    }

    void Update()
    {
        bool drawingMode = Input.GetKey(KeyCode.Tab) && !gestureCompleted;

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            gestureCompleted = false;
        }

        // Enable overlay
        if (drawingOverlay != null)
            drawingOverlay.enabled = drawingMode;

        // Pause the game
        Time.timeScale = drawingMode ? 0f : 1f;

        // Only allow drawing while Tab is held
        if (!drawingMode) return;

        // --- Existing drawing code here ---
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            rawPoints.Clear();
            smoothedPoints.Clear();
            lineRenderer.positionCount = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;

            if (smoothedPoints.Count > 5 && Time.unscaledTime - lastRecognitionTime > cooldown)
            {
                RecognizeGesture();
                lastRecognitionTime = Time.unscaledTime;
            }

            lineRenderer.positionCount = 0; // Clear stroke
        }

        if (isDrawing)
        {
            Vector3 mouseWorldPos = gameCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            if (rawPoints.Count == 0 || Vector3.Distance(rawPoints[rawPoints.Count - 1], mouseWorldPos) > minDistance)
            {
                rawPoints.Add(mouseWorldPos);
                UpdateSmoothedLine();
            }
        }
    }

    // Smooth the line by interpolating between points
    void UpdateSmoothedLine()
    {
        smoothedPoints.Clear();
        if (rawPoints.Count < 2) return;

        for (int i = 0; i < rawPoints.Count - 1; i++)
        {
            Vector3 start = rawPoints[i];
            Vector3 end = rawPoints[i + 1];

            for (int j = 0; j < smoothSteps; j++)
            {
                float t = j / (float)smoothSteps;
                smoothedPoints.Add(Vector3.Lerp(start, end, t));
            }
        }
        smoothedPoints.Add(rawPoints[rawPoints.Count - 1]); // add last point

        lineRenderer.positionCount = smoothedPoints.Count;
        lineRenderer.SetPositions(smoothedPoints.ToArray());
    }

    // Normalize points for shape recognition
    List<Vector2> NormalizePoints(List<Vector3> input)
    {
        Vector2 center = Vector2.zero;
        foreach (var p in input) center += (Vector2)p;
        center /= input.Count;

        List<Vector2> normalized = new List<Vector2>();
        foreach (var p in input) normalized.Add((Vector2)p - center);

        float maxDist = 0f;
        foreach (var p in normalized)
            maxDist = Mathf.Max(maxDist, Mathf.Abs(p.x), Mathf.Abs(p.y));

        if (maxDist > 0)
        {
            for (int i = 0; i < normalized.Count; i++)
                normalized[i] /= maxDist;
        }

        return normalized;
    }

    // Simple tear drop shape detection
    bool IsTearDropShape(List<Vector2> points)
    {
        if (points.Count < 5) return false;

        // 1. Find top and bottom points
        Vector2 top = points[0];
        Vector2 bottom = points[0];
        foreach (var p in points)
        {
            if (p.y > top.y) top = p;
            if (p.y < bottom.y) bottom = p;
        }

        // 2. Aspect ratio: height vs width
        float height = top.y - bottom.y;
        float maxWidth = 0f;
        foreach (var p in points)
        {
            float distX = Mathf.Abs(p.x - (top.x + bottom.x) / 2f); // distance from vertical center
            if (distX > maxWidth) maxWidth = distX;
        }

        float aspectRatio = height / (maxWidth * 2f); // height / width
        if (aspectRatio < 1.2f) return false; // too flat, not tear drop

        // 3. Width at top vs bottom
        float widthTop = 0f;
        float widthBottom = 0f;
        foreach (var p in points)
        {
            if (Mathf.Abs(p.y - top.y) < 0.1f) widthTop = Mathf.Max(widthTop, Mathf.Abs(p.x - top.x));
            if (Mathf.Abs(p.y - bottom.y) < 0.1f) widthBottom = Mathf.Max(widthBottom, Mathf.Abs(p.x - bottom.x));
        }

        if (widthBottom <= widthTop) return false; // must be wider at bottom

        // 4. Optional: check curvature / side tapering
        // Compute average slope of sides (rough approximation)
        Vector2 leftMost = points[0];
        Vector2 rightMost = points[0];
        foreach (var p in points)
        {
            if (p.x < leftMost.x) leftMost = p;
            if (p.x > rightMost.x) rightMost = p;
        }
        float leftSlope = (top.y - bottom.y) / Mathf.Max(0.01f, leftMost.x - top.x);
        float rightSlope = (top.y - bottom.y) / Mathf.Max(0.01f, rightMost.x - top.x);
        // Not strictly necessary, but can tune if needed

        return true;
    }

    void RecognizeGesture()
    {
        List<Vector2> normalized = NormalizePoints(smoothedPoints);

        if (IsTearDropShape(normalized))
        {
            Debug.Log("Tear drop recognized! Summoning rain.");
            RainWeather.Instance.SummonRain();
            gestureCompleted = true;
            Time.timeScale = 1f;

            lineRenderer.positionCount = 0;
        }
        else
        {
            Debug.Log("Gesture not recognized.");
        }
    }
}