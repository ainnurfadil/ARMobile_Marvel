using UnityEngine;
using UnityEngine.SceneManagement;

public class QuizNavigation : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Type the exact name of your Main Menu scene here")]
    public string mainMenuSceneName = "MainMenu"; 

    // This function will be called by the Button
    public void LoadMainMenu()
    {
        // Optional: Play a click sound here if you have an AudioManager
        // AudioManager.Instance.PlayClick();
        
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}