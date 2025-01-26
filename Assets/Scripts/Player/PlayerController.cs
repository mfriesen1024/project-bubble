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
    private AudioClip[] walkSounds; // Array of walking sounds
    private AudioSource audioSource;

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
    }

    private bool CanMove(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        // Perform raycast
        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            Debug.Log("Blocked by: " + hit.collider.name);
            return false; // Blocked by an object within raycastDistance
        }

        return true; // No obstacles, movement allowed
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
