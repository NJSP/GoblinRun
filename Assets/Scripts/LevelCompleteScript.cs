using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteController : MonoBehaviour
{
    public TextMeshProUGUI totalLootValueText; // Assign in the Inspector
    public TextMeshProUGUI levelTimeText; // Assign in the Inspector
    public Transform itemsListParent; // Assign in the Inspector
    public GameObject itemEntryPrefab; // Assign in the Inspector
    public GameObject ThanksForPlayingScreen; // Assign in the Inspector

    void Start()
    {
        // Display total loot value
        totalLootValueText.text = $"Total Loot Value: {GameData.TotalLootValue}";

        // Display level time
        int minutes = Mathf.FloorToInt(GameData.LevelTime / 60f);
        int seconds = Mathf.FloorToInt(GameData.LevelTime % 60f);
        levelTimeText.text = $"Time: {minutes:00}:{seconds:00}";

        // Group items by name and count
        Dictionary<string, (int count, int value)> itemCounts = new Dictionary<string, (int count, int value)>();
        foreach (ItemSO item in GameData.CollectedItems)
        {
            if (itemCounts.ContainsKey(item.itemName))
            {
                itemCounts[item.itemName] = (itemCounts[item.itemName].count + 1, itemCounts[item.itemName].value + item.value);
            }
            else
            {
                itemCounts[item.itemName] = (1, item.value);
            }
        }

        // Display collected items
        foreach (var item in itemCounts)
        {
            GameObject itemEntry = Instantiate(itemEntryPrefab, itemsListParent);
            itemEntry.GetComponentInChildren<TextMeshProUGUI>().text = $"- {item.Key} x{item.Value.count} : {item.Value.value}";
        }

        
    }
    public void ToggleThanksForPlaying()
    {
        ThanksForPlayingScreen.SetActive(!ThanksForPlayingScreen.activeSelf);
    }

    public void ReturnToMainMenu()
    {
        
        // Load the MainMenuScene
        SceneManager.LoadScene("MainMenu");
    }
}