using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private const float GameDuration = 180f;

    public float RemainingTime { get; private set; }
    public bool IsRunning { get; private set; }

    public event Action TimerFinished;

    private void Update()
    {
        if (!IsRunning)
            return;

        RemainingTime -= Time.deltaTime;
       // Debug.Log($"Remaining Time: {RemainingTime} seconds");
        if (RemainingTime <= 0f)
        {
            RemainingTime = 0f;
            StopTimer();

            TimerFinished?.Invoke();
        }
    }

    public void StartTimer()
    {
        RemainingTime = GameDuration;
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }
}