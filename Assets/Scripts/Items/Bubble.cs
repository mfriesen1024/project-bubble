using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float fallSpeed = 2f; // Fall speed of the bubble
    public int damage = 10; // Damage the bubble does to the player
    public string floorTag = "Floor"; // Tag for floor objects
    public GameObject player; // Reference to the player object
    public BubbleSpawner spawner; // Reference to the spawner
    public float scaleIncreasePerSecond = 0.1f; // Rate of scale increase

    private Rigidbody rb; // Rigidbody component

    private void Awake()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on the Bubble prefab!");
        }
    }

    private void FixedUpdate()
    {
        // Move the bubble downward using Rigidbody
        if (rb != null)
        {
            rb.MovePosition(transform.position + Vector3.down * fallSpeed * Time.fixedDeltaTime);
        }

        // Gradually increase the scale of the bubble
        transform.localScale += Vector3.one * scaleIncreasePerSecond * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(floorTag))
        {
            spawner.PlayDestroySound(); // Play destroy sound
            Destroy(gameObject);
            Debug.Log("Bubble destroyed on the floor.");
        }
        else if (other.CompareTag("Player"))
        {
            spawner.ReducePlayerHealth(damage);
            spawner.PlayDestroySound(); // Play destroy sound
            Destroy(gameObject);
            Debug.Log("Bubble damaged the player.");
        }
    }
}
