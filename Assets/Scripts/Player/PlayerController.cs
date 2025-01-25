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

    void Start()
    {
        if (cameraTransform != null)
        {
            cameraOffset = cameraTransform.position - transform.position;
        }

        currentHealth = maxHealth;
    }

    void Update()
    {
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

        if (cameraTransform != null)
        {
            cameraTransform.position = transform.position + cameraOffset;
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
