using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemSO itemData; // Reference to your Scriptable Object
    public GameObject prefab; // Reference to the prefab to spawn

    private void Start()
    {
        if (itemData != null)
        {
            // Instantiate the prefab at the spawner's position
            GameObject spawnedItem = Instantiate(prefab, transform.position, Quaternion.identity);

            // Customize the spawned item based on the Scriptable Object
            spawnedItem.name = itemData.itemName;

            // Example: Change visual elements based on the SO
            Renderer renderer = spawnedItem.GetComponent<Renderer>();
            if (renderer != null && itemData.icon != null)
            {
                renderer.material.mainTexture = itemData.icon.texture;
            }

            Debug.Log($"Spawned item: {itemData.itemName}");
        }
        else
        {
            Debug.LogWarning("Item data or prefab is missing.");
        }
    }
}
