// // // // // using System.Collections;
// // // // // using System.Collections.Generic;
// // // // // using UnityEngine;
// // // // // using UnityEngine.UI; // Penting untuk LayoutRebuilder
// // // // // using TMPro;
// // // // // using System.Linq;

// // // // // public class QuizManager : MonoBehaviour
// // // // // {
// // // // //     [Header("Data Files")]
// // // // //     public TextAsset questionJson;
// // // // //     public TextAsset wrongFeedbackJson;
// // // // //     public TextAsset correctFeedbackJson;

// // // // //     [Header("Panels")]
// // // // //     public GameObject questionPanel;
// // // // //     public GameObject confirmationPopup;
// // // // //     public GameObject feedFalsePanel;
// // // // //     public GameObject feedCorrectPanel;

// // // // //     [Header("Question UI References")]
// // // // //     public TMP_Text descriptionText;
// // // // //     public Image questionImage; 
// // // // //     public Transform sentenceContainer; // Container untuk kalimat dengan {blank}
// // // // //     public GameObject textPrefab; // Prefab UI Text biasa
// // // // //     public GameObject slotPrefab; // Prefab UI Slot kosong (Button/Image)
// // // // //     public SmartGridSystem optionsGrid;
// // // // //     public Button checkButton; 
// // // // //     public TMP_Text checkButtonText;

// // // // //     [Header("Confirmation UI References")]
// // // // //     public TMP_Text confirmAnswerSummary;
// // // // //     public Button confirmSubmitBtn;
// // // // //     public Button confirmBackBtn;

// // // // //     [Header("Feedback UI References")]
// // // // //     public TMP_Text correctMessageText;
// // // // //     public TMP_Text wrongHeaderText;
// // // // //     public Transform wrongContentContainer; 
// // // // //     public GameObject wrongSectionPrefab; 

// // // // //     // State Variables
// // // // //     private QuestionList qList;
// // // // //     private WrongFeedbackList wList;
// // // // //     private CorrectFeedbackList cList;
    
// // // // //     private int currentQuestionIndex = 0;
// // // // //     private Dictionary<int, string> currentAnswers = new Dictionary<int, string>(); 
// // // // //     private List<GameObject> activeSentenceElements = new List<GameObject>();
// // // // //     private int totalBlanks = 0;

// // // // //     void Start()
// // // // //     {
// // // // //         LoadData();
// // // // //         ShowQuestion(0);
        
// // // // //         checkButton.onClick.AddListener(OnCheckUlangClicked);
// // // // //         confirmBackBtn.onClick.AddListener(() => TogglePopup(false));
// // // // //         confirmSubmitBtn.onClick.AddListener(EvaluateAnswer);
// // // // //     }

// // // // //     void LoadData()
// // // // //     {
// // // // //         if (questionJson != null)
// // // // //             qList = JsonUtility.FromJson<QuestionList>(questionJson.text);
// // // // //         if (wrongFeedbackJson != null)
// // // // //             wList = JsonUtility.FromJson<WrongFeedbackList>(wrongFeedbackJson.text);
// // // // //         if (correctFeedbackJson != null)
// // // // //             cList = JsonUtility.FromJson<CorrectFeedbackList>(correctFeedbackJson.text);
// // // // //     }

// // // // //     // --- LOGIC: QUESTION PANEL ---

// // // // //     void ShowQuestion(int index)
// // // // //     {
// // // // //         currentQuestionIndex = index;
// // // // //         currentAnswers.Clear();
        
// // // // //         // PERBAIKAN 1: Hapus semua anak di sentenceContainer dengan aman
// // // // //         foreach (Transform child in sentenceContainer) 
// // // // //         {
// // // // //             Destroy(child.gameObject);
// // // // //         }
// // // // //         activeSentenceElements.Clear();
        
// // // // //         if (qList == null || qList.questions == null || index >= qList.questions.Length) return;

// // // // //         QuestionData data = qList.questions[index];

// // // // //         // 1. Isi Data Statis
// // // // //         descriptionText.text = data.descriptionText;
// // // // //         if (data.imageSegment.Length > 0 && !string.IsNullOrEmpty(data.imageSegment[0]))
// // // // //         {
// // // // //             string path = data.imageSegment[0].Replace("Images/", "").Replace(".png", ""); 
// // // // //             questionImage.sprite = Resources.Load<Sprite>(path);
// // // // //         }

// // // // //         // 2. Bangun Kalimat (Sentence Structure)
// // // // //         totalBlanks = 0;
// // // // //         int slotIndexCounter = 0;

// // // // //         foreach (string part in data.sentenceStructure)
// // // // //         {
// // // // //             if (part == "{blank}")
// // // // //             {
// // // // //                 // Instantiate Slot
// // // // //                 GameObject slot = Instantiate(slotPrefab, sentenceContainer);
// // // // //                 int mySlotId = slotIndexCounter; 
                
// // // // //                 // Set text slot jadi "___"
// // // // //                 TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
// // // // //                 if (slotText != null) slotText.text = "___";
                
// // // // //                 // Listener untuk menghapus jawaban saat diklik
// // // // //                 Button slotBtn = slot.GetComponent<Button>();
// // // // //                 if (slotBtn != null && slotText != null)
// // // // //                 {
// // // // //                     slotBtn.onClick.AddListener(() => RemoveAnswerFromSlot(mySlotId, slotText));
// // // // //                 }

// // // // //                 activeSentenceElements.Add(slot);
// // // // //                 totalBlanks++;
// // // // //                 slotIndexCounter++;
// // // // //             }
// // // // //             else
// // // // //             {
// // // // //                 // Instantiate Teks Biasa
// // // // //                 GameObject txt = Instantiate(textPrefab, sentenceContainer);
// // // // //                 TMP_Text txtComp = txt.GetComponent<TMP_Text>();
// // // // //                 if (txtComp != null) txtComp.text = part;
                
// // // // //                 activeSentenceElements.Add(txt);
// // // // //             }
// // // // //         }

// // // // //         // 3. Setup Opsi Jawaban di Smart Grid
// // // // //         if (optionsGrid != null)
// // // // //         {
// // // // //             optionsGrid.GenerateOptions(data.options, OnOptionSelected);
// // // // //         }

// // // // //         checkButton.interactable = false;
// // // // //         checkButtonText.text = "CEK ULANG";

// // // // //         // PERBAIKAN 2: Tunggu 1 frame lalu rebuild layout agar posisi rapi
// // // // //         StartCoroutine(ForceRebuildLayout());
// // // // //     }

// // // // //     IEnumerator ForceRebuildLayout()
// // // // //     {
// // // // //         yield return new WaitForEndOfFrame();
// // // // //         if (sentenceContainer != null)
// // // // //             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
// // // // //     }

// // // // //     void OnOptionSelected(string answerText, GameObject btnObj)
// // // // //     {
// // // // //         for (int i = 0; i < totalBlanks; i++)
// // // // //         {
// // // // //             if (!currentAnswers.ContainsKey(i))
// // // // //             {
// // // // //                 currentAnswers.Add(i, answerText);
                
// // // // //                 UpdateSlotVisual(i, answerText);
                
// // // // //                 btnObj.SetActive(false); 
// // // // //                 if (optionsGrid != null) optionsGrid.RecalculateLayout(); 
                
// // // // //                 CheckCompletion();
// // // // //                 return;
// // // // //             }
// // // // //         }
// // // // //     }

// // // // //     void RemoveAnswerFromSlot(int slotId, TMP_Text uiText)
// // // // //     {
// // // // //         if (currentAnswers.ContainsKey(slotId))
// // // // //         {
// // // // //             string returnedAnswer = currentAnswers[slotId];
// // // // //             currentAnswers.Remove(slotId);
// // // // //             uiText.text = "___"; 

// // // // //             // Kembalikan tombol ke Grid
// // // // //             if (optionsGrid != null)
// // // // //             {
// // // // //                 foreach(Transform child in optionsGrid.transform)
// // // // //                 {
// // // // //                     TMP_Text btnText = child.GetComponentInChildren<TMP_Text>();
// // // // //                     if (!child.gameObject.activeSelf && btnText != null && btnText.text == returnedAnswer)
// // // // //                     {
// // // // //                         child.gameObject.SetActive(true);
// // // // //                         optionsGrid.RecalculateLayout(); 
// // // // //                         break;
// // // // //                     }
// // // // //                 }
// // // // //             }
            
// // // // //             // Refresh layout slot dan container
// // // // //             LayoutRebuilder.ForceRebuildLayoutImmediate(uiText.transform.parent.GetComponent<RectTransform>());
// // // // //             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());

// // // // //             CheckCompletion();
// // // // //         }
// // // // //     }

// // // // //     void UpdateSlotVisual(int slotId, string text)
// // // // //     {
// // // // //         int currentSlotCount = 0;
        
// // // // //         // PERBAIKAN 3: Deteksi Slot berdasarkan komponen Button, BUKAN nama objek
// // // // //         foreach(var obj in activeSentenceElements)
// // // // //         {
// // // // //             Button btn = obj.GetComponent<Button>();
            
// // // // //             if (btn != null) // Ini adalah Slot karena punya komponen Button
// // // // //             {
// // // // //                 if (currentSlotCount == slotId)
// // // // //                 {
// // // // //                     TMP_Text textComp = obj.GetComponentInChildren<TMP_Text>();
// // // // //                     if (textComp != null)
// // // // //                     {
// // // // //                         textComp.text = text;
// // // // //                     }
                    
// // // // //                     // PERBAIKAN 4: Paksa refresh layout tombol ini agar melebar sesuai teks jawaban
// // // // //                     LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
                    
// // // // //                     // Paksa refresh container utama agar elemen lain bergeser
// // // // //                     LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
                    
// // // // //                     break; 
// // // // //                 }
// // // // //                 currentSlotCount++;
// // // // //             }
// // // // //         }
// // // // //     }

// // // // //     void CheckCompletion()
// // // // //     {
// // // // //         checkButton.interactable = (currentAnswers.Count == totalBlanks);
// // // // //     }

// // // // //     void OnCheckUlangClicked()
// // // // //     {
// // // // //         TogglePopup(true);
        
// // // // //         string summary = "";
// // // // //         QuestionData data = qList.questions[currentQuestionIndex];
// // // // //         int slotCounter = 0;
        
// // // // //         foreach (string part in data.sentenceStructure)
// // // // //         {
// // // // //             if (part == "{blank}")
// // // // //             {
// // // // //                 if (currentAnswers.ContainsKey(slotCounter))
// // // // //                     summary += "<b>" + currentAnswers[slotCounter] + "</b>"; 
// // // // //                 slotCounter++;
// // // // //             }
// // // // //             else
// // // // //             {
// // // // //                 summary += part;
// // // // //             }
// // // // //         }
// // // // //         confirmAnswerSummary.text = summary;
// // // // //     }

// // // // //     void TogglePopup(bool show)
// // // // //     {
// // // // //         confirmationPopup.SetActive(show);
// // // // //     }

// // // // //     // --- LOGIC: EVALUATION ---

// // // // //     void EvaluateAnswer()
// // // // //     {
// // // // //         TogglePopup(false);
// // // // //         QuestionData data = qList.questions[currentQuestionIndex];
        
// // // // //         bool isCorrect = true;
        
// // // // //         for (int i = 0; i < totalBlanks; i++)
// // // // //         {
// // // // //             if (!currentAnswers.ContainsKey(i)) 
// // // // //             {
// // // // //                 isCorrect = false;
// // // // //                 break;
// // // // //             }

// // // // //             string userAnswer = currentAnswers[i].Trim();
// // // // //             string keyAnswer = data.correctAnswers[i].Trim();

// // // // //             if (!string.Equals(userAnswer, keyAnswer, System.StringComparison.OrdinalIgnoreCase))
// // // // //             {
// // // // //                 isCorrect = false;
// // // // //                 break;
// // // // //             }
// // // // //         }

// // // // //         if (isCorrect)
// // // // //         {
// // // // //             ShowCorrectFeedback();
// // // // //         }
// // // // //         else
// // // // //         {
// // // // //             ShowWrongFeedback();
// // // // //         }
// // // // //     }

// // // // //     // --- LOGIC: FEEDBACK ---

// // // // //     void ShowCorrectFeedback()
// // // // //     {
// // // // //         questionPanel.SetActive(false);
// // // // //         feedCorrectPanel.SetActive(true);

// // // // //         if (cList != null && cList.feedback_correct != null)
// // // // //         {
// // // // //             CorrectFeedbackData data = cList.feedback_correct.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
// // // // //             if (data != null)
// // // // //             {
// // // // //                 correctMessageText.text = data.correctAnswers;
// // // // //             }
// // // // //         }
// // // // //     }

// // // // //     void ShowWrongFeedback()
// // // // //     {
// // // // //         questionPanel.SetActive(false);
// // // // //         feedFalsePanel.SetActive(true);

// // // // //         // Bersihkan konten lama
// // // // //         foreach(Transform child in wrongContentContainer) Destroy(child.gameObject);

// // // // //         if (wList != null && wList.feedback_wrong != null)
// // // // //         {
// // // // //             WrongFeedbackData data = wList.feedback_wrong.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
            
// // // // //             if (data != null)
// // // // //             {
// // // // //                 wrongHeaderText.text = data.title;

// // // // //                 foreach (var section in data.sections)
// // // // //                 {
// // // // //                     GameObject secObj = Instantiate(wrongSectionPrefab, wrongContentContainer);
                    
// // // // //                     TMP_Text headerTxt = secObj.transform.Find("Header")?.GetComponent<TMP_Text>();
// // // // //                     if(headerTxt) headerTxt.text = section.header;

// // // // //                     TMP_Text contentTxt = secObj.transform.Find("Content")?.GetComponent<TMP_Text>();
// // // // //                     if(contentTxt) contentTxt.text = section.content;

// // // // //                     Image img = secObj.transform.Find("SectionImage")?.GetComponent<Image>();
// // // // //                     if (img != null)
// // // // //                     {
// // // // //                         if (!string.IsNullOrEmpty(section.image_path))
// // // // //                         {
// // // // //                             img.gameObject.SetActive(true);
// // // // //                             // Load sprite logic here
// // // // //                         }
// // // // //                         else
// // // // //                         {
// // // // //                             img.gameObject.SetActive(false);
// // // // //                         }
// // // // //                     }
// // // // //                 }
// // // // //             }
// // // // //         }
// // // // //     }
    
// // // // //     public void BackToQuestion()
// // // // //     {
// // // // //         feedFalsePanel.SetActive(false);
// // // // //         feedCorrectPanel.SetActive(false);
// // // // //         questionPanel.SetActive(true);
// // // // //     }
// // // // // }

// // // // using System.Collections;
// // // // using System.Collections.Generic;
// // // // using UnityEngine;
// // // // using UnityEngine.UI; // Penting untuk LayoutRebuilder
// // // // using TMPro;
// // // // using System.Linq;

// // // // public class QuizManager : MonoBehaviour
// // // // {
// // // //     [Header("Data Files")]
// // // //     public TextAsset questionJson;
// // // //     public TextAsset wrongFeedbackJson;
// // // //     public TextAsset correctFeedbackJson;

// // // //     [Header("Panels")]
// // // //     public GameObject questionPanel;
// // // //     public GameObject confirmationPopup;
// // // //     public GameObject feedFalsePanel;
// // // //     public GameObject feedCorrectPanel;

// // // //     [Header("Question UI References")]
// // // //     public TMP_Text descriptionText;
// // // //     public Image questionImage; 
// // // //     public Transform sentenceContainer; // Container untuk kalimat dengan {blank}
// // // //     public GameObject textPrefab; // Prefab UI Text biasa
// // // //     public GameObject slotPrefab; // Prefab UI Slot kosong (Button/Image)
// // // //     public SmartGridSystem optionsGrid;
// // // //     public Button checkButton; 
// // // //     public TMP_Text checkButtonText;

// // // //     [Header("Confirmation UI References")]
// // // //     public TMP_Text confirmAnswerSummary;
// // // //     public Button confirmSubmitBtn;
// // // //     public Button confirmBackBtn;

// // // //     [Header("Feedback UI References")]
// // // //     public TMP_Text correctMessageText;
// // // //     public TMP_Text wrongHeaderText;
// // // //     public Transform wrongContentContainer; 
// // // //     public GameObject wrongSectionPrefab; 

// // // //     // State Variables
// // // //     private QuestionList qList;
// // // //     private WrongFeedbackList wList;
// // // //     private CorrectFeedbackList cList;
    
// // // //     private int currentQuestionIndex = 0;
// // // //     private Dictionary<int, string> currentAnswers = new Dictionary<int, string>(); 
// // // //     private List<GameObject> activeSentenceElements = new List<GameObject>();
// // // //     private int totalBlanks = 0;

// // // //     void Start()
// // // //     {
// // // //         LoadData();
// // // //         ShowQuestion(0);
        
// // // //         checkButton.onClick.AddListener(OnCheckUlangClicked);
// // // //         confirmBackBtn.onClick.AddListener(() => TogglePopup(false));
// // // //         confirmSubmitBtn.onClick.AddListener(EvaluateAnswer);
// // // //     }

// // // //     void LoadData()
// // // //     {
// // // //         if (questionJson != null)
// // // //             qList = JsonUtility.FromJson<QuestionList>(questionJson.text);
// // // //         if (wrongFeedbackJson != null)
// // // //             wList = JsonUtility.FromJson<WrongFeedbackList>(wrongFeedbackJson.text);
// // // //         if (correctFeedbackJson != null)
// // // //             cList = JsonUtility.FromJson<CorrectFeedbackList>(correctFeedbackJson.text);
// // // //     }

// // // //     // --- LOGIC: QUESTION PANEL ---

// // // //     void ShowQuestion(int index)
// // // //     {
// // // //         currentQuestionIndex = index;
// // // //         currentAnswers.Clear();
        
// // // //         // PERBAIKAN 1: Hapus semua anak di sentenceContainer dengan aman
// // // //         foreach (Transform child in sentenceContainer) 
// // // //         {
// // // //             Destroy(child.gameObject);
// // // //         }
// // // //         activeSentenceElements.Clear();
        
// // // //         if (qList == null || qList.questions == null || index >= qList.questions.Length) return;

// // // //         QuestionData data = qList.questions[index];

// // // //         // 1. Isi Data Statis
// // // //         descriptionText.text = data.descriptionText;
// // // //         if (data.imageSegment.Length > 0 && !string.IsNullOrEmpty(data.imageSegment[0]))
// // // //         {
// // // //             string path = data.imageSegment[0].Replace("Images/", "").Replace(".png", ""); 
// // // //             questionImage.sprite = Resources.Load<Sprite>(path);
// // // //         }

// // // //         // 2. Bangun Kalimat (Sentence Structure)
// // // //         totalBlanks = 0;
// // // //         int slotIndexCounter = 0;

// // // //         foreach (string part in data.sentenceStructure)
// // // //         {
// // // //             if (part == "{blank}")
// // // //             {
// // // //                 // Instantiate Slot
// // // //                 GameObject slot = Instantiate(slotPrefab, sentenceContainer);
                
// // // //                 // PERBAIKAN UTAMA: Paksa Skala kembali ke 1,1,1
// // // //                 slot.transform.localScale = Vector3.one; 
                
// // // //                 int mySlotId = slotIndexCounter; 
                
// // // //                 // Set text slot jadi "___"
// // // //                 TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
// // // //                 if (slotText != null) slotText.text = "___";
                
// // // //                 // Listener untuk menghapus jawaban saat diklik
// // // //                 Button slotBtn = slot.GetComponent<Button>();
// // // //                 if (slotBtn != null && slotText != null)
// // // //                 {
// // // //                     slotBtn.onClick.AddListener(() => RemoveAnswerFromSlot(mySlotId, slotText));
// // // //                 }

// // // //                 activeSentenceElements.Add(slot);
// // // //                 totalBlanks++;
// // // //                 slotIndexCounter++;
// // // //             }
// // // //             else
// // // //             {
// // // //                 // Instantiate Teks Biasa
// // // //                 GameObject txt = Instantiate(textPrefab, sentenceContainer);
                
// // // //                 // PERBAIKAN UTAMA: Paksa Skala kembali ke 1,1,1
// // // //                 txt.transform.localScale = Vector3.one;

// // // //                 TMP_Text txtComp = txt.GetComponent<TMP_Text>();
// // // //                 if (txtComp != null) txtComp.text = part;
                
// // // //                 activeSentenceElements.Add(txt);
// // // //             }
// // // //         }

// // // //         // 3. Setup Opsi Jawaban di Smart Grid
// // // //         if (optionsGrid != null)
// // // //         {
// // // //             optionsGrid.GenerateOptions(data.options, OnOptionSelected);
// // // //         }

// // // //         checkButton.interactable = false;
// // // //         checkButtonText.text = "CEK ULANG";

// // // //         // PERBAIKAN 2: Tunggu frame, lalu paksa update canvas
// // // //         StartCoroutine(ForceRebuildLayout());
// // // //     }

// // // //     IEnumerator ForceRebuildLayout()
// // // //     {
// // // //         // Tunggu sampai akhir frame agar semua Instantiate selesai
// // // //         yield return new WaitForEndOfFrame();
        
// // // //         if (sentenceContainer != null)
// // // //         {
// // // //             // Update Canvas secara global
// // // //             Canvas.ForceUpdateCanvases();
            
// // // //             // Rebuild container kalimat
// // // //             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
            
// // // //             // Rebuild juga parentnya (misalnya Content dari ScrollView) agar ukurannya pas
// // // //             if (sentenceContainer.parent != null)
// // // //             {
// // // //                  LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.parent.GetComponent<RectTransform>());
// // // //             }
// // // //         }
// // // //     }

// // // //     void OnOptionSelected(string answerText, GameObject btnObj)
// // // //     {
// // // //         for (int i = 0; i < totalBlanks; i++)
// // // //         {
// // // //             if (!currentAnswers.ContainsKey(i))
// // // //             {
// // // //                 currentAnswers.Add(i, answerText);
                
// // // //                 UpdateSlotVisual(i, answerText);
                
// // // //                 btnObj.SetActive(false); 
// // // //                 if (optionsGrid != null) optionsGrid.RecalculateLayout(); 
                
// // // //                 CheckCompletion();
// // // //                 return;
// // // //             }
// // // //         }
// // // //     }

// // // //     void RemoveAnswerFromSlot(int slotId, TMP_Text uiText)
// // // //     {
// // // //         if (currentAnswers.ContainsKey(slotId))
// // // //         {
// // // //             string returnedAnswer = currentAnswers[slotId];
// // // //             currentAnswers.Remove(slotId);
// // // //             uiText.text = "___"; 

// // // //             // Kembalikan tombol ke Grid
// // // //             if (optionsGrid != null)
// // // //             {
// // // //                 foreach(Transform child in optionsGrid.transform)
// // // //                 {
// // // //                     TMP_Text btnText = child.GetComponentInChildren<TMP_Text>();
// // // //                     if (!child.gameObject.activeSelf && btnText != null && btnText.text == returnedAnswer)
// // // //                     {
// // // //                         child.gameObject.SetActive(true);
// // // //                         optionsGrid.RecalculateLayout(); 
// // // //                         break;
// // // //                     }
// // // //                 }
// // // //             }
            
// // // //             // Refresh layout slot dan container
// // // //             LayoutRebuilder.ForceRebuildLayoutImmediate(uiText.transform.parent.GetComponent<RectTransform>());
// // // //             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());

// // // //             CheckCompletion();
// // // //         }
// // // //     }

// // // //     void UpdateSlotVisual(int slotId, string text)
// // // //     {
// // // //         int currentSlotCount = 0;
        
// // // //         // Deteksi Slot berdasarkan komponen Button
// // // //         foreach(var obj in activeSentenceElements)
// // // //         {
// // // //             Button btn = obj.GetComponent<Button>();
            
// // // //             if (btn != null) // Ini adalah Slot karena punya komponen Button
// // // //             {
// // // //                 if (currentSlotCount == slotId)
// // // //                 {
// // // //                     TMP_Text textComp = obj.GetComponentInChildren<TMP_Text>();
// // // //                     if (textComp != null)
// // // //                     {
// // // //                         textComp.text = text;
// // // //                     }
                    
// // // //                     // Paksa refresh layout tombol ini agar melebar sesuai teks jawaban
// // // //                     LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
                    
// // // //                     // Paksa refresh container utama agar elemen lain bergeser
// // // //                     LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
                    
// // // //                     break; 
// // // //                 }
// // // //                 currentSlotCount++;
// // // //             }
// // // //         }
// // // //     }

// // // //     void CheckCompletion()
// // // //     {
// // // //         checkButton.interactable = (currentAnswers.Count == totalBlanks);
// // // //     }

// // // //     void OnCheckUlangClicked()
// // // //     {
// // // //         TogglePopup(true);
        
// // // //         string summary = "";
// // // //         QuestionData data = qList.questions[currentQuestionIndex];
// // // //         int slotCounter = 0;
        
// // // //         foreach (string part in data.sentenceStructure)
// // // //         {
// // // //             if (part == "{blank}")
// // // //             {
// // // //                 if (currentAnswers.ContainsKey(slotCounter))
// // // //                     summary += "<b>" + currentAnswers[slotCounter] + "</b>"; 
// // // //                 slotCounter++;
// // // //             }
// // // //             else
// // // //             {
// // // //                 summary += part;
// // // //             }
// // // //         }
// // // //         confirmAnswerSummary.text = summary;
// // // //     }

// // // //     void TogglePopup(bool show)
// // // //     {
// // // //         confirmationPopup.SetActive(show);
// // // //     }

// // // //     // --- LOGIC: EVALUATION ---

// // // //     void EvaluateAnswer()
// // // //     {
// // // //         TogglePopup(false);
// // // //         QuestionData data = qList.questions[currentQuestionIndex];
        
// // // //         bool isCorrect = true;
        
// // // //         for (int i = 0; i < totalBlanks; i++)
// // // //         {
// // // //             if (!currentAnswers.ContainsKey(i)) 
// // // //             {
// // // //                 isCorrect = false;
// // // //                 break;
// // // //             }

// // // //             string userAnswer = currentAnswers[i].Trim();
// // // //             string keyAnswer = data.correctAnswers[i].Trim();

// // // //             if (!string.Equals(userAnswer, keyAnswer, System.StringComparison.OrdinalIgnoreCase))
// // // //             {
// // // //                 isCorrect = false;
// // // //                 break;
// // // //             }
// // // //         }

// // // //         if (isCorrect)
// // // //         {
// // // //             ShowCorrectFeedback();
// // // //         }
// // // //         else
// // // //         {
// // // //             ShowWrongFeedback();
// // // //         }
// // // //     }

// // // //     // --- LOGIC: FEEDBACK ---

// // // //     void ShowCorrectFeedback()
// // // //     {
// // // //         questionPanel.SetActive(false);
// // // //         feedCorrectPanel.SetActive(true);

// // // //         if (cList != null && cList.feedback_correct != null)
// // // //         {
// // // //             CorrectFeedbackData data = cList.feedback_correct.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
// // // //             if (data != null)
// // // //             {
// // // //                 correctMessageText.text = data.correctAnswers;
// // // //             }
// // // //         }
// // // //     }

// // // //     void ShowWrongFeedback()
// // // //     {
// // // //         questionPanel.SetActive(false);
// // // //         feedFalsePanel.SetActive(true);

// // // //         // Bersihkan konten lama
// // // //         foreach(Transform child in wrongContentContainer) Destroy(child.gameObject);

// // // //         if (wList != null && wList.feedback_wrong != null)
// // // //         {
// // // //             WrongFeedbackData data = wList.feedback_wrong.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
            
// // // //             if (data != null)
// // // //             {
// // // //                 wrongHeaderText.text = data.title;

// // // //                 foreach (var section in data.sections)
// // // //                 {
// // // //                     GameObject secObj = Instantiate(wrongSectionPrefab, wrongContentContainer);
                    
// // // //                     TMP_Text headerTxt = secObj.transform.Find("Header")?.GetComponent<TMP_Text>();
// // // //                     if(headerTxt) headerTxt.text = section.header;

// // // //                     TMP_Text contentTxt = secObj.transform.Find("Content")?.GetComponent<TMP_Text>();
// // // //                     if(contentTxt) contentTxt.text = section.content;

// // // //                     Image img = secObj.transform.Find("SectionImage")?.GetComponent<Image>();
// // // //                     if (img != null)
// // // //                     {
// // // //                         if (!string.IsNullOrEmpty(section.image_path))
// // // //                         {
// // // //                             img.gameObject.SetActive(true);
// // // //                             // Load sprite logic here
// // // //                         }
// // // //                         else
// // // //                         {
// // // //                             img.gameObject.SetActive(false);
// // // //                         }
// // // //                     }
// // // //                 }
// // // //             }
// // // //         }
// // // //     }
    
// // // //     public void BackToQuestion()
// // // //     {
// // // //         feedFalsePanel.SetActive(false);
// // // //         feedCorrectPanel.SetActive(false);
// // // //         questionPanel.SetActive(true);
// // // //     }
// // // // }


// // // using System.Collections;
// // // using System.Collections.Generic;
// // // using UnityEngine;
// // // using UnityEngine.UI; // Penting untuk LayoutRebuilder
// // // using TMPro;
// // // using System.Linq;

// // // public class QuizManager : MonoBehaviour
// // // {
// // //     [Header("Data Files")]
// // //     public TextAsset questionJson;
// // //     public TextAsset wrongFeedbackJson;
// // //     public TextAsset correctFeedbackJson;

// // //     [Header("Panels")]
// // //     public GameObject questionPanel;
// // //     public GameObject confirmationPopup;
// // //     public GameObject feedFalsePanel;
// // //     public GameObject feedCorrectPanel;

// // //     [Header("Question UI References")]
// // //     public TMP_Text descriptionText;
// // //     public Image questionImage; 
// // //     public Transform sentenceContainer; // Container untuk kalimat dengan {blank}
// // //     public GameObject textPrefab; // Prefab UI Text biasa
// // //     public GameObject slotPrefab; // Prefab UI Slot kosong (Button/Image)
// // //     public SmartGridSystem optionsGrid;
// // //     public Button checkButton; 
// // //     public TMP_Text checkButtonText;

// // //     [Header("Confirmation UI References")]
// // //     public Color summaryHighlightColor = Color.green;
// // //     public TMP_Text confirmAnswerSummary;
// // //     public Button confirmSubmitBtn;
// // //     public Button confirmBackBtn;

// // //     [Header("Feedback UI References")]
// // //     public TMP_Text correctMessageText;
// // //     public TMP_Text wrongHeaderText;
// // //     public Transform wrongContentContainer; 
// // //     public GameObject wrongSectionPrefab; 

// // //     // State Variables
// // //     private QuestionList qList;
// // //     private WrongFeedbackList wList;
// // //     private CorrectFeedbackList cList;
    
// // //     private int currentQuestionIndex = 0;
// // //     private Dictionary<int, string> currentAnswers = new Dictionary<int, string>(); 
// // //     private List<GameObject> activeSentenceElements = new List<GameObject>();
// // //     private int totalBlanks = 0;

// // //     void Start()
// // //     {
// // //         LoadData();
// // //         ShowQuestion(0);
        
// // //         checkButton.onClick.AddListener(OnCheckUlangClicked);
// // //         confirmBackBtn.onClick.AddListener(() => TogglePopup(false));
// // //         confirmSubmitBtn.onClick.AddListener(EvaluateAnswer);
// // //     }

// // //     void LoadData()
// // //     {
// // //         if (questionJson != null)
// // //             qList = JsonUtility.FromJson<QuestionList>(questionJson.text);
// // //         if (wrongFeedbackJson != null)
// // //             wList = JsonUtility.FromJson<WrongFeedbackList>(wrongFeedbackJson.text);
// // //         if (correctFeedbackJson != null)
// // //             cList = JsonUtility.FromJson<CorrectFeedbackList>(correctFeedbackJson.text);
// // //     }

// // //     // --- LOGIC: QUESTION PANEL ---

// // //     void ShowQuestion(int index)
// // //     {
// // //         currentQuestionIndex = index;
// // //         currentAnswers.Clear();
        
// // //         // PERBAIKAN 1: Hapus semua anak di sentenceContainer dengan aman
// // //         foreach (Transform child in sentenceContainer) 
// // //         {
// // //             Destroy(child.gameObject);
// // //         }
// // //         activeSentenceElements.Clear();
        
// // //         if (qList == null || qList.questions == null || index >= qList.questions.Length) return;

// // //         QuestionData data = qList.questions[index];

// // //         // 1. Isi Data Statis
// // //         descriptionText.text = data.descriptionText;
// // //         if (data.imageSegment.Length > 0 && !string.IsNullOrEmpty(data.imageSegment[0]))
// // //         {
// // //             string rawPath = data.imageSegment[0]; // Ambil: "Assets/Resources/Quiz_images_src/..."
// // //             string loadPath = rawPath;

// // //             // LANGKAH 1: Potong prefix "Assets/Resources/" jika ada
// // //             // Resources.Load minta path relatif dari folder Resources
// // //             if (rawPath.Contains("Resources/"))
// // //             {
// // //                 // Mengambil substring tepat setelah kata "Resources/"
// // //                 loadPath = rawPath.Substring(rawPath.IndexOf("Resources/") + "Resources/".Length);
// // //             }

// // //             // LANGKAH 2: Buang ekstensi file (.png / .jpg)
// // //             // Resources.Load akan gagal jika ada ekstensi
// // //             int dotIndex = loadPath.LastIndexOf('.');
// // //             if (dotIndex != -1)
// // //             {
// // //                 loadPath = loadPath.Substring(0, dotIndex);
// // //             }

// // //             // Debugging (Opsional: Cek di Console apakah path sudah benar)
// // //             // Debug.Log($"Original: {rawPath} | Loading: {loadPath}");

// // //             // LANGKAH 3: Load Sprite
// // //             Sprite loadedSprite = Resources.Load<Sprite>(loadPath);

// // //             if (loadedSprite != null)
// // //             {
// // //                 questionImage.sprite = loadedSprite;
// // //                 questionImage.gameObject.SetActive(true); // Pastikan Image nyala
                
// // //                 // Opsional: Agar gambar tidak gepeng, reset aspek rasionya
// // //                 questionImage.preserveAspect = true; 
// // //             }
// // //             else
// // //             {
// // //                 Debug.LogError($"Gambar tidak ditemukan di path: {loadPath}. Cek folder Resources!");
// // //                 questionImage.gameObject.SetActive(false); // Sembunyikan jika gagal load
// // //             }
// // //         }
// // //         else
// // //         {
// // //             // Jika tidak ada data gambar di JSON, sembunyikan Image ImageSegment
// // //             questionImage.gameObject.SetActive(false);
// // //         }

// // //         // 2. Bangun Kalimat (Sentence Structure)
// // //         totalBlanks = 0;
// // //         int slotIndexCounter = 0;

// // //         foreach (string part in data.sentenceStructure)
// // //         {
// // //             if (part == "{blank}")
// // //             {
// // //                 // PERBAIKAN UTAMA: Instantiate dengan 'false' agar tidak mengikuti posisi World yang aneh
// // //                 GameObject slot = Instantiate(slotPrefab, sentenceContainer, false);

// // //                 // TAMBAHAN 1: Paksa GameObject nyala
// // //                 slot.SetActive(true); 

// // //                 // TAMBAHAN 2: Paksa Scale Normal
// // //                 slot.transform.localScale = Vector3.one; 
                
// // //                 // TAMBAHAN 3: Paksa komponen Button dan Image nyala (Jaga-jaga)
// // //                 Button btn = slot.GetComponent<Button>();
// // //                 if(btn) btn.enabled = true;
// // //                 Image img = slot.GetComponent<Image>();
// // //                 if(img) img.enabled = true;
                
// // //                 // Pastikan Transform bersih
// // //                 slot.transform.localScale = Vector3.one;
// // //                 slot.transform.localPosition = Vector3.zero;
// // //                 slot.transform.localRotation = Quaternion.identity;
                
// // //                 int mySlotId = slotIndexCounter; 
                
// // //                 // Set text slot jadi "___"
// // //                 TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
// // //                 if (slotText != null)
// // //                 {
// // //                     slotText.text = "_________";
// // //                     slotText.ForceMeshUpdate(); // TAMBAHAN PENTING: Paksa hitung ukuran detik ini juga!
// // //                 };
                
// // //                 // Listener untuk menghapus jawaban saat diklik
// // //                 Button slotBtn = slot.GetComponent<Button>();
// // //                 if (slotBtn != null && slotText != null)
// // //                 {
// // //                     slotBtn.onClick.AddListener(() => RemoveAnswerFromSlot(mySlotId, slotText));
// // //                 }

// // //                 activeSentenceElements.Add(slot);
// // //                 totalBlanks++;
// // //                 slotIndexCounter++;
// // //             }
// // //             else
// // //             {
// // //                 // Instantiate Teks Biasa dengan 'false'
// // //                 GameObject txt = Instantiate(textPrefab, sentenceContainer, false);
                
// // //                 // TAMBAHAN: Paksa nyala & normal
// // //                 txt.SetActive(true);
// // //                 txt.transform.localScale = Vector3.one;
                
// // //                 // Pastikan Transform bersih
// // //                 txt.transform.localScale = Vector3.one;
// // //                 txt.transform.localPosition = Vector3.zero;
// // //                 txt.transform.localRotation = Quaternion.identity;

// // //                 TMP_Text txtComp = txt.GetComponent<TMP_Text>();
// // //                 if (txtComp != null)                
// // //                 {
// // //                     txtComp.text = part;
// // //                     txtComp.ForceMeshUpdate(); // TAMBAHAN PENTING: Paksa hitung ukuran text!
// // //                 };
                
// // //                 activeSentenceElements.Add(txt);
// // //             }
// // //         }

// // //         // 3. Setup Opsi Jawaban di Smart Grid
// // //         if (optionsGrid != null)
// // //         {
// // //             optionsGrid.GenerateOptions(data.options, OnOptionSelected);
// // //         }

// // //         checkButton.interactable = false;
// // //         checkButtonText.text = "CEK ULANG";

// // //         // PERBAIKAN 2: Gunakan "Nuclear Option" refresh layout
// // //         StartCoroutine(ForceRebuildLayout());
// // //     }

// // //     IEnumerator ForceRebuildLayout()
// // //     {
// // //         // Tunggu sampai akhir frame agar TMP selesai render visualnya
// // //         yield return new WaitForEndOfFrame();

// // //         if (sentenceContainer != null)
// // //         {
// // //             // 1. Rebuild ukuran masing-masing elemen Text & Slot dulu
// // //             foreach(RectTransform child in sentenceContainer)
// // //             {
// // //                 LayoutRebuilder.ForceRebuildLayoutImmediate(child);
// // //             }

// // //             // 2. Baru Rebuild Container-nya (FlowLayoutGroup bekerja di sini)
// // //             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
            
// // //             // 3. Matikan-Nyalakan untuk memancing "OnEnable" (Trik ampuh Unity UI)
// // //             sentenceContainer.gameObject.SetActive(false);
// // //             sentenceContainer.gameObject.SetActive(true);
// // //         }
// // //     }

// // //     void OnOptionSelected(string answerText, GameObject btnObj)
// // //     {
// // //         for (int i = 0; i < totalBlanks; i++)
// // //         {
// // //             if (!currentAnswers.ContainsKey(i))
// // //             {
// // //                 currentAnswers.Add(i, answerText);
                
// // //                 UpdateSlotVisual(i, answerText);
                
// // //                 btnObj.SetActive(false); 
// // //                 if (optionsGrid != null) optionsGrid.RecalculateLayout(); 
                
// // //                 CheckCompletion();
// // //                 return;
// // //             }
// // //         }
// // //     }

// // //     void RemoveAnswerFromSlot(int slotId, TMP_Text uiText)
// // //     {
// // //         if (currentAnswers.ContainsKey(slotId))
// // //         {
// // //             string returnedAnswer = currentAnswers[slotId];
// // //             currentAnswers.Remove(slotId);
// // //             uiText.text = "___"; 

// // //             // Kembalikan tombol ke Grid
// // //             if (optionsGrid != null)
// // //             {
// // //                 foreach(Transform child in optionsGrid.transform)
// // //                 {
// // //                     TMP_Text btnText = child.GetComponentInChildren<TMP_Text>();
// // //                     if (!child.gameObject.activeSelf && btnText != null && btnText.text == returnedAnswer)
// // //                     {
// // //                         child.gameObject.SetActive(true);
// // //                         optionsGrid.RecalculateLayout(); 
// // //                         break;
// // //                     }
// // //                 }
// // //             }
            
// // //             // Refresh layout slot dan container
// // //             LayoutRebuilder.ForceRebuildLayoutImmediate(uiText.transform.parent.GetComponent<RectTransform>());
// // //             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());

// // //             CheckCompletion();
// // //         }
// // //     }

// // //     void UpdateSlotVisual(int slotId, string text)
// // //     {
// // //         int currentSlotCount = 0;
        
// // //         // Deteksi Slot berdasarkan komponen Button
// // //         foreach(var obj in activeSentenceElements)
// // //         {
// // //             Button btn = obj.GetComponent<Button>();
            
// // //             if (btn != null) // Ini adalah Slot karena punya komponen Button
// // //             {
// // //                 if (currentSlotCount == slotId)
// // //                 {
// // //                     TMP_Text textComp = obj.GetComponentInChildren<TMP_Text>();
// // //                     if (textComp != null)
// // //                     {
// // //                         textComp.text = text;
// // //                     }
                    
// // //                     // Paksa refresh layout tombol ini agar melebar sesuai teks jawaban
// // //                     LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
                    
// // //                     // Paksa refresh container utama agar elemen lain bergeser
// // //                     LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
                    
// // //                     break; 
// // //                 }
// // //                 currentSlotCount++;
// // //             }
// // //         }
// // //     }

// // //     void CheckCompletion()
// // //     {
// // //         checkButton.interactable = (currentAnswers.Count == totalBlanks);
// // //     }

// // //     void OnCheckUlangClicked()
// // //     {
// // //         TogglePopup(true);
        
// // //         string summary = "";
// // //         QuestionData data = qList.questions[currentQuestionIndex];
// // //         int slotCounter = 0;

// // //         string colorHex = "#" + ColorUtility.ToHtmlStringRGB(summaryHighlightColor);
        
// // //         foreach (string part in data.sentenceStructure)
// // //         {
// // //             if (part == "{blank}")
// // //             {
// // //                 if (currentAnswers.ContainsKey(slotCounter))
// // //                 {
// // //                     // 2. Masukkan tag <color> di sekitar jawaban
// // //                     string answer = currentAnswers[slotCounter];
// // //                     summary += $"<color={colorHex}><b>{answer}</b></color>";
// // //                 }
// // //                 slotCounter++;
// // //             }
// // //             else
// // //             {
// // //                 summary += part;
// // //             }
// // //         }
// // //         confirmAnswerSummary.text = summary;
// // //     }

// // //     void TogglePopup(bool show)
// // //     {
// // //         confirmationPopup.SetActive(show);
// // //     }

// // //     // --- LOGIC: EVALUATION ---

// // //     void EvaluateAnswer()
// // //     {
// // //         TogglePopup(false);
// // //         QuestionData data = qList.questions[currentQuestionIndex];
        
// // //         bool isCorrect = true;
        
// // //         for (int i = 0; i < totalBlanks; i++)
// // //         {
// // //             if (!currentAnswers.ContainsKey(i)) 
// // //             {
// // //                 isCorrect = false;
// // //                 break;
// // //             }

// // //             string userAnswer = currentAnswers[i].Trim();
// // //             string keyAnswer = data.correctAnswers[i].Trim();

// // //             if (!string.Equals(userAnswer, keyAnswer, System.StringComparison.OrdinalIgnoreCase))
// // //             {
// // //                 isCorrect = false;
// // //                 break;
// // //             }
// // //         }

// // //         if (isCorrect)
// // //         {
// // //             ShowCorrectFeedback();
// // //         }
// // //         else
// // //         {
// // //             ShowWrongFeedback();
// // //         }
// // //     }

// // //     // --- LOGIC: FEEDBACK ---

// // //     void ShowCorrectFeedback()
// // //     {
// // //         questionPanel.SetActive(false);
// // //         feedCorrectPanel.SetActive(true);

// // //         if (cList != null && cList.feedback_correct != null)
// // //         {
// // //             CorrectFeedbackData data = cList.feedback_correct.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
// // //             if (data != null)
// // //             {
// // //                 correctMessageText.text = data.correctAnswers;
// // //             }
// // //         }
// // //     }

// // //     void ShowWrongFeedback()
// // //     {
// // //         questionPanel.SetActive(false);
// // //         feedFalsePanel.SetActive(true);

// // //         // Bersihkan konten lama
// // //         foreach(Transform child in wrongContentContainer) Destroy(child.gameObject);

// // //         if (wList != null && wList.feedback_wrong != null)
// // //         {
// // //             WrongFeedbackData data = wList.feedback_wrong.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
            
// // //             if (data != null)
// // //             {
// // //                 wrongHeaderText.text = data.title;

// // //                 foreach (var section in data.sections)
// // //                 {
// // //                     GameObject secObj = Instantiate(wrongSectionPrefab, wrongContentContainer);
                    
// // //                     TMP_Text headerTxt = secObj.transform.Find("Header")?.GetComponent<TMP_Text>();
// // //                     if(headerTxt) headerTxt.text = section.header;

// // //                     TMP_Text contentTxt = secObj.transform.Find("Content")?.GetComponent<TMP_Text>();
// // //                     if(contentTxt) contentTxt.text = section.content;

// // //                     Image img = secObj.transform.Find("SectionImage")?.GetComponent<Image>();
// // //                     if (img != null)
// // //                     {
// // //                         if (!string.IsNullOrEmpty(section.image_path))
// // //                         {
// // //                             img.gameObject.SetActive(true);
// // //                             // Load sprite logic here
// // //                         }
// // //                         else
// // //                         {
// // //                             img.gameObject.SetActive(false);
// // //                         }
// // //                     }
// // //                 }
// // //             }
// // //         }
// // //     }
    
// // //     public void BackToQuestion()
// // //     {
// // //         feedFalsePanel.SetActive(false);
// // //         feedCorrectPanel.SetActive(false);
// // //         questionPanel.SetActive(true);
// // //     }
// // // }

// // using System.Collections;
// // using System.Collections.Generic;
// // using UnityEngine;
// // using UnityEngine.UI; // Penting untuk LayoutRebuilder
// // using TMPro;
// // using System.Linq;
// // using UnityEngine.SceneManagement; // TAMBAHAN: Penting untuk pindah Scene


// // public class QuizManager : MonoBehaviour
// // {
// //     [Header("Data Files")]
// //     public TextAsset questionJson;
// //     public TextAsset wrongFeedbackJson;
// //     public TextAsset correctFeedbackJson;

// //     [Header("Panels")]
// //     public GameObject questionPanel;
// //     public GameObject confirmationPopup;
// //     public GameObject feedFalsePanel;
// //     public GameObject feedCorrectPanel;

// //     [Header("Question UI References")]
// //     public TMP_Text descriptionText;
// //     public Image questionImage; 
// //     public Transform sentenceContainer; // Container untuk kalimat dengan {blank}
// //     public GameObject textPrefab; // Prefab UI Text biasa
// //     public GameObject slotPrefab; // Prefab UI Slot kosong (Button/Image)
// //     public SmartGridSystem optionsGrid;
// //     public Button checkButton; 
// //     public TMP_Text checkButtonText;

// //     [Header("Confirmation UI References")]
// //     public Color summaryHighlightColor = Color.green;
// //     public TMP_Text confirmAnswerSummary;
// //     public Button confirmSubmitBtn;
// //     public Button confirmBackBtn;

// //     [Header("Feedback UI References")]
// //     public TMP_Text correctMessageText;
// //     public TMP_Text wrongHeaderText;
// //     public Transform wrongContentContainer; 
// //     public GameObject wrongSectionPrefab; 

// //     [Header("Scene Navigation")]
// //     // TAMBAHAN: Referensi tombol untuk pindah scene
// //     public Button feedFalseBackButton; // Tombol "KEMBALI" di panel salah
// //     public Button feedCorrectNextButton; // Tombol "LANJUTKAN" di panel benar
// //     public string completionSceneName = "CompletionQuiz"; // Nama scene tujuan

// //     // State Variables
// //     private QuestionList qList;
// //     private WrongFeedbackList wList;
// //     private CorrectFeedbackList cList;
    
// //     private int currentQuestionIndex = 0;
// //     private Dictionary<int, string> currentAnswers = new Dictionary<int, string>(); 
// //     private List<GameObject> activeSentenceElements = new List<GameObject>();
// //     private int totalBlanks = 0;

// //     void Start()
// //     {
// //         LoadData();
// //         ShowQuestion(0);
        
// //         // Setup tombol UI utama
// //         checkButton.onClick.AddListener(OnCheckUlangClicked);
// //         confirmBackBtn.onClick.AddListener(() => TogglePopup(false));
// //         confirmSubmitBtn.onClick.AddListener(EvaluateAnswer);

// //         // TAMBAHAN: Setup tombol Navigasi ke Scene Completion
// //         if (feedFalseBackButton != null)
// //         {
// //             feedFalseBackButton.onClick.RemoveAllListeners(); // Hapus fungsi lama (BackToQuestion)
// //             feedFalseBackButton.onClick.AddListener(GoToCompletionScene);
// //         }

// //         if (feedCorrectNextButton != null)
// //         {
// //             feedCorrectNextButton.onClick.RemoveAllListeners();
// //             feedCorrectNextButton.onClick.AddListener(GoToCompletionScene);
// //         }
// //     }

// //     void LoadData()
// //     {
// //         if (questionJson != null)
// //             qList = JsonUtility.FromJson<QuestionList>(questionJson.text);
// //         if (wrongFeedbackJson != null)
// //             wList = JsonUtility.FromJson<WrongFeedbackList>(wrongFeedbackJson.text);
// //         if (correctFeedbackJson != null)
// //             cList = JsonUtility.FromJson<CorrectFeedbackList>(correctFeedbackJson.text);
// //     }

// //     // --- LOGIC: SCENE MANAGEMENT ---

// //     // TAMBAHAN: Fungsi untuk pindah ke Scene CompletionPanel
// //     public void GoToCompletionScene()
// //     {
// //         // Pastikan scene "CompletionPanel" sudah ada di Build Settings
// //         SceneManager.LoadScene(completionSceneName);
// //     }

// //     // --- LOGIC: QUESTION PANEL ---

// //     void ShowQuestion(int index)
// //     {
// //         currentQuestionIndex = index;
// //         currentAnswers.Clear();
        
// //         // Hapus semua anak di sentenceContainer dengan aman
// //         foreach (Transform child in sentenceContainer) 
// //         {
// //             Destroy(child.gameObject);
// //         }
// //         activeSentenceElements.Clear();
        
// //         if (qList == null || qList.questions == null || index >= qList.questions.Length) return;

// //         QuestionData data = qList.questions[index];

// //         // 1. Isi Data Statis
// //         descriptionText.text = data.descriptionText;
// //         if (data.imageSegment.Length > 0 && !string.IsNullOrEmpty(data.imageSegment[0]))
// //         {
// //             string rawPath = data.imageSegment[0]; 
// //             string loadPath = rawPath;

// //             if (rawPath.Contains("Resources/"))
// //             {
// //                 loadPath = rawPath.Substring(rawPath.IndexOf("Resources/") + "Resources/".Length);
// //             }

// //             int dotIndex = loadPath.LastIndexOf('.');
// //             if (dotIndex != -1)
// //             {
// //                 loadPath = loadPath.Substring(0, dotIndex);
// //             }

// //             Sprite loadedSprite = Resources.Load<Sprite>(loadPath);

// //             if (loadedSprite != null)
// //             {
// //                 questionImage.sprite = loadedSprite;
// //                 questionImage.gameObject.SetActive(true); 
// //                 questionImage.preserveAspect = true; 
// //             }
// //             else
// //             {
// //                 questionImage.gameObject.SetActive(false); 
// //             }
// //         }
// //         else
// //         {
// //             questionImage.gameObject.SetActive(false);
// //         }

// //         // 2. Bangun Kalimat (Sentence Structure)
// //         totalBlanks = 0;
// //         int slotIndexCounter = 0;

// //         foreach (string part in data.sentenceStructure)
// //         {
// //             if (part == "{blank}")
// //             {
// //                 // Instantiate dengan 'false' agar tidak mengikuti posisi World
// //                 GameObject slot = Instantiate(slotPrefab, sentenceContainer, false);

// //                 slot.SetActive(true); 
// //                 slot.transform.localScale = Vector3.one; 
                
// //                 Button btn = slot.GetComponent<Button>();
// //                 if(btn) btn.enabled = true;
// //                 Image img = slot.GetComponent<Image>();
// //                 if(img) img.enabled = true;
                
// //                 slot.transform.localScale = Vector3.one;
// //                 slot.transform.localPosition = Vector3.zero;
// //                 slot.transform.localRotation = Quaternion.identity;
                
// //                 int mySlotId = slotIndexCounter; 
                
// //                 TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
// //                 if (slotText != null)
// //                 {
// //                     slotText.text = "_________";
// //                     slotText.ForceMeshUpdate(); 
// //                 };
                
// //                 Button slotBtn = slot.GetComponent<Button>();
// //                 if (slotBtn != null && slotText != null)
// //                 {
// //                     slotBtn.onClick.AddListener(() => RemoveAnswerFromSlot(mySlotId, slotText));
// //                 }

// //                 activeSentenceElements.Add(slot);
// //                 totalBlanks++;
// //                 slotIndexCounter++;
// //             }
// //             else
// //             {
// //                 GameObject txt = Instantiate(textPrefab, sentenceContainer, false);
                
// //                 txt.SetActive(true);
// //                 txt.transform.localScale = Vector3.one;
// //                 txt.transform.localPosition = Vector3.zero;
// //                 txt.transform.localRotation = Quaternion.identity;

// //                 TMP_Text txtComp = txt.GetComponent<TMP_Text>();
// //                 if (txtComp != null)                
// //                 {
// //                     txtComp.text = part;
// //                     txtComp.ForceMeshUpdate(); 
// //                 };
                
// //                 activeSentenceElements.Add(txt);
// //             }
// //         }

// //         // 3. Setup Opsi Jawaban di Smart Grid
// //         if (optionsGrid != null)
// //         {
// //             optionsGrid.GenerateOptions(data.options, OnOptionSelected);
// //         }

// //         checkButton.interactable = false;
// //         checkButtonText.text = "CEK ULANG";

// //         StartCoroutine(ForceRebuildLayout());
// //     }

// //     IEnumerator ForceRebuildLayout()
// //     {
// //         yield return new WaitForEndOfFrame();

// //         if (sentenceContainer != null)
// //         {
// //             foreach(RectTransform child in sentenceContainer)
// //             {
// //                 LayoutRebuilder.ForceRebuildLayoutImmediate(child);
// //             }

// //             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
            
// //             // Matikan-Nyalakan untuk memancing "OnEnable"
// //             sentenceContainer.gameObject.SetActive(false);
// //             sentenceContainer.gameObject.SetActive(true);
// //         }
// //     }

// //     void OnOptionSelected(string answerText, GameObject btnObj)
// //     {
// //         for (int i = 0; i < totalBlanks; i++)
// //         {
// //             if (!currentAnswers.ContainsKey(i))
// //             {
// //                 currentAnswers.Add(i, answerText);
                
// //                 UpdateSlotVisual(i, answerText);
                
// //                 btnObj.SetActive(false); 
// //                 if (optionsGrid != null) optionsGrid.RecalculateLayout(); 
                
// //                 CheckCompletion();
// //                 return;
// //             }
// //         }
// //     }

// //     void RemoveAnswerFromSlot(int slotId, TMP_Text uiText)
// //     {
// //         if (currentAnswers.ContainsKey(slotId))
// //         {
// //             string returnedAnswer = currentAnswers[slotId];
// //             currentAnswers.Remove(slotId);
// //             uiText.text = "___"; 

// //             // Kembalikan tombol ke Grid
// //             if (optionsGrid != null)
// //             {
// //                 foreach(Transform child in optionsGrid.transform)
// //                 {
// //                     TMP_Text btnText = child.GetComponentInChildren<TMP_Text>();
// //                     if (!child.gameObject.activeSelf && btnText != null && btnText.text == returnedAnswer)
// //                     {
// //                         child.gameObject.SetActive(true);
// //                         optionsGrid.RecalculateLayout(); 
// //                         break;
// //                     }
// //                 }
// //             }
            
// //             LayoutRebuilder.ForceRebuildLayoutImmediate(uiText.transform.parent.GetComponent<RectTransform>());
// //             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());

// //             CheckCompletion();
// //         }
// //     }

// //     void UpdateSlotVisual(int slotId, string text)
// //     {
// //         int currentSlotCount = 0;
        
// //         foreach(var obj in activeSentenceElements)
// //         {
// //             Button btn = obj.GetComponent<Button>();
            
// //             if (btn != null) 
// //             {
// //                 if (currentSlotCount == slotId)
// //                 {
// //                     TMP_Text textComp = obj.GetComponentInChildren<TMP_Text>();
// //                     if (textComp != null)
// //                     {
// //                         textComp.text = text;
// //                     }
                    
// //                     LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
// //                     LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
                    
// //                     break; 
// //                 }
// //                 currentSlotCount++;
// //             }
// //         }
// //     }

// //     void CheckCompletion()
// //     {
// //         checkButton.interactable = (currentAnswers.Count == totalBlanks);
// //     }

// //     void OnCheckUlangClicked()
// //     {
// //         TogglePopup(true);
        
// //         string summary = "";
// //         QuestionData data = qList.questions[currentQuestionIndex];
// //         int slotCounter = 0;

// //         string colorHex = "#" + ColorUtility.ToHtmlStringRGB(summaryHighlightColor);
        
// //         foreach (string part in data.sentenceStructure)
// //         {
// //             if (part == "{blank}")
// //             {
// //                 if (currentAnswers.ContainsKey(slotCounter))
// //                 {
// //                     string answer = currentAnswers[slotCounter];
// //                     summary += $"<color={colorHex}><b>{answer}</b></color>";
// //                 }
// //                 slotCounter++;
// //             }
// //             else
// //             {
// //                 summary += part;
// //             }
// //         }
// //         confirmAnswerSummary.text = summary;
// //     }

// //     void TogglePopup(bool show)
// //     {
// //         confirmationPopup.SetActive(show);
// //     }

// //     // --- LOGIC: EVALUATION ---

// //     void EvaluateAnswer()
// //     {
// //         TogglePopup(false);
// //         QuestionData data = qList.questions[currentQuestionIndex];
        
// //         bool isCorrect = true;
        
// //         for (int i = 0; i < totalBlanks; i++)
// //         {
// //             if (!currentAnswers.ContainsKey(i)) 
// //             {
// //                 isCorrect = false;
// //                 break;
// //             }

// //             string userAnswer = currentAnswers[i].Trim();
// //             string keyAnswer = data.correctAnswers[i].Trim();

// //             if (!string.Equals(userAnswer, keyAnswer, System.StringComparison.OrdinalIgnoreCase))
// //             {
// //                 isCorrect = false;
// //                 break;
// //             }
// //         }

// //         if (isCorrect)
// //         {
// //             ShowCorrectFeedback();
// //         }
// //         else
// //         {
// //             ShowWrongFeedback();
// //         }
// //     }

// //     // --- LOGIC: FEEDBACK ---

// //     void ShowCorrectFeedback()
// //     {
// //         questionPanel.SetActive(false);
// //         feedCorrectPanel.SetActive(true);

// //         if (cList != null && cList.feedback_correct != null)
// //         {
// //             CorrectFeedbackData data = cList.feedback_correct.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
// //             if (data != null)
// //             {
// //                 correctMessageText.text = data.correctAnswers;
// //             }
// //         }
// //     }

// //     void ShowWrongFeedback()
// //     {
// //         questionPanel.SetActive(false);
// //         feedFalsePanel.SetActive(true);

// //         // Bersihkan konten lama
// //         foreach(Transform child in wrongContentContainer) Destroy(child.gameObject);

// //         if (wList != null && wList.feedback_wrong != null)
// //         {
// //             WrongFeedbackData data = wList.feedback_wrong.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
            
// //             if (data != null)
// //             {
// //                 wrongHeaderText.text = data.title;

// //                 foreach (var section in data.sections)
// //                 {
// //                     GameObject secObj = Instantiate(wrongSectionPrefab, wrongContentContainer);
                    
// //                     TMP_Text headerTxt = secObj.transform.Find("Header")?.GetComponent<TMP_Text>();
// //                     if(headerTxt) headerTxt.text = section.header;

// //                     TMP_Text contentTxt = secObj.transform.Find("Content")?.GetComponent<TMP_Text>();
// //                     if(contentTxt) contentTxt.text = section.content;

// //                     Image img = secObj.transform.Find("SectionImage")?.GetComponent<Image>();
// //                     if (img != null)
// //                     {
// //                         if (!string.IsNullOrEmpty(section.image_path))
// //                         {
// //                             img.gameObject.SetActive(true);
// //                         }
// //                         else
// //                         {
// //                             img.gameObject.SetActive(false);
// //                         }
// //                     }
// //                 }
// //             }
// //         }
// //     }
    
// //     // Fungsi ini bisa tetap ada jika ingin digunakan untuk keperluan lain
// //     public void BackToQuestion()
// //     {
// //         feedFalsePanel.SetActive(false);
// //         feedCorrectPanel.SetActive(false);
// //         questionPanel.SetActive(true);
// //     }
// // }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI; // Penting untuk LayoutRebuilder
// using TMPro;
// using System.Linq;
// using UnityEngine.SceneManagement; // Penting untuk pindah Scene

// public class QuizManager : MonoBehaviour
// {
//     [Header("Data Files")]
//     public TextAsset questionJson;
//     public TextAsset wrongFeedbackJson;
//     public TextAsset correctFeedbackJson;

//     [Header("Panels")]
//     public GameObject questionPanel;
//     public GameObject confirmationPopup;
//     public GameObject feedFalsePanel;
//     public GameObject feedCorrectPanel;

//     [Header("Question UI References")]
//     public TMP_Text descriptionText;
//     public Image questionImage; 
//     public Transform sentenceContainer; // Container untuk kalimat dengan {blank}
//     public GameObject textPrefab; // Prefab UI Text biasa
//     public GameObject slotPrefab; // Prefab UI Slot kosong (Button/Image)
//     public SmartGridSystem optionsGrid;
//     public Button checkButton; 
//     public TMP_Text checkButtonText;

//     [Header("Confirmation UI References")]
//     public Color summaryHighlightColor = Color.green;
//     public TMP_Text confirmAnswerSummary;
//     public Button confirmSubmitBtn;
//     public Button confirmBackBtn;

//     [Header("Feedback UI References")]
//     public TMP_Text correctMessageText;
//     public TMP_Text wrongHeaderText;       // Judul Besar Feedback (jika ada di JSON root)
//     public Transform wrongContentContainer; // Tempat spawn section feedback

//     // --- SYSTEM MAPPING PREFAB BARU ---
//     [System.Serializable]
//     public struct SectionPrefabMapping
//     {
//         public string typeName; // Isi di inspector: "HeaderDescImage", "DetailList", dll (Sesuai JSON)
//         public GameObject prefab;
//     }

//     [Header("Feedback Dynamic Prefabs")]
//     public List<SectionPrefabMapping> feedbackPrefabs; // Assign prefab section disini
//     public GameObject listItemTextPrefab; // Prefab text kecil untuk poin-poin list

//     [Header("Scene Navigation")]
//     public Button feedFalseBackButton; // Tombol "KEMBALI" di panel salah
//     public Button feedCorrectNextButton; // Tombol "LANJUTKAN" di panel benar
//     public string completionSceneName = "CompletionQuiz"; // Nama scene tujuan

//     // State Variables
//     // PERUBAHAN NAMA CLASS: Menggunakan QuizApp... untuk menghindari konflik
//     private QuizAppQuestionList qList;
//     private QuizAppWrongFeedbackList wList;
//     private QuizAppCorrectFeedbackList cList;
    
//     private int currentQuestionIndex = 0;
//     private Dictionary<int, string> currentAnswers = new Dictionary<int, string>(); 
//     private List<GameObject> activeSentenceElements = new List<GameObject>();
//     private int totalBlanks = 0;

//     void Start()
//     {
//         LoadData();
//         ShowQuestion(0);
        
//         // Setup tombol UI utama
//         checkButton.onClick.AddListener(OnCheckUlangClicked);
//         confirmBackBtn.onClick.AddListener(() => TogglePopup(false));
//         confirmSubmitBtn.onClick.AddListener(EvaluateAnswer);

//         // Setup tombol Navigasi ke Scene Completion
//         if (feedFalseBackButton != null)
//         {
//             feedFalseBackButton.onClick.RemoveAllListeners(); 
//             feedFalseBackButton.onClick.AddListener(GoToCompletionScene);
//         }

//         if (feedCorrectNextButton != null)
//         {
//             feedCorrectNextButton.onClick.RemoveAllListeners();
//             feedCorrectNextButton.onClick.AddListener(GoToCompletionScene);
//         }
//     }

//     void LoadData()
//     {
//         // Menggunakan nama class baru saat Deserialize
//         if (questionJson != null)
//             qList = JsonUtility.FromJson<QuizAppQuestionList>(questionJson.text);
        
//         if (wrongFeedbackJson != null)
//             wList = JsonUtility.FromJson<QuizAppWrongFeedbackList>(wrongFeedbackJson.text); 
        
//         if (correctFeedbackJson != null)
//             cList = JsonUtility.FromJson<QuizAppCorrectFeedbackList>(correctFeedbackJson.text);
//     }

//     // --- LOGIC: SCENE MANAGEMENT ---

//     public void GoToCompletionScene()
//     {
//         SceneManager.LoadScene(completionSceneName);
//     }

//     // --- LOGIC: QUESTION PANEL ---

//     void ShowQuestion(int index)
//     {
//         currentQuestionIndex = index;
//         currentAnswers.Clear();
        
//         foreach (Transform child in sentenceContainer) 
//         {
//             Destroy(child.gameObject);
//         }
//         activeSentenceElements.Clear();
        
//         if (qList == null || qList.questions == null || index >= qList.questions.Length) return;

//         QuizAppQuestionData data = qList.questions[index]; // Update Nama Class

//         // 1. Isi Data Statis
//         descriptionText.text = data.descriptionText;
        
//         // Load Image untuk Soal
//         if (data.imageSegment.Length > 0 && !string.IsNullOrEmpty(data.imageSegment[0]))
//         {
//             string cleanPath = CleanResourcePath(data.imageSegment[0]);
//             Sprite loadedSprite = Resources.Load<Sprite>(cleanPath);

//             if (loadedSprite != null)
//             {
//                 questionImage.sprite = loadedSprite;
//                 questionImage.gameObject.SetActive(true); 
//                 questionImage.preserveAspect = true; 
//             }
//             else
//             {
//                 questionImage.gameObject.SetActive(false); 
//             }
//         }
//         else
//         {
//             questionImage.gameObject.SetActive(false);
//         }

//         // 2. Bangun Kalimat (Sentence Structure)
//         totalBlanks = 0;
//         int slotIndexCounter = 0;

//         foreach (string part in data.sentenceStructure)
//         {
//             if (part == "{blank}")
//             {
//                 GameObject slot = Instantiate(slotPrefab, sentenceContainer, false);
//                 slot.SetActive(true); 
                
//                 int mySlotId = slotIndexCounter; 
                
//                 TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
//                 if (slotText != null)
//                 {
//                     slotText.text = "_________";
//                     slotText.ForceMeshUpdate(); 
//                 };
                
//                 Button slotBtn = slot.GetComponent<Button>();
//                 if (slotBtn != null && slotText != null)
//                 {
//                     slotBtn.onClick.AddListener(() => RemoveAnswerFromSlot(mySlotId, slotText));
//                 }

//                 activeSentenceElements.Add(slot);
//                 totalBlanks++;
//                 slotIndexCounter++;
//             }
//             else
//             {
//                 GameObject txt = Instantiate(textPrefab, sentenceContainer, false);
//                 txt.SetActive(true);
                
//                 TMP_Text txtComp = txt.GetComponent<TMP_Text>();
//                 if (txtComp != null)                
//                 {
//                     txtComp.text = part;
//                     txtComp.ForceMeshUpdate(); 
//                 };
                
//                 activeSentenceElements.Add(txt);
//             }
//         }

//         // 3. Setup Opsi Jawaban di Smart Grid
//         if (optionsGrid != null)
//         {
//             optionsGrid.GenerateOptions(data.options, OnOptionSelected);
//         }

//         checkButton.interactable = false;
//         checkButtonText.text = "CEK ULANG";

//         StartCoroutine(ForceRebuildLayout());
//     }

//     IEnumerator ForceRebuildLayout()
//     {
//         yield return new WaitForEndOfFrame();
//         if (sentenceContainer != null)
//         {
//             foreach(RectTransform child in sentenceContainer)
//                 LayoutRebuilder.ForceRebuildLayoutImmediate(child);

//             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
            
//             sentenceContainer.gameObject.SetActive(false);
//             sentenceContainer.gameObject.SetActive(true);
//         }
//     }

//     string CleanResourcePath(string rawPath)
//     {
//         string loadPath = rawPath;
//         if (rawPath.Contains("Resources/"))
//         {
//             loadPath = rawPath.Substring(rawPath.IndexOf("Resources/") + "Resources/".Length);
//         }
//         int dotIndex = loadPath.LastIndexOf('.');
//         if (dotIndex != -1)
//         {
//             loadPath = loadPath.Substring(0, dotIndex);
//         }
//         return loadPath;
//     }

//     // --- LOGIC: ANSWER HANDLING ---

//     void OnOptionSelected(string answerText, GameObject btnObj)
//     {
//         for (int i = 0; i < totalBlanks; i++)
//         {
//             if (!currentAnswers.ContainsKey(i))
//             {
//                 currentAnswers.Add(i, answerText);
//                 UpdateSlotVisual(i, answerText);
//                 btnObj.SetActive(false); 
//                 if (optionsGrid != null) optionsGrid.RecalculateLayout(); 
//                 CheckCompletion();
//                 return;
//             }
//         }
//     }

//     void RemoveAnswerFromSlot(int slotId, TMP_Text uiText)
//     {
//         if (currentAnswers.ContainsKey(slotId))
//         {
//             string returnedAnswer = currentAnswers[slotId];
//             currentAnswers.Remove(slotId);
//             uiText.text = "___"; 

//             if (optionsGrid != null)
//             {
//                 foreach(Transform child in optionsGrid.transform)
//                 {
//                     TMP_Text btnText = child.GetComponentInChildren<TMP_Text>();
//                     if (!child.gameObject.activeSelf && btnText != null && btnText.text == returnedAnswer)
//                     {
//                         child.gameObject.SetActive(true);
//                         optionsGrid.RecalculateLayout(); 
//                         break;
//                     }
//                 }
//             }
//             LayoutRebuilder.ForceRebuildLayoutImmediate(uiText.transform.parent.GetComponent<RectTransform>());
//             LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
//             CheckCompletion();
//         }
//     }

//     void UpdateSlotVisual(int slotId, string text)
//     {
//         int currentSlotCount = 0;
//         foreach(var obj in activeSentenceElements)
//         {
//             Button btn = obj.GetComponent<Button>();
//             if (btn != null) 
//             {
//                 if (currentSlotCount == slotId)
//                 {
//                     TMP_Text textComp = obj.GetComponentInChildren<TMP_Text>();
//                     if (textComp != null) textComp.text = text;
//                     LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
//                     LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
//                     break; 
//                 }
//                 currentSlotCount++;
//             }
//         }
//     }

//     void CheckCompletion()
//     {
//         checkButton.interactable = (currentAnswers.Count == totalBlanks);
//     }

//     void OnCheckUlangClicked()
//     {
//         TogglePopup(true);
//         string summary = "";
//         QuizAppQuestionData data = qList.questions[currentQuestionIndex]; // Update Nama Class
//         int slotCounter = 0;
//         string colorHex = "#" + ColorUtility.ToHtmlStringRGB(summaryHighlightColor);
        
//         foreach (string part in data.sentenceStructure)
//         {
//             if (part == "{blank}")
//             {
//                 if (currentAnswers.ContainsKey(slotCounter))
//                 {
//                     string answer = currentAnswers[slotCounter];
//                     summary += $"<color={colorHex}><b>{answer}</b></color>";
//                 }
//                 slotCounter++;
//             }
//             else
//             {
//                 summary += part;
//             }
//         }
//         confirmAnswerSummary.text = summary;
//     }

//     void TogglePopup(bool show)
//     {
//         confirmationPopup.SetActive(show);
//     }

//     // --- LOGIC: EVALUATION ---

//     void EvaluateAnswer()
//     {
//         TogglePopup(false);
//         QuizAppQuestionData data = qList.questions[currentQuestionIndex]; // Update Nama Class
        
//         bool isCorrect = true;
        
//         for (int i = 0; i < totalBlanks; i++)
//         {
//             if (!currentAnswers.ContainsKey(i)) { isCorrect = false; break; }

//             string userAnswer = currentAnswers[i].Trim();
//             string keyAnswer = data.correctAnswers[i].Trim();

//             if (!string.Equals(userAnswer, keyAnswer, System.StringComparison.OrdinalIgnoreCase))
//             {
//                 isCorrect = false;
//                 break;
//             }
//         }

//         if (isCorrect) ShowCorrectFeedback();
//         else ShowWrongFeedback();
//     }

//     // --- LOGIC: FEEDBACK ---

//     void ShowCorrectFeedback()
//     {
//         questionPanel.SetActive(false);
//         feedCorrectPanel.SetActive(true);

//         if (cList != null && cList.feedback_correct != null)
//         {
//             QuizAppCorrectFeedbackData data = cList.feedback_correct.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id); // Update Nama Class
//             if (data != null)
//             {
//                 correctMessageText.text = data.correctAnswers;
//             }
//         }
//     }

//     void ShowWrongFeedback()
//     {
//         questionPanel.SetActive(false);
//         feedFalsePanel.SetActive(true);

//         foreach (Transform child in wrongContentContainer) Destroy(child.gameObject);

//         if (wList != null && wList.feedback_wrong != null)
//         {
//             int currentId = qList.questions[currentQuestionIndex].id;
            
//             QuizAppWrongFeedbackData data = wList.feedback_wrong.FirstOrDefault(x => x.id == currentId); // Update Nama Class

//             if (data != null)
//             {
//                 if (wrongHeaderText != null) wrongHeaderText.text = "Pembahasan"; 

//                 foreach (var section in data.sections)
//                 {
//                     GameObject prefabToUse = GetPrefabByType(section.section_type);

//                     if (prefabToUse != null)
//                     {
//                         GameObject secObj = Instantiate(prefabToUse, wrongContentContainer);
//                         SetupSectionData(secObj, section);
//                     }
//                     else
//                     {
//                         Debug.LogWarning($"Prefab untuk tipe '{section.section_type}' belum di-assign di Inspector!");
//                     }
//                 }
//             }
//         }
//     }

//     GameObject GetPrefabByType(string typeName)
//     {
//         foreach(var map in feedbackPrefabs)
//         {
//             if (map.typeName == typeName) return map.prefab;
//         }
//         return null;
//     }

//     void SetupSectionData(GameObject obj, QuizAppWrongFeedbackSection data) // Update Nama Class
//     {
//         SetText(obj, "Header", data.header);

//         string contentText = !string.IsNullOrEmpty(data.content) ? data.content : data.Description;
//         SetText(obj, "Content", contentText);

//         if (!string.IsNullOrEmpty(data.color_bg_image))
//         {
//             string hexColor = data.color_bg_image;
//             if (!hexColor.StartsWith("#")) hexColor = "#" + hexColor;

//             if (ColorUtility.TryParseHtmlString(hexColor, out Color newCol))
//             {
//                 Image bgImage = obj.GetComponent<Image>();
//                 if (bgImage == null) 
//                 {
//                     Transform bgTrans = obj.transform.Find("Background");
//                     if (bgTrans) bgImage = bgTrans.GetComponent<Image>();
//                 }

//                 if (bgImage != null) bgImage.color = newCol;
//             }
//         }

//         if (!string.IsNullOrEmpty(data.image_path))
//         {
//             Transform imgTrans = obj.transform.Find("SectionImage");
//             if (imgTrans != null)
//             {
//                 Image uiImg = imgTrans.GetComponent<Image>();
//                 if (uiImg != null)
//                 {
//                     string cleanPath = CleanResourcePath(data.image_path);
//                     Sprite spr = Resources.Load<Sprite>(cleanPath);

//                     if (spr != null)
//                     {
//                         uiImg.sprite = spr;
//                         uiImg.gameObject.SetActive(true);
//                         uiImg.preserveAspect = true;
//                     }
//                     else
//                     {
//                         uiImg.gameObject.SetActive(false);
//                     }
//                 }
//             }
//         }

//         if (data.list != null && data.list.Length > 0)
//         {
//             Transform listContainer = obj.transform.Find("ListContainer");
//             if (listContainer != null && listItemTextPrefab != null)
//             {
//                 foreach (string point in data.list)
//                 {
//                     GameObject itemObj = Instantiate(listItemTextPrefab, listContainer);
                    
//                     TMP_Text txt = itemObj.GetComponent<TMP_Text>();
//                     if (txt == null) txt = itemObj.GetComponentInChildren<TMP_Text>();
                    
//                     if (txt != null) 
//                     {
//                         txt.text = "• " + point; 
//                     }
//                 }
//             }
//         }
//     }

//     void SetText(GameObject parent, string childName, string value)
//     {
//         if (string.IsNullOrEmpty(value)) return;
//         Transform t = parent.transform.Find(childName);
//         if (t != null)
//         {
//             TMP_Text tmp = t.GetComponent<TMP_Text>();
//             if (tmp) tmp.text = value;
//         }
//     }

//     public void BackToQuestion()
//     {
//         feedFalsePanel.SetActive(false);
//         feedCorrectPanel.SetActive(false);
//         questionPanel.SetActive(true);
//     }
// }

// // =========================================================
// // CLASS SERIALIZATION (RENAMED TO AVOID AMBIGUITY)
// // =========================================================

// // "QuizApp" prefix ditambahkan untuk memastikan nama Class UNIK

// [System.Serializable]
// public class QuizAppQuestionList
// {
//     public QuizAppQuestionData[] questions;
// }

// [System.Serializable]
// public class QuizAppQuestionData
// {
//     public int id;
//     public string descriptionText;
//     public string[] imageSegment;
//     public string[] sentenceStructure;
//     public string[] options;
//     public string[] correctAnswers;
// }

// [System.Serializable]
// public class QuizAppCorrectFeedbackList
// {
//     public QuizAppCorrectFeedbackData[] feedback_correct;
// }

// [System.Serializable]
// public class QuizAppCorrectFeedbackData
// {
//     public int id;
//     public string correctAnswers;
// }

// [System.Serializable]
// public class QuizAppWrongFeedbackList
// {
//     public List<QuizAppWrongFeedbackData> feedback_wrong;
// }

// [System.Serializable]
// public class QuizAppWrongFeedbackData
// {
//     public int id;
//     public string title; 
//     public List<QuizAppWrongFeedbackSection> sections;
// }

// [System.Serializable]
// public class QuizAppWrongFeedbackSection
// {
//     public string section_type; 
    
//     // Properti Text
//     public string header;
//     public string content;      
//     public string Description;  
    
//     // Properti Visual
//     public string image_path;
//     public string color_bg_image; 
    
//     // Properti List
//     public string[] list;       
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Penting untuk LayoutRebuilder
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement; // Penting untuk pindah Scene

public class QuizManager : MonoBehaviour
{
    [Header("Data Files")]
    public TextAsset questionJson;
    public TextAsset wrongFeedbackJson;
    public TextAsset correctFeedbackJson;

    [Header("Panels")]
    public GameObject questionPanel;
    public GameObject confirmationPopup;
    public GameObject feedFalsePanel;
    public GameObject feedCorrectPanel;

    [Header("Question UI References")]
    public TMP_Text descriptionText;
    public Image questionImage; 
    public Transform sentenceContainer; // Container untuk kalimat dengan {blank}
    public GameObject textPrefab; // Prefab UI Text biasa
    public GameObject slotPrefab; // Prefab UI Slot kosong (Button/Image)
    public SmartGridSystem optionsGrid;
    public Button checkButton; 
    public TMP_Text checkButtonText;

    [Header("Confirmation UI References")]
    public Color summaryHighlightColor = Color.green;
    public TMP_Text confirmAnswerSummary;
    public Button confirmSubmitBtn;
    public Button confirmBackBtn;

    [Header("Feedback UI References")]
    public TMP_Text correctMessageText;
    public TMP_Text wrongHeaderText;       // Judul Besar Feedback (jika ada di JSON root)
    public Transform wrongContentContainer; // Tempat spawn section feedback

    // --- SYSTEM MAPPING PREFAB BARU ---
    [System.Serializable]
    public struct SectionPrefabMapping
    {
        public string typeName; // Isi di inspector: "HeaderDescImage", "DetailList", dll (Sesuai JSON)
        public GameObject prefab;
    }

    [Header("Feedback Dynamic Prefabs")]
    public List<SectionPrefabMapping> feedbackPrefabs; // Assign prefab section disini
    public GameObject listItemTextPrefab; // Prefab text kecil untuk poin-poin list

    [Header("Scene Navigation")]
    public Button feedFalseBackButton; // Tombol "KEMBALI" di panel salah
    public Button feedCorrectNextButton; // Tombol "LANJUTKAN" di panel benar
    public string completionSceneName = "CompletionQuiz"; // Nama scene tujuan

    // State Variables
    private QuizAppQuestionList qList;
    private QuizAppWrongFeedbackList wList;
    private QuizAppCorrectFeedbackList cList;
    
    private int currentQuestionIndex = 0;
    private Dictionary<int, string> currentAnswers = new Dictionary<int, string>(); 
    private List<GameObject> activeSentenceElements = new List<GameObject>();
    private int totalBlanks = 0;

    void Start()
    {
        LoadData();
        ShowQuestion(0);
        
        // Setup tombol UI utama
        checkButton.onClick.AddListener(OnCheckUlangClicked);
        confirmBackBtn.onClick.AddListener(() => TogglePopup(false));
        confirmSubmitBtn.onClick.AddListener(EvaluateAnswer);

        // Setup tombol Navigasi ke Scene Completion
        if (feedFalseBackButton != null)
        {
            feedFalseBackButton.onClick.RemoveAllListeners(); 
            feedFalseBackButton.onClick.AddListener(GoToCompletionScene);
        }

        if (feedCorrectNextButton != null)
        {
            feedCorrectNextButton.onClick.RemoveAllListeners();
            feedCorrectNextButton.onClick.AddListener(GoToCompletionScene);
        }
    }

    void LoadData()
    {
        if (questionJson != null)
            qList = JsonUtility.FromJson<QuizAppQuestionList>(questionJson.text);
        
        if (wrongFeedbackJson != null)
            wList = JsonUtility.FromJson<QuizAppWrongFeedbackList>(wrongFeedbackJson.text); 
        
        if (correctFeedbackJson != null)
            cList = JsonUtility.FromJson<QuizAppCorrectFeedbackList>(correctFeedbackJson.text);
    }

    // --- LOGIC: SCENE MANAGEMENT ---

    public void GoToCompletionScene()
    {
        SceneManager.LoadScene(completionSceneName);
    }

    // --- LOGIC: QUESTION PANEL ---

    void ShowQuestion(int index)
    {
        currentQuestionIndex = index;
        currentAnswers.Clear();
        
        foreach (Transform child in sentenceContainer) 
        {
            Destroy(child.gameObject);
        }
        activeSentenceElements.Clear();
        
        if (qList == null || qList.questions == null || index >= qList.questions.Length) return;

        QuizAppQuestionData data = qList.questions[index];

        // 1. Isi Data Statis
        descriptionText.text = data.descriptionText;
        
        // Load Image untuk Soal
        if (data.imageSegment.Length > 0 && !string.IsNullOrEmpty(data.imageSegment[0]))
        {
            string cleanPath = CleanResourcePath(data.imageSegment[0]);
            Sprite loadedSprite = Resources.Load<Sprite>(cleanPath);

            if (loadedSprite != null)
            {
                questionImage.sprite = loadedSprite;
                questionImage.gameObject.SetActive(true); 
                questionImage.preserveAspect = true; 
            }
            else
            {
                questionImage.gameObject.SetActive(false); 
            }
        }
        else
        {
            questionImage.gameObject.SetActive(false);
        }

        // 2. Bangun Kalimat (Sentence Structure)
        totalBlanks = 0;
        int slotIndexCounter = 0;

        foreach (string part in data.sentenceStructure)
        {
            if (part == "{blank}")
            {
                GameObject slot = Instantiate(slotPrefab, sentenceContainer, false);
                slot.SetActive(true); 
                
                int mySlotId = slotIndexCounter; 
                
                TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
                if (slotText != null)
                {
                    slotText.text = "_________";
                    slotText.ForceMeshUpdate(); 
                };
                
                Button slotBtn = slot.GetComponent<Button>();
                if (slotBtn != null && slotText != null)
                {
                    slotBtn.onClick.AddListener(() => RemoveAnswerFromSlot(mySlotId, slotText));
                }

                activeSentenceElements.Add(slot);
                totalBlanks++;
                slotIndexCounter++;
            }
            else
            {
                GameObject txt = Instantiate(textPrefab, sentenceContainer, false);
                txt.SetActive(true);
                
                TMP_Text txtComp = txt.GetComponent<TMP_Text>();
                if (txtComp != null)                
                {
                    txtComp.text = part;
                    txtComp.ForceMeshUpdate(); 
                };
                
                activeSentenceElements.Add(txt);
            }
        }

        if (optionsGrid != null)
        {
            optionsGrid.GenerateOptions(data.options, OnOptionSelected);
        }

        checkButton.interactable = false;
        checkButtonText.text = "CEK ULANG";

        StartCoroutine(ForceRebuildLayout());
    }

    IEnumerator ForceRebuildLayout()
    {
        yield return new WaitForEndOfFrame();
        if (sentenceContainer != null)
        {
            foreach(RectTransform child in sentenceContainer)
                LayoutRebuilder.ForceRebuildLayoutImmediate(child);

            LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
            
            sentenceContainer.gameObject.SetActive(false);
            sentenceContainer.gameObject.SetActive(true);
        }
    }

    string CleanResourcePath(string rawPath)
    {
        string loadPath = rawPath;
        if (rawPath.Contains("Resources/"))
        {
            loadPath = rawPath.Substring(rawPath.IndexOf("Resources/") + "Resources/".Length);
        }
        int dotIndex = loadPath.LastIndexOf('.');
        if (dotIndex != -1)
        {
            loadPath = loadPath.Substring(0, dotIndex);
        }
        return loadPath;
    }

    // --- LOGIC: ANSWER HANDLING ---

    void OnOptionSelected(string answerText, GameObject btnObj)
    {
        for (int i = 0; i < totalBlanks; i++)
        {
            if (!currentAnswers.ContainsKey(i))
            {
                currentAnswers.Add(i, answerText);
                UpdateSlotVisual(i, answerText);
                btnObj.SetActive(false); 
                if (optionsGrid != null) optionsGrid.RecalculateLayout(); 
                CheckCompletion();
                return;
            }
        }
    }

    void RemoveAnswerFromSlot(int slotId, TMP_Text uiText)
    {
        if (currentAnswers.ContainsKey(slotId))
        {
            string returnedAnswer = currentAnswers[slotId];
            currentAnswers.Remove(slotId);
            uiText.text = "___"; 

            if (optionsGrid != null)
            {
                foreach(Transform child in optionsGrid.transform)
                {
                    TMP_Text btnText = child.GetComponentInChildren<TMP_Text>();
                    if (!child.gameObject.activeSelf && btnText != null && btnText.text == returnedAnswer)
                    {
                        child.gameObject.SetActive(true);
                        optionsGrid.RecalculateLayout(); 
                        break;
                    }
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(uiText.transform.parent.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
            CheckCompletion();
        }
    }

    void UpdateSlotVisual(int slotId, string text)
    {
        int currentSlotCount = 0;
        foreach(var obj in activeSentenceElements)
        {
            Button btn = obj.GetComponent<Button>();
            if (btn != null) 
            {
                if (currentSlotCount == slotId)
                {
                    TMP_Text textComp = obj.GetComponentInChildren<TMP_Text>();
                    if (textComp != null) textComp.text = text;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(obj.GetComponent<RectTransform>());
                    LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceContainer.GetComponent<RectTransform>());
                    break; 
                }
                currentSlotCount++;
            }
        }
    }

    void CheckCompletion()
    {
        checkButton.interactable = (currentAnswers.Count == totalBlanks);
    }

    void OnCheckUlangClicked()
    {
        TogglePopup(true);
        string summary = "";
        QuizAppQuestionData data = qList.questions[currentQuestionIndex];
        int slotCounter = 0;
        string colorHex = "#" + ColorUtility.ToHtmlStringRGB(summaryHighlightColor);
        
        foreach (string part in data.sentenceStructure)
        {
            if (part == "{blank}")
            {
                if (currentAnswers.ContainsKey(slotCounter))
                {
                    string answer = currentAnswers[slotCounter];
                    summary += $"<color={colorHex}><b>{answer}</b></color>";
                }
                slotCounter++;
            }
            else
            {
                summary += part;
            }
        }
        confirmAnswerSummary.text = summary;
    }

    void TogglePopup(bool show)
    {
        confirmationPopup.SetActive(show);
    }

    // --- LOGIC: EVALUATION ---

    void EvaluateAnswer()
    {
        TogglePopup(false);
        QuizAppQuestionData data = qList.questions[currentQuestionIndex];
        
        bool isCorrect = true;
        
        for (int i = 0; i < totalBlanks; i++)
        {
            if (!currentAnswers.ContainsKey(i)) { isCorrect = false; break; }

            string userAnswer = currentAnswers[i].Trim();
            string keyAnswer = data.correctAnswers[i].Trim();

            if (!string.Equals(userAnswer, keyAnswer, System.StringComparison.OrdinalIgnoreCase))
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect) ShowCorrectFeedback();
        else ShowWrongFeedback();
    }

    // --- LOGIC: FEEDBACK (CORRECT) ---

    void ShowCorrectFeedback()
    {
        questionPanel.SetActive(false);
        feedCorrectPanel.SetActive(true);

        if (cList != null && cList.feedback_correct != null)
        {
            QuizAppCorrectFeedbackData data = cList.feedback_correct.FirstOrDefault(x => x.id == qList.questions[currentQuestionIndex].id);
            if (data != null)
            {
                correctMessageText.text = data.correctAnswers;
            }
        }
    }

    // --- LOGIC: FEEDBACK (WRONG - DIPERBAIKI) ---

    void ShowWrongFeedback()
    {
        questionPanel.SetActive(false);
        feedFalsePanel.SetActive(true);

        foreach (Transform child in wrongContentContainer) Destroy(child.gameObject);

        if (wList != null && wList.feedback_wrong != null)
        {
            int currentId = qList.questions[currentQuestionIndex].id;
            QuizAppWrongFeedbackData data = wList.feedback_wrong.FirstOrDefault(x => x.id == currentId);

            if (data != null)
            {
                if (wrongHeaderText != null) wrongHeaderText.text = "JAWABAN SALAH"; 

                foreach (var section in data.sections)
                {
                    GameObject prefabToUse = GetPrefabByType(section.section_type);

                    if (prefabToUse != null)
                    {
                        GameObject secObj = Instantiate(prefabToUse, wrongContentContainer);
                        
                        // PERBAIKAN 1: Pastikan object root AKTIF setelah instantiate
                        // Ini mengatasi masalah jika Prefab aslinya di-uncheck (inactive)
                        secObj.SetActive(true); 

                        SetupSectionData(secObj, section);
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab untuk tipe '{section.section_type}' belum di-assign di Inspector!");
                    }
                }
                
                // PERBAIKAN 2: Rebuild layout setelah spawn agar posisi rapi
                StartCoroutine(ForceRebuildFeedbackLayout());
            }
        }
    }
    
    // Coroutine khusus untuk rebuild layout feedback
    IEnumerator ForceRebuildFeedbackLayout()
    {
        yield return new WaitForEndOfFrame();
        if (wrongContentContainer != null)
        {
            foreach (RectTransform child in wrongContentContainer)
                LayoutRebuilder.ForceRebuildLayoutImmediate(child);

            LayoutRebuilder.ForceRebuildLayoutImmediate(wrongContentContainer.GetComponent<RectTransform>());
        }
    }

    GameObject GetPrefabByType(string typeName)
    {
        foreach(var map in feedbackPrefabs)
        {
            if (map.typeName == typeName) return map.prefab;
        }
        return null;
    }

    // --- SETUP DATA KE PREFAB (DIPERBAIKI) ---

    void SetupSectionData(GameObject obj, QuizAppWrongFeedbackSection data)
    {
        // A. Set Header (Gunakan fungsi SetTextAndToggle)
        SetTextAndToggle(obj, "Header", data.header);

        // B. Set Content / Description
        string contentText = !string.IsNullOrEmpty(data.content) ? data.content : data.Description;
        SetTextAndToggle(obj, "Content", contentText);

        // C. Set Background Color
        if (!string.IsNullOrEmpty(data.color_bg_image))
        {
            string hexColor = data.color_bg_image;
            if (!hexColor.StartsWith("#")) hexColor = "#" + hexColor;

            if (ColorUtility.TryParseHtmlString(hexColor, out Color newCol))
            {
                Image bgImage = obj.GetComponent<Image>();
                if (bgImage == null) 
                {
                    Transform bgTrans = obj.transform.Find("Background");
                    if (bgTrans) bgImage = bgTrans.GetComponent<Image>();
                }
                if (bgImage != null) bgImage.color = newCol;
            }
        }

        // D. Set Image (Logika Toggle Lebih Ketat)
        Transform imgTrans = obj.transform.Find("SectionImage");
        if (imgTrans != null)
        {
            if (!string.IsNullOrEmpty(data.image_path))
            {
                string cleanPath = CleanResourcePath(data.image_path);
                Sprite spr = Resources.Load<Sprite>(cleanPath);

                if (spr != null)
                {
                    Image uiImg = imgTrans.GetComponent<Image>();
                    if (uiImg != null)
                    {
                        uiImg.sprite = spr;
                        uiImg.preserveAspect = true;
                    }
                    // Jika path ada & sprite ketemu, NYALAKAN object
                    imgTrans.gameObject.SetActive(true);
                }
                else
                {
                    // Sprite tidak ketemu
                    imgTrans.gameObject.SetActive(false);
                }
            }
            else
            {
                // Jika path kosong di JSON, MATIKAN object agar tidak ada kotak putih
                imgTrans.gameObject.SetActive(false);
            }
        }

        // E. Set List Items
        Transform listContainer = obj.transform.Find("ListContainer");
        if (listContainer != null)
        {
            if (data.list != null && data.list.Length > 0)
            {
                listContainer.gameObject.SetActive(true); // Nyalakan container list
                
                // Pastikan prefab list item ada
                if (listItemTextPrefab != null)
                {
                    foreach (string point in data.list)
                    {
                        GameObject itemObj = Instantiate(listItemTextPrefab, listContainer);
                        itemObj.SetActive(true); // Pastikan item aktif
                        
                        TMP_Text txt = itemObj.GetComponent<TMP_Text>();
                        if (txt == null) txt = itemObj.GetComponentInChildren<TMP_Text>();
                        
                        if (txt != null) 
                        {
                            txt.text = "• " + point; 
                        }
                    }
                }
            }
            else
            {
                // Jika list kosong, matikan container agar rapi
                listContainer.gameObject.SetActive(false);
            }
        }
    }

    // Fungsi helper baru: Set text dan atur nyala/mati object berdasarkan isi text
    void SetTextAndToggle(GameObject parent, string childName, string value)
    {
        Transform t = parent.transform.Find(childName);
        if (t != null)
        {
            bool hasContent = !string.IsNullOrEmpty(value);
            t.gameObject.SetActive(hasContent); // Nyalakan jika ada isi, matikan jika kosong

            if (hasContent)
            {
                TMP_Text tmp = t.GetComponent<TMP_Text>();
                if (tmp) tmp.text = value;
            }
        }
    }

    public void BackToQuestion()
    {
        feedFalsePanel.SetActive(false);
        feedCorrectPanel.SetActive(false);
        questionPanel.SetActive(true);
    }
}

// =========================================================
// CLASS SERIALIZATION
// =========================================================

[System.Serializable]
public class QuizAppQuestionList
{
    public QuizAppQuestionData[] questions;
}

[System.Serializable]
public class QuizAppQuestionData
{
    public int id;
    public string descriptionText;
    public string[] imageSegment;
    public string[] sentenceStructure;
    public string[] options;
    public string[] correctAnswers;
}

[System.Serializable]
public class QuizAppCorrectFeedbackList
{
    public QuizAppCorrectFeedbackData[] feedback_correct;
}

[System.Serializable]
public class QuizAppCorrectFeedbackData
{
    public int id;
    public string correctAnswers;
}

[System.Serializable]
public class QuizAppWrongFeedbackList
{
    public List<QuizAppWrongFeedbackData> feedback_wrong;
}

[System.Serializable]
public class QuizAppWrongFeedbackData
{
    public int id;
    public string title; 
    public List<QuizAppWrongFeedbackSection> sections;
}

[System.Serializable]
public class QuizAppWrongFeedbackSection
{
    public string section_type; 
    
    // Properti Text
    public string header;
    public string content;      
    public string Description;  
    
    // Properti Visual
    public string image_path;
    public string color_bg_image; 
    
    // Properti List
    public string[] list;       
}

