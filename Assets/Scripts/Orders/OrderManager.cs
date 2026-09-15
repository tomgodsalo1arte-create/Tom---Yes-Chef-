using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private IngredientData[] availableIngredients;
    [SerializeField] private CustomerWindow[] customerWindows;
    private readonly List<Coroutine> respawnCoroutines = new();
    public event Action<int> ScoreEarned;

    public void Initialize()
    {
        foreach (CustomerWindow window in customerWindows)
        {
            AssignNewOrder(window);
        }
    }

    public Order CreateOrder()
    {
        int ingredientCount = UnityEngine.Random.value < 0.5f ? 2 : 3;

        List<IngredientType> ingredients =
            new List<IngredientType>(ingredientCount);

        int baseScore = 0;

        for (int i = 0; i < ingredientCount; i++)
        {
            IngredientData data = GetRandomIngredient();

            if (data == null)
                return null;

            ingredients.Add(data.Type);
            baseScore += data.ScoreValue;
        }

        return new Order(
            ingredients,
            baseScore,
            Time.time
        );
    }

    private void AssignNewOrder(CustomerWindow window)
    {
        Order order = CreateOrder();

        if (order == null)
            return;

        window.SetOrder(order);

        window.OrderCompleted -= HandleOrderCompleted;
        window.OrderCompleted += HandleOrderCompleted;
    }

    public void HandleOrderCompleted(
    CustomerWindow window,
    int score)
    {
        ScoreEarned?.Invoke(score);

        Coroutine coroutine = StartCoroutine(
            RespawnOrder(window)
        );

        respawnCoroutines.Add(coroutine);
    }
    private IEnumerator RespawnOrder(CustomerWindow window)
    {
        yield return new WaitForSeconds(5f);

        AssignNewOrder(window);
    }

    private IngredientData GetRandomIngredient()
    {
        if (availableIngredients == null ||
            availableIngredients.Length == 0)
        {
            Debug.LogError(
                "OrderManager has no ingredient data configured.",
                this);

            return null;
        }

        return availableIngredients[
            UnityEngine.Random.Range(
                0,
                availableIngredients.Length)
        ];
    }
    public void ResetOrders()
    {
        foreach (Coroutine coroutine in respawnCoroutines)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }

        respawnCoroutines.Clear();

        foreach (CustomerWindow window in customerWindows)
        {
            window.ClearOrder();
        }
    }

}