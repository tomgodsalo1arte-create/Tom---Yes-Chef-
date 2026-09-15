using System.Collections;
using UnityEngine;

public class StoveSlot : MonoBehaviour
{
    [SerializeField] private Transform ingredientPoint;
    [SerializeField] private PreparationProgressUI progressUI;
    private Ingredient currentIngredient;
    private Coroutine cookingCoroutine;

    public bool IsAvailable => currentIngredient == null;
    public bool IsCooking { get; private set; }

    public float RemainingCookingTime { get; private set; }

      public bool TryPlaceIngredient(IngredientHolder holder)
    {
        if (holder == null || !IsAvailable || !holder.HasIngredient)
            return false;
    
        Ingredient ingredient = holder.HeldIngredient;
    
        if (ingredient.Type != IngredientType.Meat)
            return false;
    
        if (ingredient.IsPrepared)
            return false;
    
        ingredient = holder.RemoveIngredient();
    
        currentIngredient = ingredient;
    
        currentIngredient.transform.SetParent(ingredientPoint);
        currentIngredient.transform.SetLocalPositionAndRotation(
            Vector3.zero,
            Quaternion.identity
        );
    
        cookingCoroutine = StartCoroutine(CookIngredient());
    
        return true;
    }
   public bool TryTakeIngredient(IngredientHolder holder)
    {
        if (IsCooking || currentIngredient == null)
            return false;
    
        if (!holder.TryHold(currentIngredient))
            return false;
    
        currentIngredient = null;
    
        return true;
    }

    private IEnumerator CookIngredient()
    {
        IsCooking = true;

        float totalCookingTime =
            currentIngredient.Data.PreparationTime;

        RemainingCookingTime = totalCookingTime;

        while (RemainingCookingTime > 0f)
        {
            RemainingCookingTime -= Time.deltaTime;

            progressUI.Show(
                RemainingCookingTime,
                totalCookingTime
            );

            yield return null;
        }

        RemainingCookingTime = 0f;

        currentIngredient.SetPrepared();

        IsCooking = false;
        cookingCoroutine = null;

        progressUI.Hide();

        Debug.Log("Meat cooking complete.");
    }
    public void ResetSlot()
    {
        if (cookingCoroutine != null)
        {
            StopCoroutine(cookingCoroutine);
            cookingCoroutine = null;
        }

        if (currentIngredient != null)
        {
            Destroy(currentIngredient.gameObject);
            currentIngredient = null;
        }

        RemainingCookingTime = 0f;
        IsCooking = false;

        if (progressUI != null)
        {
            progressUI.Hide();
        }
    }
}