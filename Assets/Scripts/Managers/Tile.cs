using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public bool IsWall { get; private set; } = false; // Flag to indicate if the tile is a wall

    public GameObject wallPrefab; // Assign a wall prefab if needed

    public void Initialize(Vector2Int position, bool isWall = false)
    {
        GridPosition = position;
        IsWall = isWall;

        if (IsWall && wallPrefab != null)
        {
            // Spawn a wall on this tile
            Instantiate(wallPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
        }
    }
}
