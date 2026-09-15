using UnityEngine;

public class Refrigerator : MonoBehaviour, IInteractable
{
    [SerializeField] private IngredientData[] availableIngredients;
    [SerializeField] private Ingredient ingredientPrefab;
    [SerializeField] private Transform ingredientSpawnPoint;

   public void Interact(PlayerController player)
{
    IngredientHolder holder = player.GetIngredientHolder();

    if (holder.HasIngredient)
    {
        Debug.Log("Player is already holding an ingredient.");
        return;
    }

    IngredientData data = GetRandomIngredientData();

    if (data == null)
        return;

    Ingredient ingredient = Instantiate(
        ingredientPrefab,
        ingredientSpawnPoint.position,
        ingredientSpawnPoint.rotation
    );

    ingredient.Initialize(data);

    if (!holder.TryHold(ingredient))
    {
        Destroy(ingredient.gameObject);
        return;
    }

    Debug.Log($"Picked up {data.Type}");
}

    private IngredientData GetRandomIngredientData()
    {
        if (availableIngredients == null || availableIngredients.Length == 0)
        {
            Debug.LogError("Refrigerator has no ingredient data configured.", this);
            return null;
        }

        return availableIngredients[
            Random.Range(0, availableIngredients.Length)
        ];
    }
}