using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Inventory
{
    internal class InventoryComponent : MonoBehaviour
    {
        private SortedSet<ItemSO> inventory;
        private List<ItemPickup> overlappingItems = new List<ItemPickup>();

        void Start()
        {
            inventory = new SortedSet<ItemSO>(new ItemComparer());
        }

        public void AddItem(string name, int value)
        {
            ItemSO newItem = new ItemSO { itemName = name, value = value };
            inventory.Add(newItem);
        }

        void OnTriggerEnter(Collider other)
        {
            // Check if the other object has an ItemPickup component
            ItemPickup item = other.GetComponent<ItemPickup>();
            if (item != null)
            {
                overlappingItems.Add(item); // Add the item to the overlapping list
            }
        }

        void OnTriggerExit(Collider other)
        {
            // Remove the item from the overlapping list when it leaves the collider
            ItemPickup item = other.GetComponent<ItemPickup>();
            if (item != null)
            {
                overlappingItems.Remove(item);
            }
        }

        void Update()
        {
            // Check for the 'E' key press
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickupItem();
            }

            // Check for the 'Q' key press
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropLowestValueItem();
            }

            if (inventory.Min == null)
            {
                inventory.Remove(inventory.Min);

            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                foreach (ItemSO item in inventory)
                {
                    Debug.Log($"Item: {item.itemName}, Value: {item.value}");
                }
            }
        }
        public void DropLowestValueItem()
        {
            if (inventory.Count > 0)
            {
                ItemSO lowestValueItem = inventory.Min;

                // Assuming the prefab is stored in the ItemSO
                GameObject itemPrefab = lowestValueItem.prefab;
                if (itemPrefab != null)
                {
                    // Spawn the item on top of the player
                    Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);
                }

                inventory.Remove(lowestValueItem);
                Debug.Log($"Dropped item: {lowestValueItem.itemName} with value {lowestValueItem.value}");
            }
            else
            {
                Debug.Log("No items to drop.");
            }

            RemoveNullItems();
        }

        private void PickupItem()
        {
            if (overlappingItems.Count > 0 && overlappingItems[0] != null)
            {
                // Pick the first item in the overlapping list
                ItemPickup itemToPick = overlappingItems[0];
                inventory.Add(itemToPick.itemData); // Add item to inventory
                Debug.Log($"Picked up: {itemToPick.itemData.itemName}, Value: {itemToPick.itemData.value}");

                // Destroy or deactivate the item GameObject
                Destroy(itemToPick.gameObject);

                // Remove it from the overlapping list
                overlappingItems.Remove(itemToPick);
            }
            else
            {
                Debug.Log("No items to pick up.");
            }
            RemoveNullItems();

        }

        public void RemoveNullItems()
        {
            // Remove all null items
            inventory.RemoveWhere(item => item == null);
            Debug.Log("Removed null placeholders from the list.");
        }
    }
    
}
