using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Items;

[RequireComponent(typeof(GridManager))]
public class BerryAndBeakerSpawner : MonoBehaviour
{
    private GridManager gridManager;

    [Header("Berry Settings")]
    public List<GameObject> berryPrefabs;
    public List<BerryArea> berryAreas;
    public Vector3 berryScale = new Vector3(1f, 1f, 1f);
    public float berryHeightOffset = 0.5f;

    [Header("Beaker Settings")]
    public GameObject beakerPrefab;
    public List<Vector2Int> beakerPositions;
    public Vector3 beakerScale = new Vector3(1f, 1f, 1f);
    public float beakerHeightOffset = 0.5f;

    [Header("Gate Settings")]
    public GameObject gatePrefab;
    public List<Vector2Int> gatePositions;
    public Vector3 gateScale = new Vector3(1f, 1f, 1f);
    public float gateHeightOffset = 0.5f;

    [Header("Finish Point Settings")]
    public GameObject finishPointPrefab;
    public Vector2Int finishPointPosition;
    public Vector3 finishPointScale = new Vector3(1f, 1f, 1f);
    public float finishPointHeightOffset = 0.5f;
    public GameObject gameWonPanel;

    [Header("Audio Settings")]
    public AudioSource existingAudioSource;
    public AudioSource newAudioSource;

    [Header("Input Settings")]
    public bool disableWASDInput = false;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    private void Start()
    {
        SpawnBerriesInAreas();
        SpawnBeakers();
        SpawnGates();
        SpawnFinishPoint();
    }

    private void Update()
    {
        HandleInput(); // Handle keyboard input dynamically
    }

    private void HandleInput()
    {
        if (disableWASDInput)
        {
            // Disable WASD input by consuming the key events
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                Debug.Log("WASD input is disabled.");
            }
        }
    }

    public void SwitchAudioSources()
    {
        if (existingAudioSource != null)
        {
            existingAudioSource.Stop();
            existingAudioSource.enabled = false;
        }

        if (newAudioSource != null)
        {
            newAudioSource.enabled = true;
            newAudioSource.Play();
            Debug.Log("Audio source switched.");
        }
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

        for (int i = 0; i < gatePositions.Count; i++)
        {
            Vector2Int spawnPosition = gatePositions[i];
            if (gridManager.wallPositions.Contains(spawnPosition))
            {
                Debug.LogWarning($"Gate position {spawnPosition} overlaps with a wall. Skipping...");
                continue;
            }

            Vector3 gatePosition = gridManager.GetWorldPosition(spawnPosition);
            gatePosition.y = gateHeightOffset;

            GameObject spawnedGate = Instantiate(gatePrefab, gatePosition, Quaternion.identity, gridManager.transform);
            spawnedGate.transform.localScale = gateScale;

            // Apparently doors are spawned by area index, but the ingerdients are spawned in inverse order? This fixes that.
            PotionType requiredPotion = (PotionType)(2 - i);
            if(spawnedGate.TryGetComponent(out Door doorComponent)) { doorComponent.RequiredPotion = requiredPotion; }

            Debug.Log($"Gate spawned at position {spawnPosition}");
        }
    }

    private void SpawnFinishPoint()
    {
        if (finishPointPrefab == null || gameWonPanel == null)
        {
            Debug.LogError("Finish point prefab or game won panel is not assigned!");
            return;
        }

        Vector3 finishPosition = gridManager.GetWorldPosition(finishPointPosition);
        finishPosition.y = finishPointHeightOffset;

        GameObject spawnedFinishPoint = Instantiate(finishPointPrefab, finishPosition, Quaternion.identity, gridManager.transform);
        spawnedFinishPoint.transform.localScale = finishPointScale;

        FinishPoint finishPointScript = spawnedFinishPoint.AddComponent<FinishPoint>();
        finishPointScript.gameWonPanel = gameWonPanel;

        Debug.Log($"Finish point spawned at position {finishPointPosition}");
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
    public string name;
    public List<Vector2Int> specificTiles;
}

public class FinishPoint : MonoBehaviour
{
    public GameObject gameWonPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has the tag "Player"
        {
            gameWonPanel.SetActive(true);
            Debug.Log("Game won! Finish point reached.");
        }
    }
}
