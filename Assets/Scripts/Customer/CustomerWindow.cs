using System;
using UnityEngine;

public class CustomerWindow : MonoBehaviour, IInteractable
{
    private Order currentOrder;

    public bool HasOrder => currentOrder != null;
    public Order CurrentOrder => currentOrder;

    public event Action<CustomerWindow, int> OrderCompleted;
    public event Action<Order> OrderUpdated;

    [SerializeField] private CustomerWindowUI orderUI;

    private void OnEnable()
    {
        OrderUpdated += HandleOrderUpdated;
    }

    private void OnDisable()
    {
        OrderUpdated -= HandleOrderUpdated;
    }

    private void HandleOrderUpdated(Order order)
    {
        orderUI.DisplayOrder(order);
    }
    public void SetOrder(Order order)
    {
        currentOrder = order;
        OrderUpdated?.Invoke(currentOrder);
    }

    public void Interact(PlayerController player)
    {
        if (currentOrder == null)
            return;

        IngredientHolder holder = player.GetIngredientHolder();

        if (holder == null || !holder.HasIngredient)
            return;

        Ingredient ingredient = holder.HeldIngredient;

        if (!currentOrder.TryFulfill(ingredient))
        {
            Debug.Log("Ingredient is not required for this order.");
            return;
        }

        // Ingredient was successfully delivered.
        holder.RemoveIngredient();
        Destroy(ingredient.gameObject);

        if (currentOrder.IsComplete)
        {
            CompleteOrder();
        }
        else
        {
            // The order changed, so notify the UI.
            OrderUpdated?.Invoke(currentOrder);
        }
    }

    private void CompleteOrder()
    {
        int score = currentOrder.CalculateScore(Time.time);

        orderUI.ShowScorePopup(score);

        currentOrder = null;

        OrderUpdated?.Invoke(null);
        OrderCompleted?.Invoke(this, score);
    }
    public void ClearOrder()
    {
        currentOrder = null;
        OrderUpdated?.Invoke(null);
    }
}