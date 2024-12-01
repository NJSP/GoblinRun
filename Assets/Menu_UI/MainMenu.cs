using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Replace "GameScene" with the name of your game scene
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game"); // Debug for editor testing
        Application.Quit(); // Works in builds only
    }

    public void ShowCredits()
    {
        Debug.Log("Credits Menu Clicked");
        // Implement credits logic or scene transition here
        // Example: SceneManager.LoadScene("CreditsScene");
    }
}
