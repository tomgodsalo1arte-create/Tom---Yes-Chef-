using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;

    private void OnEnable()
    {
        scoreManager.ScoreChanged += UpdateCurrentScore;
        scoreManager.HighScoreChanged += UpdateHighScore;

        UpdateCurrentScore(scoreManager.CurrentScore);
        UpdateHighScore(scoreManager.HighScore);
    }

    private void OnDisable()
    {
        scoreManager.ScoreChanged -= UpdateCurrentScore;
        scoreManager.HighScoreChanged -= UpdateHighScore;
    }

    private void UpdateCurrentScore(int score)
    {
        currentScoreText.text = $"SCORE: {score}";
    }

    private void UpdateHighScore(int score)
    {
        highScoreText.text = $"HIGH SCORE: {score}";
    }
}