using UnityEngine;

public class Stove : MonoBehaviour, IInteractable
{
    [SerializeField] private StoveSlot[] slots;

    public void Interact(PlayerController player)
    {
        IngredientHolder holder = player.GetIngredientHolder();

        if (holder == null)
            return;

        if (holder.HasIngredient)
        {
            TryPlaceIngredient(holder);
            return;
        }

        TryTakeIngredient(holder);
    }

    private void TryPlaceIngredient(IngredientHolder holder)
    {
        foreach (StoveSlot slot in slots)
        {
            if (slot.TryPlaceIngredient(holder))
                return;
        }

        Debug.Log("No available stove slot.");
    }

    private void TryTakeIngredient(IngredientHolder holder)
    {
        foreach (StoveSlot slot in slots)
        {
            if (slot.TryTakeIngredient(holder))
                return;
        }

        Debug.Log("No cooked meat available.");
    }
    public void ResetStove()
    {
        foreach (StoveSlot slot in slots)
        {
            if (slot != null)
            {
                slot.ResetSlot();
            }
        }
    }
}