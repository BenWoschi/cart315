using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public RawImage[] lifeIcons;

    private int maxLives = 3;
    private int currentLives;

    void Awake()
    {
        ResetLives();
        gameObject.SetActive(false); // hidden by default
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ResetLives()
    {
        currentLives = maxLives;

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].enabled = true;
        }
    }

    public void LoseLife()
    {
        if (currentLives <= 0) return;

        currentLives--;

        lifeIcons[currentLives].enabled = false;

        if (currentLives <= 0)
        {
            OnOutOfLives();
        }
    }

    void OnOutOfLives()
    {
        Debug.Log("Game Over");
        // You can call your minigame fail logic here
    }

    public int GetLives()
    {
        return currentLives;
    }
}