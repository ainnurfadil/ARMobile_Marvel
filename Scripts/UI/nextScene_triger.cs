using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScene_triger : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("WelcomeScreen",LoadSceneMode.Single);
    }

}
