using Assets.Prefabs;
using UnityEngine;

public class LeverScript : MonoBehaviour, IInteractable
{
    public bool isOn = false;
    public GameObject door;
    public float rotationSpeed = 2.0f;
    private Quaternion targetRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRotation = door.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        door.transform.rotation = Quaternion.Lerp(door.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Interact()
    {
        if (isOn)
        {
            // smoothly rotate the door back to its original position
            targetRotation = Quaternion.Euler(door.transform.eulerAngles.x, door.transform.eulerAngles.y + 90, door.transform.eulerAngles.z);
            isOn = false;
        }
        else
        {
            // smoothly rotate the door to the open position
            targetRotation = Quaternion.Euler(door.transform.eulerAngles.x, door.transform.eulerAngles.y - 90, door.transform.eulerAngles.z);
            isOn = true;
        }
    }
}
