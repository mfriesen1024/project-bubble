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

    [Header("Gate Settings")]
    public GameObject gatePrefab; // Gate (barrier) prefab
    public List<Vector2Int> gatePositions; // Specific positions for gates
    public Vector3 gateScale = new Vector3(1f, 1f, 1f); // Target scale for the gates
    public float gateHeightOffset = 0.5f; // Height offset above the floor

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        SpawnBerriesInAreas();
        SpawnBeakers();
        SpawnGates();
    }

    private void SpawnBerriesInAreas()
    {
        if (berryPrefabs.Count != berryAreas.Count)
        {
            Debug.LogError("Mismatch between number of berry prefabs and berry areas! Ensure each area has one specific berry prefab.");
            return;
        }

        for (int i = 0; i < berryAreas.Count; i++)
        {
            BerryArea berryArea = berryAreas[i];
            GameObject berryPrefab = berryPrefabs[i];

            List<Vector2Int> availablePositions = GetAvailablePositionsInTileList(berryArea);

            if (availablePositions.Count == 0)
            {
                Debug.LogWarning($"No valid positions found in area {berryArea.name} for berry spawning!");
                continue;
            }

            Vector2Int spawnPosition = availablePositions[Random.Range(0, availablePositions.Count)];
            Vector3 berryPosition = gridManager.GetWorldPosition(spawnPosition);
            berryPosition.y = berryHeightOffset;

            GameObject spawnedBerry = Instantiate(berryPrefab, berryPosition, Quaternion.identity, gridManager.transform);
            spawnedBerry.transform.localScale = berryScale;

            Debug.Log($"Berry of type {berryPrefab.name} spawned in area {berryArea.name} at position {spawnPosition}");
        }
    }

    private void SpawnBeakers()
    {
        if (beakerPositions == null || beakerPositions.Count == 0)
        {
            Debug.LogWarning("No positions specified for beakers!");
            return;
        }

        foreach (Vector2Int spawnPosition in beakerPositions)
        {
            if (gridManager.wallPositions.Contains(spawnPosition))
            {
                Debug.LogWarning($"Beaker position {spawnPosition} overlaps with a wall. Skipping...");
                continue;
            }

            Vector3 beakerPosition = gridManager.GetWorldPosition(spawnPosition);
            beakerPosition.y = beakerHeightOffset;

            GameObject spawnedBeaker = Instantiate(beakerPrefab, beakerPosition, Quaternion.identity, gridManager.transform);
            spawnedBeaker.transform.localScale = beakerScale;

            Debug.Log($"Beaker spawned at position {spawnPosition}");
        }
    }

    private void SpawnGates()
    {
        if (gatePositions == null || gatePositions.Count == 0)
        {
            Debug.LogWarning("No positions specified for gates!");
            return;
        }

        foreach (Vector2Int spawnPosition in gatePositions)
        {
            if (gridManager.wallPositions.Contains(spawnPosition))
            {
                Debug.LogWarning($"Gate position {spawnPosition} overlaps with a wall. Skipping...");
                continue;
            }

            Vector3 gatePosition = gridManager.GetWorldPosition(spawnPosition);
            gatePosition.y = gateHeightOffset;

            GameObject spawnedGate = Instantiate(gatePrefab, gatePosition, Quaternion.identity, gridManager.transform);
            spawnedGate.transform.localScale = gateScale;

            Debug.Log($"Gate spawned at position {spawnPosition}");
        }
    }

    private List<Vector2Int> GetAvailablePositionsInTileList(BerryArea berryArea)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        foreach (Vector2Int position in berryArea.specificTiles)
        {
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
