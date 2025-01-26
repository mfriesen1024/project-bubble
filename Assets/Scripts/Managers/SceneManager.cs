using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Called when the Play button is pressed
    public void PlayGame()
    {
        // Load the game scene (replace "GameScene" with your scene name)
        SceneManager.LoadScene("MainLevel");
    }

    // Called when the Options button is pressed
    public void OpenOptions()
    {
        // Load the options scene (replace "OptionsScene" with your scene name)
        SceneManager.LoadScene("OptionsScene");
    }

    // Called when the Quit button is pressed
    public void QuitGame()
    {
        // Exit the application
        Debug.Log("Quit Game"); // This is just for testing in the editor
        Application.Quit();
    }
}
