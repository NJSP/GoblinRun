using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsScreen;
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

    public void ToggleCredits()
    {
        if (creditsScreen.activeSelf)
        {
            creditsScreen.SetActive(false);
        }
        else
        {
            creditsScreen.SetActive(true);
        }

    }
}
