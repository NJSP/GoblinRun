using Assets.Inventory;
using System.Collections.Generic;
using UnityEngine;

internal class InventoryComponent : MonoBehaviour
{
    private List<ItemSO> inventory;
    private List<ItemPickup> overlappingItems = new List<ItemPickup>();
    public ObjectPooler itemPool; // Reference to the ObjectPooler

    void Start()
    {
        inventory = new List<ItemSO>();
        itemPool = ObjectPooler.Instance;
    }

    public void AddItem(string name, int value)
    {
        ItemSO newItem = ScriptableObject.CreateInstance<ItemSO>();
        newItem.itemName = name;
        newItem.value = value;
        inventory.Add(newItem);
    }

    void OnTriggerEnter(Collider other)
    {
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null && !overlappingItems.Contains(item))
        {
            overlappingItems.Add(item);
            Debug.Log($"Added to OverlappingItemsList: {item.itemData.itemName}");
        }
    }

    void OnTriggerExit(Collider other)
    {
        ItemPickup item = other.GetComponent<ItemPickup>();
        if (item != null && overlappingItems.Contains(item))
        {
            overlappingItems.Remove(item);
            Debug.Log($"Removed from OverlappingItemsList: {item.itemData.itemName}");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropLowestValueItem();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            int totalValue = 0;
            foreach (ItemSO item in inventory)
            {
                totalValue += item.value;
            }
            Debug.Log($"Total value of items: {totalValue}");
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            foreach (ItemPickup item in overlappingItems)
            {
                Debug.Log($"Overlapping Item: {item.itemData.itemName}");
            }
        }
    }

    public void DropLowestValueItem()
    {
        if (inventory.Count > 0)
        {
            ItemSO lowestValueItem = GetLowestValueItem();
            string itemTag = lowestValueItem.itemName; // Assuming itemName is used as the tag
            GameObject item = itemPool.SpawnFromPool(itemTag, transform.position + Vector3.up, Quaternion.identity);
            if (item != null)
            {
                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(Vector3.up * 5.0f, ForceMode.Impulse); // Add slight upward force
                    rb.AddTorque(Random.insideUnitSphere * 5.0f, ForceMode.Impulse); // Add random torque for spinning
                }

                // Set the item data on the dropped GameObject
                ItemPickup itemPickup = item.GetComponent<ItemPickup>();
                if (itemPickup != null)
                {
                    itemPickup.itemData = lowestValueItem;
                }
            }

            inventory.Remove(lowestValueItem);
            Debug.Log($"Dropped item: {lowestValueItem.itemName} with value {lowestValueItem.value}");
        }
        else
        {
            Debug.Log("No items to drop.");
        }
    }

    private ItemSO GetLowestValueItem()
    {
        ItemSO lowestValueItem = null;
        foreach (var item in inventory)
        {
            if (lowestValueItem == null || item.value < lowestValueItem.value)
            {
                lowestValueItem = item;
            }
        }
        return lowestValueItem;
    }

    private void PickupItem()
    {
        if (overlappingItems.Count > 0)
        {
            ItemPickup itemToPick = overlappingItems[0];
            inventory.Add(itemToPick.itemData);
            Debug.Log($"Picked up: {itemToPick.itemData.itemName}, Value: {itemToPick.itemData.value}");

            string itemTag = itemToPick.itemData.itemName; // Assuming itemName is used as the tag
            itemPool.ReturnToPool(itemTag, itemToPick.gameObject);
            overlappingItems.Remove(itemToPick);
            Debug.Log($"Removed from OverlappingItemsList: {itemToPick.itemData.itemName}");
        }
        else
        {
            Debug.Log("No items to pick up.");
        }
    }
}