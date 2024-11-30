using UnityEngine;

public class CeilingHider : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public LayerMask ceilingLayer; // Layer mask for the ceilings
    public float raycastDistance = 100f; // Maximum distance of the raycast
    private GameObject lastHitCeiling; // To keep track of the last ceiling hit

    private void Update()
    {
        if (player == null) return;

        // Cast a ray from the camera to the player
        Ray ray = new Ray(transform.position, player.position - transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, ceilingLayer))
        {
            GameObject hitCeiling = hit.collider.gameObject;

            // Hide the ceiling if it blocks the view
            if (hitCeiling != lastHitCeiling)
            {
                if (lastHitCeiling != null)
                {
                    ShowCeiling(lastHitCeiling); // Restore the previous ceiling
                }

                HideCeiling(hitCeiling); // Hide the current ceiling
                lastHitCeiling = hitCeiling;
            }
        }
        else
        {
            // No ceiling is blocking, restore the last ceiling
            if (lastHitCeiling != null)
            {
                ShowCeiling(lastHitCeiling);
                lastHitCeiling = null;
            }
        }
    }

    private void HideCeiling(GameObject ceiling)
    {
        // Disable the ceiling or make it transparent
        Renderer renderer = ceiling.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false; // Hides the ceiling visually
        }
    }

    private void ShowCeiling(GameObject ceiling)
    {
        // Re-enable the ceiling
        Renderer renderer = ceiling.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = true;
        }
    }
}
