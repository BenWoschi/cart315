using UnityEngine;

public class GameStartFlow : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject explanationPanel;

    public GameObject player;
    public GameObject energyUI;

    public Camera menuCamera;
    public Camera mainCamera;

    private int state = 0;
    // 0 = menu
    // 1 = explanation
    // 2 = gameplay

    void Start()
    {
        menuPanel.SetActive(true);
        explanationPanel.SetActive(false);

        player.SetActive(false);
        energyUI.SetActive(false);

        menuCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        if (state == 0)
        {
            // Menu → Explanation
            menuPanel.SetActive(false);
            explanationPanel.SetActive(true);
            state = 1;
        }
        else if (state == 1)
        {
            // Explanation → Game
            explanationPanel.SetActive(false);

            menuCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);

            player.SetActive(true);
            energyUI.SetActive(true);

            state = 2;
        }
    }
}