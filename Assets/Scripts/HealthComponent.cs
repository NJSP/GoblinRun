using UnityEditor;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 3;
    public int minHealth = 0;

    private GameObject character;
    private PlayerRagdollController ragdollController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        character = gameObject; // Reference to the GameObject this component is attached to
        ragdollController = character.GetComponentInChildren<PlayerRagdollController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health <= minHealth)
        {
            health = minHealth;
            Die();
        }
        Debug.Log("Current health: " + health);
    }

    public void Die()
    {
        Debug.Log("Player has died");
        ragdollController.EnableRagdoll();
    }
}
