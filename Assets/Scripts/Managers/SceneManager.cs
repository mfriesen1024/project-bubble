using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas;   // Reference to Canvas_MM
    public GameObject optionsMenuCanvas; // Reference to Canvas_Options
    public GameObject volumeOnButton;  // Reference to VolumeOn GameObject
    public GameObject volumeOffButton; // Reference to VolumeOff GameObject

    private void Start()
    {
        // Ensure the main menu is active and options menu is hidden initially
        mainMenuCanvas.SetActive(true);
        optionsMenuCanvas.SetActive(false);

        // Ensure the correct volume button is active
        UpdateVolumeButtons();
    }

    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("LevelMain");
    }

    public void OpenOptions()
    {
        // Show the options menu and hide the main menu
        mainMenuCanvas.SetActive(false);
        optionsMenuCanvas.SetActive(true);
    }

    public void BackToMainMenu()
    {
        // Show the main menu and hide the options menu
        mainMenuCanvas.SetActive(true);
        optionsMenuCanvas.SetActive(false);
    }

    public void QuitGame()
    {
        // Quit the game
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void ToggleVolume()
    {
        // Toggle the volume between on and off
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1; // Enable volume
        }
        else
        {
            AudioListener.volume = 0; // Mute volume
        }

        // Update the visibility of the volume buttons
        UpdateVolumeButtons();
    }

    private void UpdateVolumeButtons()
    {
        // Show/hide the correct volume button based on the current volume
        if (AudioListener.volume == 0)
        {
            volumeOnButton.SetActive(false);
            volumeOffButton.SetActive(true);
        }
        else
        {
            volumeOnButton.SetActive(true);
            volumeOffButton.SetActive(false);
        }
    }
}
