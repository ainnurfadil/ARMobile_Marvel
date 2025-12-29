using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject confirmationPanel;
    public GameObject loadingPanel;
    
    [Header("Confirmation Dialog")]
    public TextMeshProUGUI confirmationText;
    public Button confirmYesButton;
    public Button confirmNoButton;
    
    [Header("Loading")]
    public TextMeshProUGUI loadingText;
    public Image loadingBar;
    
    [Header("Toast")]
    public GameObject toastPanel;
    public TextMeshProUGUI toastText;
    public float toastDuration = 2f;
    
    private System.Action onConfirmYes;
    private System.Action onConfirmNo;
    
    void Start()
    {
        HideAllPanels();
        
        if (confirmYesButton != null)
            confirmYesButton.onClick.AddListener(OnConfirmYes);
        
        if (confirmNoButton != null)
            confirmNoButton.onClick.AddListener(OnConfirmNo);
    }
    
    void HideAllPanels()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
        
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
        
        if (toastPanel != null)
            toastPanel.SetActive(false);
    }
    
    public void ShowConfirmation(string message, System.Action onYes, System.Action onNo = null)
    {
        if (confirmationPanel != null)
        {
            confirmationPanel.SetActive(true);
            confirmationText.text = message;
            onConfirmYes = onYes;
            onConfirmNo = onNo;
        }
    }
    
    void OnConfirmYes()
    {
        confirmationPanel.SetActive(false);
        onConfirmYes?.Invoke();
    }
    
    void OnConfirmNo()
    {
        confirmationPanel.SetActive(false);
        onConfirmNo?.Invoke();
    }
    
    public void ShowLoading(string message = "Memuat...")
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
            loadingText.text = message;
        }
    }
    
    public void HideLoading()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
    
    public void UpdateLoadingProgress(float progress)
    {
        if (loadingBar != null)
            loadingBar.fillAmount = progress;
    }
    
    public void ShowToast(string message)
    {
        StartCoroutine(ShowToastCoroutine(message));
    }
    
    IEnumerator ShowToastCoroutine(string message)
    {
        if (toastPanel != null)
        {
            toastText.text = message;
            toastPanel.SetActive(true);
            
            yield return new WaitForSeconds(toastDuration);
            
            toastPanel.SetActive(false);
        }
    }
    
    public void ShowExitConfirmation()
    {
        ShowConfirmation(
            "Apakah kamu yakin ingin keluar?",
            () => GameManager.Instance.ExitApp(),
            null
        );
    }
    
    public void ShowResetQuizConfirmation()
    {
        ShowConfirmation(
            "Apakah kamu yakin ingin mengulang kuis? Progress kamu akan hilang.",
            () => {
                GameManager.Instance.ResetQuiz();
                GameManager.Instance.LoadScene("QuizIntro");
            },
            null
        );
    }
}