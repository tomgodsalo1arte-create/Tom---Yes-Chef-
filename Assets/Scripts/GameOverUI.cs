using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;

    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text highScoreText;

    private void OnEnable()
    {
        gameManager.GameOver += Show;
    }

    private void OnDisable()
    {
        gameManager.GameOver -= Show;
    }

    private void Show()
    {
        gameOverPanel.SetActive(true);

        finalScoreText.text =
            $"FINAL SCORE: {scoreManager.CurrentScore}";

        highScoreText.text =
            $"HIGH SCORE: {scoreManager.HighScore}";
    }

    public void Restart()
    {
        gameOverPanel.SetActive(false);

        gameManager.RestartGame();
    }
}