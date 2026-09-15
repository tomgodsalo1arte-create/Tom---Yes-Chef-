using System;
using System.Collections.Generic;

public class Order
{
    private readonly List<IngredientType> originalIngredients;
    private readonly List<IngredientType> remainingIngredients;

    private readonly int baseScore;

    public IReadOnlyList<IngredientType> OriginalIngredients => originalIngredients;
    public IReadOnlyList<IngredientType> RemainingIngredients => remainingIngredients;

    public float StartTime { get; }

    public bool IsComplete => remainingIngredients.Count == 0;

    public Order(
        IReadOnlyList<IngredientType> ingredients,
        int baseScore,
        float startTime)
    {
        originalIngredients = new List<IngredientType>(ingredients);
        remainingIngredients = new List<IngredientType>(ingredients);

        this.baseScore = baseScore;
        StartTime = startTime;
    }

    public bool TryFulfill(Ingredient ingredient)
    {
        if (ingredient == null || !ingredient.IsPrepared)
            return false;

        int index = remainingIngredients.IndexOf(ingredient.Type);

        if (index < 0)
            return false;

        remainingIngredients.RemoveAt(index);

        return true;
    }

    public int CalculateScore(float currentTime)
    {
        int elapsedSeconds = Math.Max(
            0,
            (int)Math.Floor(currentTime - StartTime)
        );

        return baseScore - elapsedSeconds;
    }
}