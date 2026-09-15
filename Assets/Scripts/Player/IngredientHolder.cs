using UnityEngine;

public class IngredientHolder : MonoBehaviour
{
    [SerializeField] private Transform handPoint;

    public Ingredient HeldIngredient { get; private set; }

    public bool HasIngredient => HeldIngredient != null;

    public bool TryHold(Ingredient ingredient)
    {
        if (HasIngredient || ingredient == null)
            return false;

        HeldIngredient = ingredient;

        Transform ingredientTransform = ingredient.transform;

        ingredientTransform.SetParent(handPoint);
        ingredientTransform.SetLocalPositionAndRotation(
            Vector3.zero,
            Quaternion.identity
        );

        return true;
    }

    public Ingredient RemoveIngredient()
    {
        Ingredient ingredient = HeldIngredient;

        if (ingredient != null)
        {
            ingredient.transform.SetParent(null);
        }

        HeldIngredient = null;

        return ingredient;
    }
    public void ResetHolder()
    {
        if (HeldIngredient != null)
        {
            Destroy(HeldIngredient.gameObject);
            HeldIngredient = null;
        }
    }
}