using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemSO itemData; // Assign the corresponding ItemSO in the Inspector
    public AudioClip pickupSound;
    public float soundVolume = 1.5f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    public void PlayPickupSound()
    {
        AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);

    }

}
