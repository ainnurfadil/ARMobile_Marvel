using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Penting untuk LayoutRebuilder
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement; // Penting untuk pindah Scene

public class Quiz : MonoBehaviour
{
    [Header("Quiz Segment")]
    public TextAsset questionJson;
    public GameObject questionPanel;
    public TMP_Text descriptionText;
    public Image questionImage; 
    public Transform sentenceContainer; // Container untuk kalimat dengan {blank}
    public GameObject textPrefab; // Prefab UI Text biasa
    public GameObject slotPrefab; // Prefab UI Slot kosong (Button/Image)
    public SmartGridSystem optionsGrid;
    public Button checkButton; 
    public TMP_Text checkButtonText;

    [Header("Compliment Reaction Segment")]
    public TextAsset complimentJson;
    public GameObject feedbackCorrectPanel;
    public TMP_Text correctMessageText;

    [Header("Explanation Segment")]
    public TextAsset explanationJson;
    public GameObject feedExplanationPanel;

    [Header("Scene Management")]
    public string completionSceneName = "QuizCompletionScene";

    void Start()
    {
        // Inisialisasi quiz di sini
        LoadData();
        ShowQuestion(0);

    }

    void LoadData()
    {
        if (questionJson != null)
            qList = JsonUtility.FromJson<QuizAppQuestionList>(questionJson.text);
        
        if (complimentJson != null)
            wList = JsonUtility.FromJson<QuizAppWrongFeedbackList>(complimentJson.text); 
        
        if (explanationJson != null)
            cList = JsonUtility.FromJson<QuizAppCorrectFeedbackList>(explanationJson.text);
    }

    void ShowQuestion(int index)
    {
        currentQuestionIndex = index;
        
    }
    
    public void GoToCompletionScene()
    {
        SceneManager.LoadScene(completionSceneName);
    }
}