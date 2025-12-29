using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial Panels")]
    public GameObject[] tutorialPanels;
    public GameObject skipButton;
    public GameObject nextButton;
    public GameObject previousButton;
    public GameObject finishButton;
    
    [Header("Progress")]
    public Image[] progressDots;
    public Color activeColor = Color.white;
    public Color inactiveColor = Color.gray;
    
    [Header("Settings")]
    public bool showTutorialOnFirstLaunch = true;
    
    private int currentStep = 0;
    private const string TUTORIAL_KEY = "ARTutorialCompleted";
    
    void Start()
    {
        if (showTutorialOnFirstLaunch && PlayerPrefs.GetInt(TUTORIAL_KEY, 0) == 0)
        {
            ShowTutorial();
        }
        else
        {
            HideTutorial();
        }
    }
    
    public void ShowTutorial()
    {
        currentStep = 0;
        UpdateTutorialDisplay();
    }
    
    public void HideTutorial()
    {
        foreach (GameObject panel in tutorialPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
    }
    
    public void NextStep()
    {
        AudioManager.Instance.PlayButtonClick();
        
        if (currentStep < tutorialPanels.Length - 1)
        {
            currentStep++;
            UpdateTutorialDisplay();
        }
    }
    
    public void PreviousStep()
    {
        AudioManager.Instance.PlayButtonClick();
        
        if (currentStep > 0)
        {
            currentStep--;
            UpdateTutorialDisplay();
        }
    }
    
    public void SkipTutorial()
    {
        AudioManager.Instance.PlayButtonClick();
        CompleteTutorial();
    }
    
    public void FinishTutorial()
    {
        AudioManager.Instance.PlayButtonClick();
        CompleteTutorial();
    }
    
    void CompleteTutorial()
    {
        PlayerPrefs.SetInt(TUTORIAL_KEY, 1);
        PlayerPrefs.Save();
        HideTutorial();
    }
    
    void UpdateTutorialDisplay()
    {
        // Hide all panels
        foreach (GameObject panel in tutorialPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
        
        // Show current panel
        if (currentStep < tutorialPanels.Length && tutorialPanels[currentStep] != null)
        {
            tutorialPanels[currentStep].SetActive(true);
        }
        
        // Update buttons
        if (previousButton != null)
            previousButton.SetActive(currentStep > 0);
        
        if (nextButton != null)
            nextButton.SetActive(currentStep < tutorialPanels.Length - 1);
        
        if (finishButton != null)
            finishButton.SetActive(currentStep == tutorialPanels.Length - 1);
        
        // Update progress dots
        UpdateProgressDots();
    }
    
    void UpdateProgressDots()
    {
        for (int i = 0; i < progressDots.Length; i++)
        {
            if (progressDots[i] != null)
            {
                progressDots[i].color = (i == currentStep) ? activeColor : inactiveColor;
            }
        }
    }
    
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey(TUTORIAL_KEY);
        PlayerPrefs.Save();
    }
}