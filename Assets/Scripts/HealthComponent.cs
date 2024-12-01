using UnityEditor;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 3;
    public int minHealth = 0;
    public delegate void HealthChanged(int newHealth);
    public event HealthChanged OnHealthChanged;

    private GameObject character;
    private PlayerRagdollController ragdollController;
    [SerializeField] public GameObject HUD;

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
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        Debug.Log("Current health: " + health);
        OnHealthChanged?.Invoke(health); // Notify listeners of health change
    }

    public void Die()
    {
        Debug.Log("Player has died");
        ragdollController.EnableRagdoll();

        // activate the DeadMenu in the HUD
        HUD.GetComponent<HUDController>().deadMenu.SetActive(true);
    }
}
