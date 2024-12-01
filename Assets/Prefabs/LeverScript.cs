using Assets.Prefabs;
using UnityEngine;

public class LeverScript : MonoBehaviour, IInteractable
{
    public bool isOn = false;
    public GameObject door;
    public float rotationSpeed = 2.0f;
    private Quaternion targetRotation;
    public float doorSlideDistance = 2.0f;
    public bool SlidingDoor = false;

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
        if (SlidingDoor)
        {
            if (isOn)
            {
                door.transform.position += door.transform.up * doorSlideDistance;
            }
            else
            {
                door.transform.position -= door.transform.up * doorSlideDistance;
            }
        }
        else
        {
            if (isOn)
            {
                targetRotation *= Quaternion.Euler(0, 0, 90);
            }
            else
            {
                targetRotation *= Quaternion.Euler(0, 0, -90);
            }
            isOn = !isOn;
        }
    }
}
