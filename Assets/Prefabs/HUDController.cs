using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI livesText; // Assign in the Inspector
    public TextMeshProUGUI lootValueText;
    public TextMeshProUGUI lowestValueItemText;
    public Image atRiskItemIcon;
    public Image atRiskBadge;
    public TextMeshProUGUI timerText;
    public GameObject ExitMenu; // Assign in the Inspector
    public Button exitButton; // Assign in the Inspector
    public Button resumeButton; // Assign in the Inspector
    public GameObject deadMenu;
    public GameObject pauseMenu;

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

        // Check for player input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void UpdateHUD()
    {
        // Update lives
        livesText.text = $"Lives: {lives}";

        // Update total loot value
        if (inventory != null)
        {
            lootValue = inventory.GetTotalLootValue();
            lootValueText.text = $"Loot Total: {lootValue}";

            // Update lowest value item icon
            ItemSO lowestValueItem = inventory.GetLowestValueItem();
            if (lowestValueItem != null)
            {
                atRiskItemIcon.sprite = lowestValueItem.icon;
                atRiskItemIcon.enabled = true;
                atRiskBadge.enabled = true;
                //lowestValueItemText.text = $"{lowestValueItem.itemName}";
            }
            else
            {
                atRiskItemIcon.enabled = false;
                atRiskBadge.enabled = false;
                //lowestValueItemText.text = "";
            }
        }
    }

    public void UpdateTimer()
    {
        // Format the timer as MM:SS
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ToggleExitMenu()
    {
        if (ExitMenu.activeSelf)
        {
            ExitMenu.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        else
        {
            Time.timeScale = 0;
            ExitMenu.SetActive(true);
        }

    }

    public void ExitLevel()
    {
        // Store data in GameData
        if (inventory != null)
        {
            GameData.CollectedItems = inventory.inventory;
            GameData.TotalLootValue = lootValue;
        }
        GameData.LevelTime = timer;

        // Load the LevelCompleteScene
        SceneManager.LoadScene("LevelCompleteScene");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            return;
        }
        else
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
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
    //    {
    //        if (inventory != null)
    //        {
    //            inventory.AddItem("Loot", value); // Update inventory
    //            UpdateHUD();
    //        }
    //    }
}