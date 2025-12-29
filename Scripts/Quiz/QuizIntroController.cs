using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizIntroController : MonoBehaviour
{
    [Header("UI References")]
    public Button startButton;
    public Transform mascot;
    
    [Header("Floating Icons")]
    public Transform[] floatingIcons;
    
    [Header("Animation Settings")]
    public float floatSpeed = 1f;
    public float floatAmount = 20f;
    
    private Vector3[] iconStartPositions;
    
    void Start()
    {
        // Setup button
        if (startButton != null)
            startButton.onClick.AddListener(OnStartQuiz);
        
        // Store initial positions for floating animation
        if (floatingIcons.Length > 0)
        {
            iconStartPositions = new Vector3[floatingIcons.Length];
            for (int i = 0; i < floatingIcons.Length; i++)
            {
                if (floatingIcons[i] != null)
                    iconStartPositions[i] = floatingIcons[i].localPosition;
            }
        }
    }
    
    void Update()
    {
        // Animate floating icons
        AnimateFloatingIcons();
    }
    
    void AnimateFloatingIcons()
    {
        for (int i = 0; i < floatingIcons.Length; i++)
        {
            if (floatingIcons[i] != null)
            {
                float offset = Mathf.Sin(Time.time * floatSpeed + i) * floatAmount;
                Vector3 newPos = iconStartPositions[i];
                newPos.y += offset;
                floatingIcons[i].localPosition = newPos;
                
                // Gentle rotation
                floatingIcons[i].Rotate(0, 0, Mathf.Sin(Time.time + i) * 0.5f);
            }
        }
    }
    
    void OnStartQuiz()
    {
        // Play sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();
        
        // Load first quiz question
        // This could be same scene with different panel, or new scene
        SceneManager.LoadScene("QuizQuestion"); // Or show quiz panel
    }
}