using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    [Header("Game Win")]
    public GameObject gameWinPanel;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public GameObject player;
    private Vector3 playerStartPosition;

    [Header("UI Smoothing")]
    public float smoothSpeed = 5f;
    private float displayedEnergy;

    [Header("Player")]
    public MonoBehaviour playerMovement;
    public static EnergyManager Instance;

    [Header("Energy")]
    [Range(0f, 1f)] public float energy = 0.2f; // start at 20%
    public Slider energyBar;

    [Header("Fish Popups")]
    public GameObject popupPanel;
    public GameObject fishCommon;   // 10%
    public GameObject fishUncommon; // 25%
    public GameObject fishRare;     // 50%

    private bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HideAllFish();
        displayedEnergy = energy;
        popupPanel.SetActive(false);
        playerStartPosition = player.transform.position;
    }

    void Update()
    {
        if (popupPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            popupPanel.SetActive(false);
            HideAllFish();

            // 🔓 Re-enable movement
            if (playerMovement != null)
                playerMovement.enabled = true;
        }

        // Smoothly move UI toward actual energy
        displayedEnergy = Mathf.Lerp(displayedEnergy, energy, Time.deltaTime * smoothSpeed);

        if (energyBar != null)
            energyBar.value = displayedEnergy;
    }

    // 🎣 Called when fishing SUCCESS
    public void OnFishingSuccess()
    {
        if (isGameOver) return;

        float roll = Random.value;

        if (roll <= 0.05f)
        {
            GainEnergy(0.5f);
            ShowFish(fishRare);
        }
        else if (roll <= 0.40f)
        {
            GainEnergy(0.25f);
            ShowFish(fishUncommon);
        }
        else
        {
            GainEnergy(0.10f);
            ShowFish(fishCommon);
        }
    }

    // ❌ Called when fishing FAIL
    public void OnFishingFail()
    {
        if (isGameOver) return;

        GainEnergy(-0.10f);
    }

    void GainEnergy(float amount)
    {
        energy += amount;
        energy = Mathf.Clamp01(energy);

        if (energy <= 0f)
        {
            GameOver(false);
        }
        else if (energy >= 1f)
        {
            GameOver(true);
        }
    }

    void ShowFish(GameObject fish)
    {
        popupPanel.SetActive(true);
        HideAllFish();
        fish.SetActive(true);

        // 🔒 Disable movement
        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    void HideAllFish()
    {
        fishCommon.SetActive(false);
        fishUncommon.SetActive(false);
        fishRare.SetActive(false);
    }

    void GameOver(bool win)
    {
        isGameOver = true;

        Debug.Log(win ? "YOU WIN" : "GAME OVER");

        if (player != null)
            player.SetActive(false);

        popupPanel.SetActive(false);

        if (win)
        {
            if (gameWinPanel != null)
                gameWinPanel.SetActive(true);
        }
        else
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }
    }

    public void RetryGame()
    {
        energy = 0.2f;
        displayedEnergy = energy;

        if (energyBar != null)
            energyBar.value = energy;

        isGameOver = false;

        player.transform.position = playerStartPosition;
        player.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (gameWinPanel != null)
            gameWinPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        Application.Quit();
    }
}