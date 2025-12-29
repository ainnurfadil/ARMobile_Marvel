using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class ARMenuController : MonoBehaviour
{
    [Header("Scene Names Configuration")]
    [Tooltip("Type the exact name of the scene for Balok Berongga here")]
    [SerializeField] private string balokBeronggaSceneName = "Scene_BalokBerongga";

    [Tooltip("Type the exact name of the scene for Balok Kearifan Lokal here")]
    [SerializeField] private string balokLokalSceneName = "Scene_BalokLokal";

    [Tooltip("Type the exact name of the Main Menu scene here")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // Function for the Top Button (Balok Berongga)
    public void OnClick_BalokBerongga()
    {
        // Check if scene name is not empty to prevent errors
        if (!string.IsNullOrEmpty(balokBeronggaSceneName))
        {
            SceneManager.LoadScene(balokBeronggaSceneName);
        }
        else
        {
            Debug.LogError("Scene name for Balok Berongga is empty in the Inspector!");
        }
    }

    // Function for the Middle Button (Balok Kearifan Lokal)
    public void OnClick_BalokLokal()
    {
        if (!string.IsNullOrEmpty(balokLokalSceneName))
        {
            SceneManager.LoadScene(balokLokalSceneName);
        }
        else
        {
            Debug.LogError("Scene name for Balok Kearifan Lokal is empty in the Inspector!");
        }
    }

    // Function for the Bottom Button (Kembali)
    public void OnClick_Kembali()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("Scene name for Main Menu is empty in the Inspector!");
        }
    }
}