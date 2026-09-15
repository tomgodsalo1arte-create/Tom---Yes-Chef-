using UnityEngine;

public class StartUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject hud;

    public void OnStartButtonPressed()
    {
        gameManager.StartGame();

        startPanel.SetActive(false);
        hud.SetActive(true);
    }
}