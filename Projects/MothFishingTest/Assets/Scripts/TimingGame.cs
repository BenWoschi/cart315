using UnityEngine;
using System.Collections;

public class TimingGame : MonoBehaviour
{
    public LivesUI livesUI;
    public FishingInteractable fishingInteractable;
    public MonoBehaviour playerMovement;

    public RectTransform needle;
    public NeedleMover needleMover;

    public RectTransform targetZone;
    public TargetZoneController zoneController;

    public float shrinkAmount = 15f;
    public float growAmount = 20f;

    public int maxMistakes = 3;
    private int mistakes = 0;

    public float winWidth = 60f;
    public float loseWidth = 280f;

    // 🔥 NEW: store defaults
    private float initialNeedleSpeed;
    private Vector2 initialNeedlePos;
    private float initialZoneWidth;

    void Awake()
    {
        // Save starting values ONCE
        initialNeedleSpeed = needleMover.speed;
        initialNeedlePos = needle.anchoredPosition;
        initialZoneWidth = zoneController.currentWidth;
    }

    public void StartGame()
    {
        livesUI.Show();
        livesUI.ResetLives();
        // 🔥 THIS is the fix
        gameObject.SetActive(true);

        // Disable player movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Reset AFTER activation
        ResetGame();
    }

    void ResetGame()
    {
        mistakes = 0;

        // Reset needle
        needle.anchoredPosition = initialNeedlePos;
        needleMover.speed = initialNeedleSpeed;

        // Reset zone
        zoneController.SetWidth(initialZoneWidth);

        // Make sure movement is active
        needleMover.enabled = false;
        StartCoroutine(StartNeedleNextFrame());
    }

    IEnumerator StartNeedleNextFrame()
    {
        yield return null; // wait 1 frame
        needleMover.enabled = true; // start moving again
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

    void CheckHit()
    {
        float needleX = needle.anchoredPosition.x;
        float zoneX = targetZone.anchoredPosition.x;
        float halfWidth = targetZone.sizeDelta.x / 2f;

        bool hit = Mathf.Abs(needleX - zoneX) <= halfWidth;

        if (hit)
        {
            Debug.Log("HIT");

            zoneController.SetWidth(zoneController.currentWidth - shrinkAmount);
            needleMover.speed += 0.2f;

            CheckWin();
        }
        else
        {
            Debug.Log("MISS");
            livesUI.LoseLife();
            mistakes++;
            ScreenShake.Instance?.Shake(0.15f, 0.2f);
            zoneController.SetWidth(zoneController.currentWidth + growAmount);

            CheckLose();
        }
    }

    void CheckWin()
    {
        if (zoneController.currentWidth <= winWidth)
        {
            Debug.Log("SUCCESS!");
            EndGame(true);
        }
    }

    void CheckLose()
    {
        if (mistakes >= maxMistakes || zoneController.currentWidth >= loseWidth)
        {
            Debug.Log("FAILED!");
            EndGame(false);
        }
    }

    void EndGame(bool success)
    {
        livesUI.Hide();
        // Re-enable movement
        if (playerMovement != null)
            playerMovement.enabled = true;

        // Stop needle
        needleMover.enabled = false;

        // 🔥 IMPORTANT: tell fishing system we're done
        if (fishingInteractable != null)
            fishingInteractable.EndFishing();

        // ✅ NEW: update energy system
        if (EnergyManager.Instance != null)
        {
            if (success)
                EnergyManager.Instance.OnFishingSuccess();
            else
                EnergyManager.Instance.OnFishingFail();
        }

        // Hide minigame
        gameObject.SetActive(false);
    }
}