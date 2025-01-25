using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GridManager))]
public class BerrySpawner : MonoBehaviour
{
    private GridManager gridManager;

    [Header("Berry Settings")]
    public List<GameObject> berryPrefabs; // List of berry prefabs
    public int numberOfBerries = 10; // Total number of berries to spawn
    public Vector3 berryScale = new Vector3(1f, 1f, 1f); // Target scale for spawned berries
    public float berryHeightOffset = 0.5f; // Height offset above the floor

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        SpawnBerries();
    }

    private void SpawnBerries()
    {
        // Collect all possible tile positions excluding wall positions
        List<Vector2Int> availablePositions = new List<Vector2Int>();
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int z = 0; z < gridManager.gridHeight; z++)
            {
                Vector2Int position = new Vector2Int(x, z);
                // Exclude tiles with walls
                if (!gridManager.wallPositions.Contains(position))
                {
                    availablePositions.Add(position);
                }
            }
        }

        // Shuffle the available positions for randomness
        for (int i = 0; i < availablePositions.Count; i++)
        {
            int randomIndex = Random.Range(i, availablePositions.Count);
            Vector2Int temp = availablePositions[i];
            availablePositions[i] = availablePositions[randomIndex];
            availablePositions[randomIndex] = temp;
        }

        // Spawn berries at random positions
        for (int i = 0; i < Mathf.Min(numberOfBerries, availablePositions.Count); i++)
        {
            Vector2Int spawnPosition = availablePositions[i];
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
        }
    }
}
