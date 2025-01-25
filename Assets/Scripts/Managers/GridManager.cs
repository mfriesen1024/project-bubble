using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;   // Tile prefab
    public GameObject wallPrefab;  // Wall prefab
    public int gridWidth = 10;     // Grid width
    public int gridHeight = 10;    // Grid height
    public float tileSize = 1f;    // Size of each tile
    public float wallSize = 1.5f; // Size of each tile

    // Define specific tiles as walls (hardcoded or dynamically loaded)
    public List<Vector2Int> wallPositions = new List<Vector2Int>
    {
        new Vector2Int(1, 1),
        new Vector2Int(3, 4),
        new Vector2Int(5, 2),
        new Vector2Int(7, 7)
    };

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                // Calculate tile position in world space
                Vector3 tilePosition = new Vector3(x * tileSize, 0, z * tileSize);

                // Instantiate the tile
                GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, transform);

                // Check if this tile is a wall
                bool isWall = wallPositions.Contains(new Vector2Int(x, z));

                // Initialize the tile
                Tile tileScript = tile.GetComponent<Tile>();
                if (tileScript != null)
                {
                    tileScript.wallPrefab = wallPrefab; // Assign the wall prefab
                    tileScript.Initialize(new Vector2Int(x, z), isWall);
                }
            }
        }
    }
}
