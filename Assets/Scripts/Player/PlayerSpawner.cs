using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class PlayerSpawner : MonoBehaviour
{
    private GridManager gridManager;

    [Header("Player Settings")]
    public GameObject playerPrefab; // Player prefab
    public Vector3 playerScale = new Vector3(1f, 1f, 1f); // Scale for the player
    public float playerHeightOffset = 0.5f; // Height offset for the player spawn above the floor

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // Iterate through row 0 to find a valid tile
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            Vector2Int tilePosition = new Vector2Int(x, 0); // Only row 0 tiles
            Debug.Log($"Checking position: {tilePosition}");

            // Check if the tile does not have a wall
            if (!gridManager.wallPositions.Contains(tilePosition))
            {
                Debug.Log($"Valid spawn position found: {tilePosition}");

                // Calculate the world position for the player
                Vector3 playerPosition = new Vector3(
                    tilePosition.x * gridManager.tileSize,
                    playerHeightOffset,
                    tilePosition.y * gridManager.tileSize
                );

                // Instantiate the player
                GameObject player = Instantiate(playerPrefab, playerPosition, Quaternion.identity, gridManager.transform);

                // Fix the player's scale
                player.transform.localScale = playerScale;

                return; // Exit once a valid position is found
            }
        }

        Debug.LogWarning("No valid spawn position for the player found on row 0!");
    }
}
