using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using TMPro; // For TextMeshPro

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // Timer text (TextMeshPro)
    [SerializeField] private GameObject gameOverPanel; // Game Over panel
    [SerializeField] private GameObject pauseMenuPanel; // Pause menu panel
    [SerializeField] private AudioSource backgroundMusic; // Reference to the existing audio source
    [SerializeField] private AudioSource gameOverMusic; // Reference to the Game Over audio source

    private float timer = 60f; // Start with 60 seconds
    private bool isGameOver = false;
    private bool isPaused = false; // Pause state flag

    public static bool IsGameOver { get; private set; } = false; // Static flag to disable input globally

    private void Start()
    {
        UpdateTimerUI(); // Initialize the timer text
        gameOverPanel.SetActive(false); // Ensure Game Over panel is hidden
        pauseMenuPanel.SetActive(false); // Ensure Pause menu is hidden

        // Ensure the game-over music is not playing initially
        if (gameOverMusic != null)
            gameOverMusic.Stop();
    }

    private void Update()
    {
        // Check for pause input using both P and Escape keys
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !isGameOver)
        {
            TogglePause();
        }

        // Reduce timer if not paused or game over
        if (!isGameOver && !isPaused)
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

    private void TogglePause()
    {
        isPaused = !isPaused; // Toggle pause state
        pauseMenuPanel.SetActive(isPaused); // Show or hide the pause menu

        if (isPaused)
        {
            Time.timeScale = 0; // Pause the game (freeze time)
            if (backgroundMusic != null)
                backgroundMusic.Pause(); // Pause background music
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            if (backgroundMusic != null)
                backgroundMusic.Play(); // Resume background music
        }
    }

    // Button Function: Resume game
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1; // Resume time
        if (backgroundMusic != null)
            backgroundMusic.Play(); // Resume music
    }

    // Button Function: Restart the current scene
    public void TryAgain()
    {
        Time.timeScale = 1; // Reset time scale
        IsGameOver = false; // Reset the global flag
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    // Button Function: Load the main menu scene
    public void BackToMenu()
    {
        Time.timeScale = 1; // Reset time scale
        IsGameOver = false; // Reset the global flag

        // Stop the current audio source
        if (backgroundMusic != null)
            backgroundMusic.Stop();

        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the actual menu scene name
    }

    // Button Function: Quit the game
    public void QuitGame()
    {
        Application.Quit(); // Close the application
        Debug.Log("Game Quit"); // This won't appear in a built game, only in the editor
    }
}
