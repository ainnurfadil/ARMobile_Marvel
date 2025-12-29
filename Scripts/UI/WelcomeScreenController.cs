using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WelcomeScreenController : MonoBehaviour
{
    public float displayTime = 2f;
    
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }
    
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(displayTime);
        
        if (PlayerPrefs.GetInt("QuizCompleted", 0) == 1)
            SceneManager.LoadScene("MainMenu");
        else
            SceneManager.LoadScene("QuizIntro");
    }
}