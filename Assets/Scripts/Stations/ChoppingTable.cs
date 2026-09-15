using System.Collections;
using UnityEngine;

public class ChoppingTable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform ingredientPoint;
    [SerializeField] private PreparationProgressUI progressUI;
    private float totalPreparationTime;

    private Ingredient currentIngredient;
    private Coroutine preparationCoroutine;

    public bool IsBusy => currentIngredient != null;
    public bool IsPreparing { get; private set; }

    public float RemainingPreparationTime { get; private set; }

    public void Interact(PlayerController player)
    {
        IngredientHolder holder = player.GetIngredientHolder();

        // Table has an ingredient on it.
        if (currentIngredient != null)
        {
            TryPickupPreparedIngredient(holder);
            return;
        }

        // Table is empty, so try to place an ingredient.
        TryPlaceIngredient(holder);
    }

    private void TryPlaceIngredient(IngredientHolder holder)
    {
        if (!holder.HasIngredient)
            return;

        Ingredient ingredient = holder.HeldIngredient;

        if (ingredient.Type != IngredientType.Vegetable)
        {
            Debug.Log("Only vegetables can be chopped.");
            return;
        }

        if (ingredient.IsPrepared)
        {
            Debug.Log("This vegetable is already prepared.");
            return;
        }

        currentIngredient = holder.RemoveIngredient();

        currentIngredient.transform.SetParent(ingredientPoint);
        currentIngredient.transform.SetLocalPositionAndRotation(
            Vector3.zero,
            Quaternion.identity
        );

        preparationCoroutine = StartCoroutine(PrepareIngredient());
    }

    private void TryPickupPreparedIngredient(IngredientHolder holder)
    {
        if (IsPreparing)
            return;

       if (!holder.TryHold(currentIngredient))
            return;

        currentIngredient = null;
    }

    private IEnumerator PrepareIngredient()
    {
        IsPreparing = true;

        totalPreparationTime =
            currentIngredient.Data.PreparationTime;

        RemainingPreparationTime = totalPreparationTime;

        while (RemainingPreparationTime > 0f)
        {
            RemainingPreparationTime -= Time.deltaTime;

            progressUI.Show(
                RemainingPreparationTime,
                totalPreparationTime
            );

            yield return null;
        }

        RemainingPreparationTime = 0f;

        currentIngredient.SetPrepared();

        IsPreparing = false;
        preparationCoroutine = null;

        progressUI.Hide();

        Debug.Log("Vegetable chopping complete.");
    }
    public void ResetTable()
    {
        if (preparationCoroutine != null)
        {
            StopCoroutine(preparationCoroutine);
            preparationCoroutine = null;
        }

        if (currentIngredient != null)
        {
            Destroy(currentIngredient.gameObject);
            currentIngredient = null;
        }

        RemainingPreparationTime = 0f;
        IsPreparing = false;
        totalPreparationTime = 0f;

        if (progressUI != null)
        {
            progressUI.Hide();
        }
    }
}