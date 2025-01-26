using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using TMPro; // For TextMeshPro

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // Timer text (TextMeshPro)
    [SerializeField] private GameObject gameOverPanel; // Game Over panel
    [SerializeField] private AudioSource backgroundMusic; // Reference to the existing audio source
    [SerializeField] private AudioSource gameOverMusic; // Reference to the Game Over audio source
    private float timer = 60f; // Start with 60 seconds
    private bool isGameOver = false;

    public static bool IsGameOver { get; private set; } = false; // Static flag to disable input globally

    private void Start()
    {
        UpdateTimerUI(); // Initialize the timer text
        gameOverPanel.SetActive(false); // Ensure Game Over panel is hidden

        // Ensure the game-over music is not playing initially
        if (gameOverMusic != null)
            gameOverMusic.Stop();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            timer -= Time.deltaTime; // Reduce the timer
            UpdateTimerUI();

            if (timer <= 0)
            {
                TriggerGameOver();
            }
        }
    }

    private void UpdateTimerUI()
    {
        // Update the TextMeshPro text with remaining time (no extra text)
        timerText.text = Mathf.Ceil(Mathf.Max(timer, 0)).ToString();
    }

    private void TriggerGameOver()
    {
        isGameOver = true; // Stop the timer
        IsGameOver = true; // Set the global flag to true
        gameOverPanel.SetActive(true); // Show Game Over panel
        timerText.text = "0"; // Set the timer to 0 explicitly if not already

        // Mute the background music
        if (backgroundMusic != null)
            backgroundMusic.Stop();

        // Play the game-over music
        if (gameOverMusic != null)
            gameOverMusic.Play();
    }

    // Button Function: Restart the current scene
    public void TryAgain()
    {
        IsGameOver = false; // Reset the global flag
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    // Button Function: Load the main menu scene
    public void BackToMenu()
    {
        IsGameOver = false; // Reset the global flag
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the actual menu scene name
    }

    // Button Function: Quit the game
    public void QuitGame()
    {
        Application.Quit(); // Close the application
        Debug.Log("Game Quit"); // This won't appear in a built game, only in the editor
    }
}
