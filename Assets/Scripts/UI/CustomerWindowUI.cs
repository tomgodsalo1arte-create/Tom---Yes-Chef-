using TMPro;
using UnityEngine;
using System.Collections;

public class CustomerWindowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scorePopupText;
    [SerializeField] private float scorePopupDuration = 2f;

    private Coroutine scorePopupCoroutine;
    [Header("Ingredient Display")]
    [SerializeField] private TMP_Text[] ingredientTexts;

    [Header("Order Timer")]
    [SerializeField] private TMP_Text orderTimerText;

    private Order currentOrder;

    private void Update()
    {
        UpdateOrderTimer();
    }
    public void ShowScorePopup(int score)
    {
        if (scorePopupCoroutine != null)
        {
            StopCoroutine(scorePopupCoroutine);
        }

        scorePopupCoroutine = StartCoroutine(
            ShowScorePopupRoutine(score)
        );
    }
    private IEnumerator ShowScorePopupRoutine(int score)
    {
        scorePopupText.gameObject.SetActive(true);

        scorePopupText.text = score >= 0
            ? $"+{score}"
            : score.ToString();

        Color color = scorePopupText.color;
        color.a = 1f;
        scorePopupText.color = color;

        float elapsed = 0f;

        while (elapsed < scorePopupDuration)
        {
            elapsed += Time.deltaTime;

            float alpha =
                1f - Mathf.Clamp01(elapsed / scorePopupDuration);

            color.a = alpha;
            scorePopupText.color = color;

            yield return null;
        }

        scorePopupText.gameObject.SetActive(false);
        scorePopupCoroutine = null;
    }
    public void DisplayOrder(Order order)
    {
        currentOrder = order;

        ClearIngredientTexts();

        if (currentOrder == null)
        {
            orderTimerText.text = string.Empty;
            return;
        }

        for (int i = 0; i < ingredientTexts.Length; i++)
        {
            if (i < currentOrder.RemainingIngredients.Count)
            {
                ingredientTexts[i].text =
                    GetIngredientLabel(
                        currentOrder.RemainingIngredients[i]
                    );
            }
            else
            {
                ingredientTexts[i].text = string.Empty;
            }
        }

        UpdateOrderTimer();
    }

    public void Clear()
    {
        currentOrder = null;

        ClearIngredientTexts();

        orderTimerText.text = string.Empty;
    }

    private void ClearIngredientTexts()
    {
        foreach (TMP_Text text in ingredientTexts)
        {
            text.text = string.Empty;
        }
    }

    private void UpdateOrderTimer()
    {
        if (currentOrder == null)
        {
            orderTimerText.text = string.Empty;
            return;
        }

        float elapsedTime = Time.time - currentOrder.StartTime;
        int elapsedSeconds = Mathf.FloorToInt(elapsedTime);

        int minutes = elapsedSeconds / 60;
        int seconds = elapsedSeconds % 60;

        orderTimerText.text = $"OPEN: {minutes:00}:{seconds:00}";
    }

    private string GetIngredientLabel(IngredientType type)
    {
        return type switch
        {
            IngredientType.Vegetable => "VEG",
            IngredientType.Cheese => "CHEESE",
            IngredientType.Meat => "MEAT",
            _ => "?"
        };
    }
}