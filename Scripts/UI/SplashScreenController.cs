using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    public float displayTime = 60f;
    
    void Start()
    {
        StartCoroutine(LoadNextScene());
    }
    
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(displayTime);
        
        SceneManager.LoadScene("WelcomeScreen");
    }
}