using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GridManager))]
public class BerryAndBeakerSpawner : MonoBehaviour
{
    private GridManager gridManager;

    [Header("Berry Settings")]
    public List<GameObject> berryPrefabs; // List of berry prefabs
    public List<BerryArea> berryAreas; // List of areas where berries can spawn
    public Vector3 berryScale = new Vector3(1f, 1f, 1f); // Target scale for spawned berries
    public float berryHeightOffset = 0.5f; // Height offset above the floor

    [Header("Beaker Settings")]
    public GameObject beakerPrefab; // Beaker prefab
    public List<Vector2Int> beakerPositions; // Manually specify positions for beakers
    public Vector3 beakerScale = new Vector3(1f, 1f, 1f); // Target scale for the beaker
    public float beakerHeightOffset = 0.5f; // Height offset above the floor

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        SpawnBerriesInAreas();
        SpawnBeakers();
    }

    private void SpawnBerriesInAreas()
    {
        foreach (BerryArea berryArea in berryAreas)
        {
            // Collect all valid tile positions within the area
            List<Vector2Int> availablePositions = GetAvailablePositionsInTileList(berryArea);

            // Skip if no valid positions are found
            if (availablePositions.Count == 0)
            {
                Debug.LogWarning($"No valid positions found in area {berryArea.name} for berry spawning!");
                continue;
            }

            // Select a random position from the available positions
            Vector2Int spawnPosition = availablePositions[Random.Range(0, availablePositions.Count)];
            Vector3 berryPosition = new Vector3(
                spawnPosition.x * gridManager.tileSize,
                berryHeightOffset, // Place berry above the floor
                spawnPosition.y * gridManager.tileSize
            );

            // Select a random berry prefab
            GameObject randomBerryPrefab = berryPrefabs[Random.Range(0, berryPrefabs.Count)];

            // Instantiate the berry
            GameObject spawnedBerry = Instantiate(randomBerryPrefab, berryPosition, Quaternion.identity, gridManager.transform);

            // Fix the scale of the spawned berry
            spawnedBerry.transform.localScale = berryScale;

            Debug.Log($"Berry spawned in area {berryArea.name} at position {spawnPosition}");
        }
    }

    private void SpawnBeakers()
    {
        // Ensure there are positions specified for beakers
        if (beakerPositions == null || beakerPositions.Count == 0)
        {
            Debug.LogWarning("No positions specified for beakers!");
            return;
        }

        // Spawn beakers at the specified positions
        foreach (Vector2Int spawnPosition in beakerPositions)
        {
            if (gridManager.wallPositions.Contains(spawnPosition))
            {
                Debug.LogWarning($"Beaker position {spawnPosition} overlaps with a wall. Skipping...");
                continue;
            }

            Vector3 beakerPosition = new Vector3(
                spawnPosition.x * gridManager.tileSize,
                beakerHeightOffset, // Place beaker above the floor
                spawnPosition.y * gridManager.tileSize
            );

            // Instantiate the beaker
            GameObject spawnedBeaker = Instantiate(beakerPrefab, beakerPosition, Quaternion.identity, gridManager.transform);

            // Fix the scale of the spawned beaker
            spawnedBeaker.transform.localScale = beakerScale;

            Debug.Log($"Beaker spawned at position {spawnPosition}");
        }
    }

    private List<Vector2Int> GetAvailablePositionsInTileList(BerryArea berryArea)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        // Loop through the specific tiles
        foreach (Vector2Int position in berryArea.specificTiles)
        {
            // Exclude tiles with walls
            if (!gridManager.wallPositions.Contains(position))
            {
                availablePositions.Add(position);
            }
        }

        return availablePositions;
    }
}

[System.Serializable]
public class BerryArea
{
    public string name; // Name of the area (optional)
    public List<Vector2Int> specificTiles; // Specific tile positions for berry spawning
}
