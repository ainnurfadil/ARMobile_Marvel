using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References - Animations")]
    [Tooltip("Drag the '3DCube' button here")]
    public RectTransform cubeRect;
    [Tooltip("Drag the 'MenuButtons' parent object here")]
    public RectTransform menuButtonsContainer;
    [Tooltip("Drag the 'TopLogoBar' object here")]
    public RectTransform topLogoBar;
    [Tooltip("Drag the 'Button_Exit' object here")]
    public RectTransform exitButtonRect;

    [Header("UI References - Popup")]
    [Tooltip("Drag the 'Panel_Exit_Popup' object here")]
    public GameObject exitPopupPanel;

    [Header("Scene Names")]
    [Tooltip("Name of the scene for the 3D Cube/AR feature")]
    public string arSceneName = "AR_Scene";
    [Tooltip("Name of the scene for Soal Scaffolding")]
    public string scaffoldingSceneName = "Scaffolding_Scene";
    [Tooltip("Name of the scene for E-Book")]
    public string ebookSceneName = "Ebook_Scene";
    [Tooltip("Name of the scene for Tentang Kami")]
    public string aboutSceneName = "About_Scene";

    [Header("Animation Settings")]
    public float floatSpeed = 2f;
    public float floatAmplitude = 15f;
    public float transitionDuration = 0.5f;

    private Vector2 _startPosCube;
    private bool _isNavigating = false; // Prevents double clicking

    private void Start()
    {
        // 1. Store original position for floating math
        if (cubeRect != null)
            _startPosCube = cubeRect.anchoredPosition;

        // 2. Ensure Popup is hidden at start
        if (exitPopupPanel != null)
            exitPopupPanel.SetActive(false);
    }

    private void Update()
    {
        // FLOATING ANIMATION LOGIC
        // Only float if we aren't currently leaving the scene
        if (cubeRect != null && !_isNavigating)
        {
            float newY = _startPosCube.y + (Mathf.Sin(Time.time * floatSpeed) * floatAmplitude);
            cubeRect.anchoredPosition = new Vector2(_startPosCube.x, newY);
        }
    }

    #region Button Events (Link these in Inspector)

    public void On3DCubeClicked()
    {
        StartCoroutine(TransitionAndLoad(arSceneName));
    }

    public void OnScaffoldingClicked()
    {
        StartCoroutine(TransitionAndLoad(scaffoldingSceneName));
    }

    public void OnEbookClicked()
    {
        StartCoroutine(TransitionAndLoad(ebookSceneName));
    }

    public void OnAboutClicked()
    {
        StartCoroutine(TransitionAndLoad(aboutSceneName));
    }

    public void OnExitButtonClicked()
    {
        // Show the popup
        if (exitPopupPanel != null)
        {
            exitPopupPanel.SetActive(true);
            // Optional: Add a simple pop-in animation for the popup here
            exitPopupPanel.transform.localScale = Vector3.zero;
            StartCoroutine(AnimatePopupOpen());
        }
    }

    #endregion

    #region Popup Events

    public void OnConfirmExitClicked()
    {
        Debug.Log("Exiting Application...");
        Application.Quit();
    }

    public void OnCancelExitClicked()
    {
        // Hide the popup
        if (exitPopupPanel != null)
            exitPopupPanel.SetActive(false);
    }

    #endregion

    #region Transition Logic

    // This Coroutine handles the "Motion" before changing scenes
    private IEnumerator TransitionAndLoad(string sceneName)
    {
        if (_isNavigating) yield break; // Stop if already loading
        _isNavigating = true;

        float timer = 0f;
        
        // Store starting positions/scales
        Vector2 startCubePos = cubeRect.anchoredPosition;
        Vector3 startMenuScale = menuButtonsContainer.localScale;
        Vector2 startExitPos = exitButtonRect.anchoredPosition;
        Vector2 startLogoPos = topLogoBar.anchoredPosition;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = timer / transitionDuration;
            
            // Smooth step for nicer motion
            t = t * t * (3f - 2f * t); 

            // 1. Move Cube Up and Fade (Simulated by scaling)
            if (cubeRect != null)
            {
                cubeRect.anchoredPosition = Vector2.Lerp(startCubePos, startCubePos + new Vector2(0, 500), t);
                cubeRect.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            }

            // 2. Move Menu Buttons Down
            if (menuButtonsContainer != null)
            {
                menuButtonsContainer.localScale = Vector3.Lerp(startMenuScale, Vector3.zero, t);
            }

            // 3. Move Exit Button Down
            if (exitButtonRect != null)
            {
                exitButtonRect.anchoredPosition = Vector2.Lerp(startExitPos, startExitPos - new Vector2(0, 200), t);
            }

            // 4. Move Logo Bar Up
            if (topLogoBar != null)
            {
                topLogoBar.anchoredPosition = Vector2.Lerp(startLogoPos, startLogoPos + new Vector2(0, 200), t);
            }

            yield return null;
        }

        // Wait a tiny moment to ensure animation finishes visually
        yield return new WaitForSeconds(0.1f);

        // Load the Scene
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator AnimatePopupOpen()
    {
        float timer = 0f;
        float duration = 0.3f;
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            // Elastic pop effect
            exitPopupPanel.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
        exitPopupPanel.transform.localScale = Vector3.one;
    }

    #endregion
}