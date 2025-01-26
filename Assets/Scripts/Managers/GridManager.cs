using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public GameObject tilePrefab;   // Tile prefab
    public GameObject wallPrefab;  // Wall prefab
    public int gridWidth = 10;     // Grid width
    public int gridHeight = 10;    // Grid height
    public float tileSize = 1f;    // Size of each tile
    public float wallSize = 1.5f;  // Size of each wall

    [Header("Wall Settings")]
    public List<Vector2Int> wallPositions = new List<Vector2Int>
    {
        new Vector2Int(1, 1),
        new Vector2Int(3, 4),
        new Vector2Int(5, 2),
        new Vector2Int(7, 7)
    };

    [Header("Grid Transform")]
    public float distanceFromCenter = 10f; // Distance to position the grid from the origin
    public Vector3 gridRotation = Vector3.zero; // Rotation of the grid in degrees

    private void Start()
    {
        PositionGridAtCenter();
        GenerateGrid();
    }

    private void PositionGridAtCenter()
    {
        // Calculate the center position of the grid
        float gridWidthWorld = gridWidth * tileSize;
        float gridHeightWorld = gridHeight * tileSize;

        // Calculate the position relative to the origin
        Vector3 spawnPosition = new Vector3(-gridWidthWorld / 2f, 0f, -gridHeightWorld / 2f);

        // Apply the position and rotation to the GridManager
        transform.position = spawnPosition;
        transform.rotation = Quaternion.Euler(gridRotation);

        Debug.Log($"GridManager positioned at: {spawnPosition} with rotation: {gridRotation}");
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector2Int gridPosition = new Vector2Int(x, z);

                // Calculate tile position in local space
                Vector3 tilePosition = new Vector3(x * tileSize, 0, z * tileSize);

                // Transform to world space with the grid's rotation
                Vector3 worldTilePosition = transform.TransformPoint(tilePosition);

                // Instantiate the tile
                GameObject tile = Instantiate(tilePrefab, worldTilePosition, Quaternion.identity, transform);

                // Add a box collider to the tile if not present
                if (tile.GetComponent<BoxCollider>() == null)
                {
                    tile.AddComponent<BoxCollider>();
                }

                // Check if this tile is a wall
                if (wallPositions.Contains(gridPosition))
                {
                    SpawnWall(gridPosition);
                }

                // Initialize the tile (if it has a script)
                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript != null)
                {
                    tileScript.wallPrefab = wallPrefab;
                    tileScript.Initialize(gridPosition, wallPositions.Contains(gridPosition));
                }
            }
        }
    }

    private void SpawnWall(Vector2Int gridPosition)
    {
        // Calculate the wall position
        Vector3 wallPosition = new Vector3(
            gridPosition.x * tileSize,
            wallPrefab.GetComponent<Renderer>().bounds.size.y / 2, // Ensure the wall sits on the floor
            gridPosition.y * tileSize
        );

        // Transform to world space with the grid's rotation
        Vector3 worldWallPosition = transform.TransformPoint(wallPosition);

        // Instantiate the wall
        GameObject wall = Instantiate(wallPrefab, worldWallPosition, Quaternion.identity, transform);

        // Adjust wall scale to match tile size without gaps
        wall.transform.localScale = new Vector3(tileSize, wall.transform.localScale.y, tileSize);

        // Add a box collider to the wall if not present
        if (wall.GetComponent<BoxCollider>() == null)
        {
            wall.AddComponent<BoxCollider>();
        }

        // Set the wall to be static to improve performance
        wall.isStatic = true;

        Debug.Log($"Wall spawned at grid position {gridPosition}");
    }

    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        // Converts a grid position to world position
        Vector3 localPosition = new Vector3(
            gridPosition.x * tileSize,
            0,
            gridPosition.y * tileSize
        );
        return transform.TransformPoint(localPosition);
    }

    public List<Vector2Int> GetAllGridPositions()
    {
        // Returns all grid positions, excluding wall positions
        List<Vector2Int> allPositions = new List<Vector2Int>();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector2Int position = new Vector2Int(x, z);

                if (!wallPositions.Contains(position))
                {
                    allPositions.Add(position);
                }
            }
        }

        return allPositions;
    }
}
