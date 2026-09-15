using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Interaction")]
    [SerializeField] private float interactionRadius = 1.5f;

    [SerializeField] private IngredientHolder ingredientHolder;
    [SerializeField] private GameManager gameManager;
    private IInteractable currentInteractable;

    private void Update()
    {
        if (gameManager.CurrentState != GameState.Playing)
            return;

        HandleMovement();
        DetectInteractable();
        HandleInteraction();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    private void DetectInteractable()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            interactionRadius
        );

        currentInteractable = null;

        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            IInteractable interactable =
                collider.GetComponent<IInteractable>();

            if (interactable == null)
                continue;

            float distance =
                (collider.transform.position - transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                currentInteractable = interactable;
            }
        }
    }

    private void HandleInteraction()
    {
        if (currentInteractable == null)
          return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentInteractable.Interact(this);
        }
    }

    public IngredientHolder GetIngredientHolder()
    {
        return ingredientHolder;
    }
}