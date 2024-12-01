using System.Collections.Generic;
using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Array of room prefabs
    public int maxRooms = 10; // Maximum number of rooms to generate
    public LayerMask roomLayer; // Layer for collision checks
    private List<GameObject> placedRooms = new List<GameObject>(); // Tracks placed rooms
    private List<Transform> availableDoorways = new List<Transform>(); // Tracks open doorways

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // Place the initial room
        GameObject startRoom = Instantiate(roomPrefabs[0], Vector3.zero, Quaternion.identity);
        placedRooms.Add(startRoom);
        AddRoomDoorways(startRoom);

        // Add additional rooms
        for (int i = 0; i < maxRooms; i++)
        {
            if (!PlaceNextRoom())
            {
                Debug.LogWarning("Failed to place a room. Stopping generation.");
                break;
            }
        }
    }

    bool PlaceNextRoom()
    {
        // Shuffle the available doorways for randomness
        ShuffleList(availableDoorways);

        foreach (Transform doorway in availableDoorways)
        {
            // Try placing a random room at this doorway
            GameObject randomRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            GameObject newRoom = Instantiate(randomRoomPrefab);

            if (TryPlaceRoom(newRoom, doorway))
            {
                placedRooms.Add(newRoom);
                AddRoomDoorways(newRoom);
                return true;
            }

            Destroy(newRoom); // Destroy if placement fails
        }

        return false; // No valid doorway found
    }

    bool TryPlaceRoom(GameObject newRoom, Transform targetDoorway)
    {
        Transform[] newRoomDoorways = GetRoomDoorways(newRoom);

        foreach (Transform newDoorway in newRoomDoorways)
        {
            // Align the new room's doorway with the target doorway
            AlignDoorways(newRoom, newDoorway, targetDoorway);

            // Check for overlaps
            if (!IsOverlapping(newRoom))
            {
                availableDoorways.Remove(targetDoorway); // Mark target doorway as used
                availableDoorways.Remove(newDoorway); // Remove matched doorway
                return true;
            }
        }

        return false; // Failed to place the room
    }


    void AlignDoorways(GameObject newRoom, Transform newDoorway, Transform targetDoorway)
    {
        // Align positions
        Vector3 offset = targetDoorway.position - newDoorway.position;
        offset.y = 0; // Keep all rooms on the same level
        newRoom.transform.position += offset;

        // Align rotations
        Quaternion targetRotation = Quaternion.LookRotation(-targetDoorway.forward, Vector3.up);
        Quaternion newRotation = Quaternion.LookRotation(newDoorway.forward, Vector3.up);
        newRoom.transform.rotation = targetRotation * Quaternion.Inverse(newRotation);
    }

    bool IsOverlapping(GameObject newRoom)
    {
        Collider[] colliders = newRoom.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            if (Physics.CheckBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, roomLayer))
            {
                return true; // Overlapping with an existing room
            }
        }
        return false;
    }

    void AddRoomDoorways(GameObject room)
    {
        Transform[] doorways = GetRoomDoorways(room);
        foreach (Transform doorway in doorways)
        {
            availableDoorways.Add(doorway);
        }
    }

    Transform[] GetRoomDoorways(GameObject room)
    {
        Transform doorwaysParent = room.transform.Find("Doorways");
        if (doorwaysParent != null)
        {
            return doorwaysParent.GetComponentsInChildren<Transform>();
        }
        return new Transform[0];
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
