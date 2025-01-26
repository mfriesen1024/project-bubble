using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(GridManager))]
public class BubbleSpawner : MonoBehaviour
{
    private GridManager gridManager;

    [Header("Bubble Settings")]
    public GameObject bubblePrefab;
    public float bubbleSpawnInterval = 2f; // Time between bubble spawns
    public Vector3 bubbleScale = new Vector3(1f, 1f, 1f);
    public float bubbleFallSpeed = 2f; // Speed at which bubbles fall
    public int bubbleDamage = 10; // Damage to the player
    public float bubbleHeightOffset = 5f; // Height from which bubbles spawn
    public float scaleIncreasePerSecond = 0.1f; // Rate of bubble scale increase

    [Header("Spawn Audio Settings")]
    [SerializeField] private AudioClip[] spawnSounds; // Array of sounds for bubble spawning
    [Range(0f, 1f)][SerializeField] private float spawnSoundVolume = 1f; // Volume for spawn sounds

    [Header("Destroy Audio Settings")]
    [SerializeField] private AudioClip[] destroySounds; // Array of sounds for bubble destruction
    [Range(0f, 1f)][SerializeField] private float destroySoundVolume = 1f; // Volume for destroy sounds

    [Header("Player Settings")]
    public GameObject player;
    public int playerHealth = 100;

    private List<Vector2Int> occupiedPositions = new List<Vector2Int>(); // Tracks occupied positions

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        // Start spawning bubbles at regular intervals
        InvokeRepeating(nameof(SpawnBubble), 0f, bubbleSpawnInterval);
    }

    public void RegisterOccupiedPositions(List<Vector2Int> positions)
    {
        // Allow external scripts to register occupied positions (e.g., walls, ingredients, beakers)
        foreach (var position in positions)
        {
            if (!occupiedPositions.Contains(position))
            {
                occupiedPositions.Add(position);
            }
        }
    }

    private void SpawnBubble()
    {
        if (bubblePrefab == null)
        {
            Debug.LogError("Bubble prefab is not assigned!");
            return;
        }

        // Get all grid positions and filter out occupied positions
        List<Vector2Int> availablePositions = gridManager.GetAllGridPositions();
        availablePositions.RemoveAll(pos => occupiedPositions.Contains(pos));

        if (availablePositions.Count == 0)
        {
            Debug.LogWarning("No valid positions available for spawning bubbles!");
            return;
        }

        // Select a random position from the available positions
        Vector2Int randomPosition = availablePositions[Random.Range(0, availablePositions.Count)];
        Vector3 spawnPosition = gridManager.GetWorldPosition(randomPosition);
        spawnPosition.y += bubbleHeightOffset;

        // Instantiate the bubble
        GameObject spawnedBubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
        spawnedBubble.transform.localScale = bubbleScale;

        // Add the Bubble script to control its behavior
        Bubble bubbleScript = spawnedBubble.AddComponent<Bubble>();
        bubbleScript.fallSpeed = bubbleFallSpeed;
        bubbleScript.damage = bubbleDamage;
        bubbleScript.floorTag = "Floor";
        bubbleScript.player = player;
        bubbleScript.spawner = this;
        bubbleScript.scaleIncreasePerSecond = scaleIncreasePerSecond;

        // Play a random spawn sound
        PlayRandomSound(spawnSounds, spawnSoundVolume);

        Debug.Log($"Bubble spawned at position {randomPosition}");
    }

    public void PlayDestroySound()
    {
        // Play a random destroy sound
        PlayRandomSound(destroySounds, destroySoundVolume);
    }

    private void PlayRandomSound(AudioClip[] clips, float volume)
    {
        if (clips != null && clips.Length > 0)
        {
            AudioClip randomClip = clips[Random.Range(0, clips.Length)];
            AudioSource.PlayClipAtPoint(randomClip, transform.position, volume);
        }
    }

    public void ReducePlayerHealth(int damage)
    {
        playerHealth -= damage;
        Debug.Log($"Player health reduced by {damage}. Current health: {playerHealth}");

        if (playerHealth <= 0)
        {
            Debug.Log("Player has been defeated!");
            // Handle player defeat (e.g., restart level, show game over screen)
        }
    }
}
