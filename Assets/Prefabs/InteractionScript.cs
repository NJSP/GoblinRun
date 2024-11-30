using Assets.Prefabs;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{
    public Collider playerCollider;
    private IInteractable currentInteractable;

    void Start()
    {
        // Initialize any necessary components or variables
    }

    void Update()
    {
        // If the player presses the E key and there is a current interactable object
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the other collider has the IInteractable component
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Clear the current interactable when the player exits the collider
        if (other.GetComponent<IInteractable>() != null)
        {
            currentInteractable = null;
        }
    }
}