using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public void Interact(PlayerController player)
    {
        Debug.Log($"Interacted with {gameObject.name}");
    }
}