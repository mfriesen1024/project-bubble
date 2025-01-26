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

    [SerializeField]
    private float raycastDistance = 0.25f; // Raycast distance adjustable in Inspector

    [SerializeField]
    private string wallTag = "Wall"; // Tag for wall objects adjustable in Inspector

    [SerializeField]
    private string floorTag = "Floor"; // Tag for floor objects adjustable in Inspector

    [SerializeField] 
    private string beakerTag = "Beaker"; // Tag for Beaker object adjustable in Inspector


    [SerializeField]
    private AudioClip[] walkSounds; // Array of walking sounds
    private AudioSource audioSource;

    [SerializeField]
    private Animator animator; // Animator for player animations

    void Start()
    {
        if (cameraTransform != null)
        {
            cameraOffset = cameraTransform.position - transform.position;
        }

        currentHealth = maxHealth;

        // Get or add the AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false; // Don't play audio on start
        audioSource.loop = false; // Ensure the clip does not loop

        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("Animator not assigned and not found on the GameObject.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isMoving && CanMove(Vector3.forward))
        {
            StartCoroutine(MovePlayer(Vector3.forward));
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }

        if (Input.GetKeyDown(KeyCode.A) && !isMoving && CanMove(Vector3.left))
        {
            StartCoroutine(MovePlayer(Vector3.left));
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.S) && !isMoving && CanMove(Vector3.back))
        {
            StartCoroutine(MovePlayer(Vector3.back));
            transform.rotation = Quaternion.LookRotation(Vector3.back);
        }

        if (Input.GetKeyDown(KeyCode.D) && !isMoving && CanMove(Vector3.right))
        {
            StartCoroutine(MovePlayer(Vector3.right));
            transform.rotation = Quaternion.LookRotation(Vector3.right);
        }

        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
        }

        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
        }

        CheckFloorBelow();
    }

    private bool CanMove(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        // Perform raycast
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag(wallTag))
            {
                Debug.Log("Blocked by wall: " + hit.collider.name);
                return false; // Blocked by an object with the specified wall tag
            }
            if (hit.collider.CompareTag(beakerTag))
            {
                Debug.Log("Blocked by beaker: " + hit.collider.name);
                return false; // Blocked by an object with the specified beaker tag
            }

        }

        return true; // No obstacles, movement allowed
    }

    private void CheckFloorBelow()
    {
        Ray downRay = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(downRay, out hit, raycastDistance))
        {
            if (!hit.collider.CompareTag(floorTag))
            {
                Debug.LogWarning("Player is not standing on a valid floor!");
                // Optional: Reset the player's position to the last valid position if needed
                transform.position += Vector3.up * 0.1f; // Slightly move the player up to avoid sinking
            }
        }
        else
        {
            Debug.LogWarning("No floor detected below the player!");
            // Optional: Handle falling logic here if needed
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        // Play a random walking sound
        PlayRandomWalkSound();

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private void PlayRandomWalkSound()
    {
        if (walkSounds != null && walkSounds.Length > 0 && audioSource != null)
        {
            // Select a random clip from the array
            AudioClip randomClip = walkSounds[Random.Range(0, walkSounds.Length)];
            audioSource.clip = randomClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No walking sounds assigned or AudioSource is not set!");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }
}