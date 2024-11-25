using UnityEngine;

public class PlayerRagdollController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;

    private void Start()
    {
        // Get all Rigidbody and Collider components in child bones
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        // Disable ragdoll at the start
        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        // Disable animator to prevent animation overriding physics
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Enable physics for all Rigidbody and Collider components
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
    }

    public void DisableRagdoll()
    {
        // Enable animator to regain control of the character
        if (animator != null)
        {
            animator.enabled = true;
        }

        // Disable physics for all Rigidbody and Collider components
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }
    }
}
