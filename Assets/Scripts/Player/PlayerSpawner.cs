using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GridManager))]
public class PlayerSpawner : MonoBehaviour
{
    private GridManager gridManager;

    [Header("Player Settings")]
    public GameObject playerPrefab; // Player prefab
    public Vector3 playerScale = new Vector3(1f, 1f, 1f); // Scale for the player
    public float playerHeightOffset = 0.5f; // Height offset for the player spawn above the floor

    [Header("Spawn Area Settings")]
    public List<Vector2Int> spawnAreas; // Manually defined spawn areas

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        SpawnPlayerInSpecificAreas();
    }

    private void SpawnPlayerInSpecificAreas()
    {
        // Log all wall positions for debugging
        Debug.Log("Wall positions: " + string.Join(", ", gridManager.wallPositions));

        // Iterate through manually specified spawn areas
        foreach (Vector2Int tilePosition in spawnAreas)
        {
            Debug.Log($"Checking position: {tilePosition}");

            // Check if the tile does not have a wall
            if (!gridManager.wallPositions.Contains(tilePosition))
            {
                Debug.Log($"Valid spawn position found: {tilePosition}");

                // Calculate the world position for the player using grid rotation
                Vector3 playerPosition = gridManager.GetWorldPosition(tilePosition);
                playerPosition.y = playerHeightOffset; // Adjust height above the floor

                // Instantiate the player
                GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity, gridManager.transform);

                // Fix the player's scale
                player.transform.localScale = playerScale;

                Debug.Log($"Player spawned at position: {tilePosition}");
                return; // Exit once a valid position is found
            }
        }

        Debug.LogWarning("No valid spawn position for the player found in specified areas!");
    }
}
