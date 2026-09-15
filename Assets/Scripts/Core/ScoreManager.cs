using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    [SerializeField] private OrderManager orderManager;

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    public event Action<int> ScoreChanged;
    public event Action<int> HighScoreChanged;
    public event Action<int> NewHighScore;

    private void Awake()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void OnEnable()
    {
        orderManager.ScoreEarned += AddScore;
    }

    private void OnDisable()
    {
        orderManager.ScoreEarned -= AddScore;
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        ScoreChanged?.Invoke(CurrentScore);
    }

    private void AddScore(int score)
    {
        CurrentScore += score;

     
        ScoreChanged?.Invoke(CurrentScore);

        if (CurrentScore <= HighScore)
            return;

        HighScore = CurrentScore;

        PlayerPrefs.SetInt(HighScoreKey, HighScore);
        PlayerPrefs.Save();

        HighScoreChanged?.Invoke(HighScore);
        NewHighScore?.Invoke(HighScore);
    }
}