using UnityEngine;

public class Trash : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        IngredientHolder holder = player.GetIngredientHolder();

        if (holder == null || !holder.HasIngredient)
            return;

        Ingredient ingredient = holder.RemoveIngredient();

        if (ingredient != null)
        {
            Destroy(ingredient.gameObject);
        }
    }
}