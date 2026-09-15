using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private OrderManager orderManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private IngredientHolder ingredientHolder;
    [SerializeField] private ChoppingTable choppingTable;
    [SerializeField] private Stove stove;
    public GameState CurrentState { get; private set; }

    public event Action GameOver;

    private void OnEnable()
    {
        gameTimer.TimerFinished += HandleTimerFinished;
    }

    private void OnDisable()
    {
        gameTimer.TimerFinished -= HandleTimerFinished;
    }

    private void Start()
    {
        SetState(GameState.Start);
    }

    public void StartGame()
    {
        SetState(GameState.Playing);

        scoreManager.ResetScore();
        orderManager.Initialize();
        gameTimer.StartTimer();
    }

    public void EndGame()
    {
        gameTimer.StopTimer();
        SetState(GameState.GameOver);

        GameOver?.Invoke();

        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        // Stop old gameplay state.
        gameTimer.StopTimer();

        // Clear player/station runtime state.
        ingredientHolder.ResetHolder();
        choppingTable.ResetTable();
        stove.ResetStove();

        // Clear old orders and score.
        orderManager.ResetOrders();
        scoreManager.ResetScore();

        // Start completely fresh game.
        SetState(GameState.Playing);

        orderManager.Initialize();
        gameTimer.StartTimer();
    }

    private void HandleTimerFinished()
    {
        EndGame();
    }

    private void SetState(GameState newState)
    {
        CurrentState = newState;
    }
}