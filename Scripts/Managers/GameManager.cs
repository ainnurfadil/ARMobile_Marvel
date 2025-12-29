using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameManager");
                instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    
    [Header("Game State")]
    public bool quizCompleted = false;
    public int currentScore = 0;
    public int totalQuestions = 5;
    
    [Header("Settings")]
    public bool soundEnabled = true;
    public bool musicEnabled = true;
    public string currentLanguage = "id";
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadGameData();
    }
    
    public void LoadGameData()
    {
        quizCompleted = PlayerPrefs.GetInt("QuizCompleted", 0) == 1;
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        currentLanguage = PlayerPrefs.GetString("Language", "id");
    }
    
    public void SaveGameData()
    {
        PlayerPrefs.SetInt("QuizCompleted", quizCompleted ? 1 : 0);
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.SetInt("SoundEnabled", soundEnabled ? 1 : 0);
        PlayerPrefs.SetInt("MusicEnabled", musicEnabled ? 1 : 0);
        PlayerPrefs.SetString("Language", currentLanguage);
        PlayerPrefs.Save();
    }
    
    public void CompleteQuiz(int score)
    {
        quizCompleted = true;
        currentScore = score;
        SaveGameData();
    }
    
    public void ResetQuiz()
    {
        quizCompleted = false;
        currentScore = 0;
        PlayerPrefs.DeleteKey("QuizCompleted");
        PlayerPrefs.DeleteKey("CurrentScore");
        PlayerPrefs.Save();
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    public void ExitApp()
    {
        SaveGameData();
        Application.Quit();
    }
    
    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        SaveGameData();
    }
    
    public void ToggleMusic()
    {
        musicEnabled = !musicEnabled;
        SaveGameData();
    }
}