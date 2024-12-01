using Assets.Prefabs;
using UnityEngine;

public class ExitDoorScript : MonoBehaviour, IInteractable
{
    // Get a reference to the player GameObject
    public GameObject Player;
    private HUDController HUD; // Reference to the HUDController

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HUD = Player.GetComponentInChildren<HUDController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        if (HUD != null)
        {
            HUD.ToggleExitMenu();
        }
    }
}