using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AboutPageManager : MonoBehaviour
{
    [Header("Navigation Settings")]
    [Tooltip("The exact name of your Main Menu scene")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Auto Scroll Settings")]
    [SerializeField] private RectTransform contentToScroll; // Assign the object holding the Panels
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float resetPositionX = -1000f; // The X position where content snaps back
    [SerializeField] private float startPositionX = 0f;    // The starting X position

    private void Update()
    {
        // 1. Move the content to the left automatically
        if (contentToScroll != null)
        {
            contentToScroll.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

            // 2. Check if we went too far left (Looping logic)
            if (contentToScroll.anchoredPosition.x <= resetPositionX)
            {
                // Snap back to start position to create the "Loop" effect
                Vector2 newPos = contentToScroll.anchoredPosition;
                newPos.x = startPositionX;
                contentToScroll.anchoredPosition = newPos;
            }
        }
    }

    // 3. Function for the "KEMBALI" button
    public void BackToMenu()
    {
        Debug.Log("Loading Main Menu...");
        SceneManager.LoadScene(mainMenuSceneName);
    }
}