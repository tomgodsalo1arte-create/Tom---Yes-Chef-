using UnityEngine;

public class Ingredient : MonoBehaviour
{

    [SerializeField] private Renderer ingredientRenderer;
    [SerializeField] private Material rawMaterial;
    [SerializeField] private Material preparedMaterial;
    public IngredientData Data { get; private set; }

    public IngredientType Type => Data.Type;

    public IngredientState State { get; private set; }

    public bool IsPrepared => State == IngredientState.Prepared;

    public void Initialize(IngredientData data)
    {
        if (data == null)
        {
            Debug.LogError("Ingredient.Initialize called with null data.", this);
            return;
        }

        Data = data;

        State = data.Type == IngredientType.Cheese
            ? IngredientState.Prepared
            : IngredientState.Raw;

        UpdateVisualState();
    }
    public void SetPrepared()
    {
        State = IngredientState.Prepared;
        UpdateVisualState();
    }
    private void UpdateVisualState()
    {
        if (ingredientRenderer == null)
            return;

        ingredientRenderer.sharedMaterial =
            IsPrepared ? preparedMaterial : rawMaterial;
    }
}