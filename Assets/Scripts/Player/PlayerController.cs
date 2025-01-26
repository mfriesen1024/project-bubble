using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = 0.2f;

    public Transform cameraTransform;
    private Vector3 cameraOffset;

    public int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private GameObject gameOverPanel; // Reference to the Game Over panel
    [SerializeField] private Animator animator; // Animator for player animations

    private bool isGameOver = false; // Local game-over state to prevent further logic after death

    private void Start()
    {
        currentHealth = maxHealth;

        if (cameraTransform != null)
        {
            cameraOffset = cameraTransform.position - transform.position;
        }

        // Ensure the Game Over panel is hidden initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Game Over Panel is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        // If the game is already over, stop processing input
        if (isGameOver || Timer.IsGameOver)
        {
            return;
        }

        // Handle movement input
        if (Input.GetKeyDown(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.forward));
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.A) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.left));
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.back));
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }

        if (Input.GetKeyDown(KeyCode.D) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.right));
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        }

        // Update camera position to follow the player
        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isGameOver || Timer.IsGameOver) return; // Prevent damage if the game is over

        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isGameOver = true; // Mark game-over state locally
        Debug.Log("Player has died!");

        // Disable player movement
        this.enabled = false;

        // Play the death animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Show the Game Over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Game Over Panel enabled.");
        }
        else
        {
            Debug.LogWarning("Game Over Panel is not assigned in the Inspector!");
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }
}
