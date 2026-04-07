using UnityEngine;
using System.Collections;

public class FishingInteractable : MonoBehaviour
{
    public TimingGame timingGame;
    public RhythmGameManager rhythmGame;

    [Header("Camera")]
    public Camera mainCamera;
    public Transform player; // drag your Player here
    public float zoomFOV = 40f;
    public float zoomDuration = 1f;

    private float originalFOV;
    [Header("Rod")]
    public GameObject fishingRod;
    public Transform handBone;         // The bone the rod will attach to

    [Header("References")]
    public Animator playerAnimator;    // Animator on the player
    public MothMovement playerMovement; // Your movement script
    public GameObject fishingUI;      // Fishing minigame UI

    [Header("Interaction")]
    public bool playerInRange = false; // Set via trigger
    public GameObject interactPrompt;  // "Press E" prompt UI

    private bool isFishing = false;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        if (mainCamera != null)
            originalFOV = mainCamera.fieldOfView;
    }

    void Update()
    {
        // Only allow starting fishing if in range and not already fishing
        if (playerInRange && !isFishing && Input.GetKeyDown(KeyCode.E))
        {
            StartFishing();
        }
    }

    // Called when pressing E
    public void StartFishing()
    {
        StartCoroutine(ZoomCamera(zoomFOV));
        isFishing = true;

        // Freeze movement
        if (playerMovement != null)
            playerMovement.enabled = false;

        // Show rod and attach to hand
        if (fishingRod != null && handBone != null)
        {
            fishingRod.SetActive(true);
            fishingRod.transform.SetParent(handBone);
            fishingRod.transform.localPosition = Vector3.zero;
            fishingRod.transform.localRotation = Quaternion.Euler(180f, 90f, 0f);
        }

        // Spawn or activate the bobber
        FishingLineController lineController = fishingRod.GetComponent<FishingLineController>();
        if (lineController != null)
        {
            if (lineController.bobber != null)
            {
                lineController.bobber.position = lineController.rodTip.position + lineController.rodTip.forward * 2f;
                lineController.bobber.gameObject.SetActive(true);
            }

            if (lineController.lineRenderer != null)
                lineController.lineRenderer.enabled = true;

            // Launch bobber using the rodTip from lineController
            BobberController bob = lineController.bobber.GetComponent<BobberController>();
            if (bob != null)
            {
                bob.Launch(lineController.rodTip.forward, 5f);
            }
        }

        // Start Cast animation
        playerAnimator.SetTrigger("Cast");

        // Hide interaction prompt
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    // 🎬 Called by Animation Event at end of Cast
    public void OnCastFinished()
    {
        // Start Idle loop animation
        playerAnimator.SetTrigger("Idle");

        // Begin random bite wait
        StartCoroutine(WaitForBite());
    }

    IEnumerator WaitForBite()
    {
        float waitTime = Random.Range(5f, 20f);
        yield return new WaitForSeconds(waitTime);

        // Play Reel animation
        playerAnimator.SetTrigger("Reel");
    }

    // 🎬 Called by Animation Event at end of Reel
    public void OnReelFinished()
    {
        StartCoroutine(ZoomCamera(originalFOV));
        // Hide the rod
        if (fishingRod != null)
            fishingRod.SetActive(false);
        // Activate minigame UI
        // --- Choose random minigame ---
        int choice = Random.Range(0, 3);

        switch (choice)
        {
            case 0:
                Debug.Log("Fishing bar minigame");
                if (fishingUI != null)
                    fishingUI.SetActive(true);
                break;

            case 1:
                Debug.Log("Timing minigame");
                if (timingGame != null)
                    timingGame.StartGame();
                break;

            case 2:
                Debug.Log("Rhythm minigame");
                if (rhythmGame != null)
                    rhythmGame.gameObject.SetActive(true);
                break;
        }

        // Optionally, you can re-enable movement here after minigame ends
        // playerMovement.enabled = true;
        // isFishing = false;

        // Hide rod, line, and bobber
        if (fishingRod != null)
        {
            fishingRod.SetActive(false);

            FishingLineController lineController = fishingRod.GetComponent<FishingLineController>();
            if (lineController != null)
            {
                if (lineController.bobber != null)
                    lineController.bobber.gameObject.SetActive(false);
                if (lineController.lineRenderer != null)
                    lineController.lineRenderer.enabled = false;
            }
        }
    }

    // These are called by your trigger area
    public void PlayerEnterRange()
    {
        playerInRange = true;
        if (interactPrompt != null)
            interactPrompt.SetActive(true);
    }

    public void PlayerExitRange()
    {
        playerInRange = false;
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }
    IEnumerator ZoomCamera(float targetFOV)
    {
        float startFOV = mainCamera.fieldOfView;
        float time = 0f;

        while (time < zoomDuration)
        {
            time += Time.deltaTime;
            float t = time / zoomDuration;

            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        mainCamera.fieldOfView = targetFOV;
    }
    public void EndFishing()
    {
        Debug.Log("EndFishing called");

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (playerAnimator != null)
        {
            playerAnimator.Rebind();   // 🔥 THIS IS THE KEY
            playerAnimator.Update(0f);
        }

        isFishing = false;
    }
}