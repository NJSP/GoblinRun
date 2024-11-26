using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI livesText; // Assign in the Inspector
    public TextMeshProUGUI lootValueText;
    public TextMeshProUGUI lowestValueItemText;
    public TextMeshProUGUI timerText;

    public int lives; // Example starting lives
    private int lootValue = 0; // Total value of loot
    private float timer = 0f; // Time elapsed in the level

    private InventoryComponent inventory; // Reference to your inventory system
    private GoblinController playerController; // Reference to the player controller
    private HealthComponent healthComponent; // Reference to the player's health component
    void Start()
    {
        // Get the player controller from the parent
        playerController = GetComponentInParent<GoblinController>();
        if (playerController != null)
        {
            inventory = playerController.GetComponent<InventoryComponent>();
            healthComponent = playerController.GetComponent<HealthComponent>();
        }

        if (inventory == null)
        {
            Debug.LogError("InventoryComponent not found on the player.");
        }
        else
        {
            inventory.OnInventoryChanged += UpdateHUD; // Subscribe to inventory changes
        }

        if (healthComponent == null)
        {
            Debug.LogError("HealthComponent not found on the player.");
        }
        else
        {
            lives = healthComponent.health; // Initialize lives from HealthComponent
            healthComponent.OnHealthChanged += UpdateLives; // Subscribe to health changes
        }
        UpdateHUD();
    }

    void UpdateLives(int newHealth)
    {
        lives = newHealth;
        UpdateHUD();
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;
        UpdateTimer();
    }

    public void UpdateHUD()
    {
        // Update lives
        livesText.text = $"Lives: {lives}";

        // Update total loot value
        if (inventory != null)
        {
            lootValue = inventory.GetTotalLootValue();
            lootValueText.text = $"Loot Value: {lootValue}";

            // Update lowest value item
            ItemSO lowestValueItem = inventory.GetLowestValueItem();
            lowestValueItemText.text = $"Lowest Value Item: {(lowestValueItem != null ? lowestValueItem.name : "None")}";
        }
    }

    public void UpdateTimer()
    {
        // Format the timer as MM:SS
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    //public void DecreaseLife()
    //{
    //    lives--;
    //    UpdateHUD();
    //    if (lives <= 0)
    //    {
    //        Debug.Log("Game Over!");
    //        // Handle game over logic
    //    }
    //}

    //public void AddLoot(int value)
    //{
    //    if (inventory != null)
    //    {
    //        inventory.AddItem("Loot", value); // Update inventory
    //        UpdateHUD();
    //    }
    //}
}