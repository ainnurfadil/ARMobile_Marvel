// // // using UnityEngine;
// // // using UnityEngine.UI;
// // // using UnityEngine.SceneManagement;
// // // using System.Collections.Generic;
// // // using System.Linq; // Added for sorting

// // // public class EBookViewer : MonoBehaviour
// // // {
// // //     [Header("Content Configuration")]
// // //     [Tooltip("Name of the folder inside 'Assets/Resources/'. Example: 'Chapter1'")]
// // //     public string resourceFolderPath = ""; 
    
// // //     [Tooltip("Check this if you want to verify loaded pages in Inspector")]
// // //     public List<Sprite> pageSprites = new List<Sprite>(); 
// // //     public string menuSceneName = "MainMenu";

// // //     [Header("UI References")]
// // //     public Image pageDisplay;          // The Image component inside the Content
// // //     public ScrollRect scrollRect;      // The ScrollRect component
// // //     public RectTransform contentHolder; // The Content object (the one that gets scaled)

// // //     [Header("Navigation Buttons")]
// // //     public Button nextButton;
// // //     public Button prevButton;
// // //     public Button backToMenuButton;
    
// // //     [Header("Zoom Controls")]
// // //     public float zoomSpeed = 0.1f;
// // //     public float minZoom = 1.0f;
// // //     public float maxZoom = 3.0f;

// // //     // Internal State
// // //     private int currentIndex = 0;
// // //     private float currentZoom = 1.0f;

// // //     void Start()
// // //     {
// // //         // 1. Setup Button Listeners
// // //         if(nextButton) nextButton.onClick.AddListener(NextPage);
// // //         if(prevButton) prevButton.onClick.AddListener(PrevPage);
// // //         if(backToMenuButton) backToMenuButton.onClick.AddListener(BackToMenu);

// // //         // 2. Load Images from Folder (if path is provided)
// // //         if (!string.IsNullOrEmpty(resourceFolderPath))
// // //         {
// // //             LoadImagesFromResources();
// // //         }

// // //         // 3. Validate References
// // //         if (pageDisplay == null)
// // //         {
// // //             Debug.LogError("EBookViewer: You forgot to assign 'Page Display' in the Inspector!");
// // //             return;
// // //         }

// // //         // 4. Initialize View
// // //         if (pageSprites != null && pageSprites.Count > 0)
// // //         {
// // //             UpdatePageDisplay();
// // //         }
// // //         else
// // //         {
// // //             FitContentToScreen(); 
// // //         }
        
// // //         // 5. FORCE SETTINGS: Ensure Aspect Ratio
// // //         pageDisplay.preserveAspect = true;
// // //     }

// // //     /// <summary>
// // //     /// Loads all sprites from the specified path in the Resources folder.
// // //     /// </summary>
// // //     void LoadImagesFromResources()
// // //     {
// // //         // Load all sprites at the path
// // //         Sprite[] loadedSprites = Resources.LoadAll<Sprite>(resourceFolderPath);

// // //         if (loadedSprites.Length > 0)
// // //         {
// // //             // Sort them by name to ensure Page1, Page2, Page3 order
// // //             // Note: Naming images Page_01, Page_02 is recommended for correct sorting
// // //             pageSprites = loadedSprites.OrderBy(x => x.name).ToList();
// // //             Debug.Log($"Loaded {pageSprites.Count} pages from Resources/{resourceFolderPath}");
// // //         }
// // //         else
// // //         {
// // //             Debug.LogError($"Could not find any sprites at path: Resources/{resourceFolderPath}. Make sure the folder exists inside a 'Resources' folder.");
// // //         }
// // //     }

// // //     void Update()
// // //     {
// // //         HandleTouchZoom();
// // //         HandleMouseZoom();
// // //     }

// // //     // ---------------- NAVIGATION LOGIC ---------------- //

// // //     public void NextPage()
// // //     {
// // //         if (pageSprites == null || pageSprites.Count == 0) return;

// // //         if (currentIndex < pageSprites.Count - 1)
// // //         {
// // //             currentIndex++;
// // //             UpdatePageDisplay();
// // //         }
// // //     }

// // //     public void PrevPage()
// // //     {
// // //         if (pageSprites == null || pageSprites.Count == 0) return;

// // //         if (currentIndex > 0)
// // //         {
// // //             currentIndex--;
// // //             UpdatePageDisplay();
// // //         }
// // //     }

// // //     public void BackToMenu()
// // //     {
// // //         SceneManager.LoadScene(menuSceneName);
// // //     }

// // //     private void UpdatePageDisplay()
// // //     {
// // //         if (pageSprites.Count == 0) return;

// // //         // 1. Set the image sprite
// // //         pageDisplay.sprite = pageSprites[currentIndex];

// // //         // 2. FORCE FIT: Reset everything so the huge image fits the small box
// // //         ResetZoom(); 
// // //         FitContentToScreen();

// // //         // 3. Manage Button Visibility
// // //         if(prevButton) prevButton.interactable = (currentIndex > 0);
// // //         if(nextButton) nextButton.interactable = (currentIndex < pageSprites.Count - 1);
// // //     }

// // //     // ---------------- ZOOM LOGIC ---------------- //

// // //     public void ZoomIn() => ApplyZoom(zoomSpeed);
// // //     public void ZoomOut() => ApplyZoom(-zoomSpeed);

// // //     private void ApplyZoom(float increment)
// // //     {
// // //         currentZoom += increment;
// // //         currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        
// // //         contentHolder.localScale = Vector3.one * currentZoom;
// // //     }

// // //     private void ResetZoom()
// // //     {
// // //         currentZoom = 1.0f;
// // //         contentHolder.localScale = Vector3.one;
// // //         contentHolder.anchoredPosition = Vector2.zero; 
// // //     }

// // //     /// <summary>
// // //     /// Forces the Content and Image to match the Viewport size exactly.
// // //     /// Handles the removal of conflicting components.
// // //     /// </summary>
// // //     private void FitContentToScreen()
// // //     {
// // //         // SAFETY FIX: Disable ContentSizeFitter if it exists, as it conflicts with this code
// // //         ContentSizeFitter fitter = contentHolder.GetComponent<ContentSizeFitter>();
// // //         if (fitter != null && fitter.enabled)
// // //         {
// // //             fitter.enabled = false;
// // //         }

// // //         // 1. Force Content Holder to stretch to fill the Viewport
// // //         contentHolder.anchorMin = Vector2.zero;
// // //         contentHolder.anchorMax = Vector2.one;
// // //         contentHolder.sizeDelta = Vector2.zero; 
// // //         contentHolder.anchoredPosition = Vector2.zero;

// // //         // 2. Force the Page Image to stretch to fill the Content Holder
// // //         RectTransform imageRect = pageDisplay.rectTransform;
// // //         imageRect.anchorMin = Vector2.zero;
// // //         imageRect.anchorMax = Vector2.one;
// // //         imageRect.sizeDelta = Vector2.zero; 
// // //         imageRect.anchoredPosition = Vector2.zero;

// // //         // 3. Ensure Image is visible
// // //         pageDisplay.preserveAspect = true;
// // //         // Reset scale in case it was messed up
// // //         imageRect.localScale = Vector3.one; 
// // //     }

// // //     // ---------------- INPUT HANDLING ---------------- //

// // //     private void HandleTouchZoom()
// // //     {
// // //         if (Input.touchCount == 2)
// // //         {
// // //             Touch touchZero = Input.GetTouch(0);
// // //             Touch touchOne = Input.GetTouch(1);

// // //             Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
// // //             Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

// // //             float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
// // //             float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

// // //             float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

// // //             ApplyZoom(-(deltaMagnitudeDiff * 0.01f));
// // //         }
// // //     }

// // //     private void HandleMouseZoom()
// // //     {
// // //         float scrollData = Input.GetAxis("Mouse ScrollWheel");
// // //         if (scrollData != 0.0f)
// // //         {
// // //             ApplyZoom(scrollData);
// // //         }
// // //     }
// // // }

// // using UnityEngine;
// // using UnityEngine.UI;
// // using UnityEngine.SceneManagement;
// // using System.Collections.Generic;
// // using System.Linq; 

// // public class EBookViewer : MonoBehaviour
// // {
// //     [Header("Content Configuration")]
// //     [Tooltip("Name of the folder inside 'Assets/Resources/'. Example: 'Chapter1'")]
// //     public string resourceFolderPath = ""; 
    
// //     [Tooltip("Check this if you want to verify loaded pages in Inspector")]
// //     public List<Sprite> pageSprites = new List<Sprite>(); 
// //     public string menuSceneName = "MainMenu";

// //     [Header("UI References")]
// //     public Image pageDisplay;          // The Image component inside the Content
// //     public ScrollRect scrollRect;      // The ScrollRect component
// //     public RectTransform contentHolder; // The Content object (the one that gets scaled)

// //     [Header("Navigation Buttons")]
// //     public Button nextButton;
// //     public Button prevButton;
// //     public Button backToMenuButton;
    
// //     [Header("Zoom Controls")]
// //     public float zoomSpeed = 0.1f;
// //     public float minZoom = 1.0f;
// //     public float maxZoom = 3.0f;

// //     // Internal State
// //     private int currentIndex = 0;
// //     private float currentZoom = 1.0f;

// //     void Start()
// //     {
// //         // 1. Setup Button Listeners
// //         if(nextButton) nextButton.onClick.AddListener(NextPage);
// //         if(prevButton) prevButton.onClick.AddListener(PrevPage);
// //         if(backToMenuButton) backToMenuButton.onClick.AddListener(BackToMenu);

// //         // 2. Load Images from Folder (if path is provided)
// //         if (!string.IsNullOrEmpty(resourceFolderPath))
// //         {
// //             LoadImagesFromResources();
// //         }

// //         // 3. Validate References
// //         if (pageDisplay == null)
// //         {
// //             Debug.LogError("EBookViewer: You forgot to assign 'Page Display' in the Inspector!");
// //             return;
// //         }

// //         // 4. Initialize View
// //         if (pageSprites != null && pageSprites.Count > 0)
// //         {
// //             UpdatePageDisplay();
// //         }
// //         else
// //         {
// //             FitContentToScreen(); 
// //         }
        
// //         // 5. FORCE SETTINGS: Ensure Aspect Ratio
// //         pageDisplay.preserveAspect = true;
// //     }

// //     /// <summary>
// //     /// Loads all sprites from the specified path in the Resources folder.
// //     /// Includes debugging to help the user find missing files.
// //     /// </summary>
// //     void LoadImagesFromResources()
// //     {
// //         // 1. Try to load as Sprites
// //         Sprite[] loadedSprites = Resources.LoadAll<Sprite>(resourceFolderPath);

// //         if (loadedSprites.Length > 0)
// //         {
// //             // Success! Sort them and assign.
// //             pageSprites = loadedSprites.OrderBy(x => x.name).ToList();
// //             Debug.Log($"[Success] Loaded {pageSprites.Count} pages from Resources/{resourceFolderPath}");
// //         }
// //         else
// //         {
// //             // 2. Debugging: Why did it fail?
// //             // Let's check if ANY file exists there (maybe they are Textures, not Sprites)
// //             Object[] allAssets = Resources.LoadAll(resourceFolderPath);
            
// //             if (allAssets.Length > 0)
// //             {
// //                 Debug.LogError($"[Error] Found {allAssets.Length} files in 'Resources/{resourceFolderPath}', but none were Sprites!");
// //                 Debug.LogError("SOLUTION: Select your images in the Project folder, change 'Texture Type' to 'Sprite (2D and UI)', and click Apply.");
// //             }
// //             else
// //             {
// //                 Debug.LogError($"[Error] Found ZERO files at path: 'Resources/{resourceFolderPath}'.");
// //                 Debug.LogError("CHECKLIST:\n1. Is the folder name spelled exactly right?\n2. Is the folder inside a folder named 'Resources'?\n3. Did you include 'Assets/' or 'Resources/' in the path? (Don't! Just use the subfolder name).");
// //             }
// //         }
// //     }

// //     void Update()
// //     {
// //         HandleTouchZoom();
// //         HandleMouseZoom();
// //     }

// //     // ---------------- NAVIGATION LOGIC ---------------- //

// //     public void NextPage()
// //     {
// //         if (pageSprites == null || pageSprites.Count == 0) return;

// //         if (currentIndex < pageSprites.Count - 1)
// //         {
// //             currentIndex++;
// //             UpdatePageDisplay();
// //         }
// //     }

// //     public void PrevPage()
// //     {
// //         if (pageSprites == null || pageSprites.Count == 0) return;

// //         if (currentIndex > 0)
// //         {
// //             currentIndex--;
// //             UpdatePageDisplay();
// //         }
// //     }

// //     public void BackToMenu()
// //     {
// //         SceneManager.LoadScene(menuSceneName);
// //     }

// //     private void UpdatePageDisplay()
// //     {
// //         if (pageSprites.Count == 0) return;

// //         // 1. Set the image sprite
// //         pageDisplay.sprite = pageSprites[currentIndex];

// //         // 2. FORCE FIT: Reset everything so the huge image fits the small box
// //         ResetZoom(); 
// //         FitContentToScreen();

// //         // 3. Manage Button Visibility
// //         if(prevButton) prevButton.interactable = (currentIndex > 0);
// //         if(nextButton) nextButton.interactable = (currentIndex < pageSprites.Count - 1);
// //     }

// //     // ---------------- ZOOM LOGIC ---------------- //

// //     public void ZoomIn() => ApplyZoom(zoomSpeed);
// //     public void ZoomOut() => ApplyZoom(-zoomSpeed);

// //     private void ApplyZoom(float increment)
// //     {
// //         currentZoom += increment;
// //         currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        
// //         contentHolder.localScale = Vector3.one * currentZoom;
// //     }

// //     private void ResetZoom()
// //     {
// //         currentZoom = 1.0f;
// //         contentHolder.localScale = Vector3.one;
// //         contentHolder.anchoredPosition = Vector2.zero; 
// //     }

// //     private void FitContentToScreen()
// //     {
// //         // SAFETY FIX: Disable ContentSizeFitter if it exists
// //         ContentSizeFitter fitter = contentHolder.GetComponent<ContentSizeFitter>();
// //         if (fitter != null && fitter.enabled)
// //         {
// //             fitter.enabled = false;
// //         }

// //         // 1. Force Content Holder to stretch to fill the Viewport
// //         contentHolder.anchorMin = Vector2.zero;
// //         contentHolder.anchorMax = Vector2.one;
// //         contentHolder.sizeDelta = Vector2.zero; 
// //         contentHolder.anchoredPosition = Vector2.zero;

// //         // 2. Force the Page Image to stretch to fill the Content Holder
// //         RectTransform imageRect = pageDisplay.rectTransform;
// //         imageRect.anchorMin = Vector2.zero;
// //         imageRect.anchorMax = Vector2.one;
// //         imageRect.sizeDelta = Vector2.zero; 
// //         imageRect.anchoredPosition = Vector2.zero;

// //         // 3. Ensure Image is visible
// //         pageDisplay.preserveAspect = true;
// //         imageRect.localScale = Vector3.one; 
// //     }

// //     // ---------------- INPUT HANDLING ---------------- //

// //     private void HandleTouchZoom()
// //     {
// //         if (Input.touchCount == 2)
// //         {
// //             Touch touchZero = Input.GetTouch(0);
// //             Touch touchOne = Input.GetTouch(1);

// //             Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
// //             Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

// //             float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
// //             float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

// //             float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

// //             ApplyZoom(-(deltaMagnitudeDiff * 0.01f));
// //         }
// //     }

// //     private void HandleMouseZoom()
// //     {
// //         float scrollData = Input.GetAxis("Mouse ScrollWheel");
// //         if (scrollData != 0.0f)
// //         {
// //             ApplyZoom(scrollData);
// //         }
// //     }
// // }

// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using System.Collections.Generic;
// using System.Linq; 

// public class EBookViewer : MonoBehaviour
// {
//     [Header("Content Configuration")]
//     [Tooltip("Paste your folder path here (e.g., 'Assets/Resources/Ebook_files' or just 'Ebook_files'). Files inside must be named '1', '2', '3', etc.")]
//     public string resourcePath = "Assets/Resources/Ebook_files";
    
//     [Tooltip("Click the Context Menu (3 dots on script -> Load Images From Path) to populate this list.")]
//     public List<Sprite> pageSprites = new List<Sprite>(); 
//     public string menuSceneName = "MainMenu";

//     [Header("UI References")]
//     public Image pageDisplay;          
//     public ScrollRect scrollRect;      
//     public RectTransform contentHolder; 

//     [Header("Navigation Buttons")]
//     public Button nextButton;
//     public Button prevButton;
//     public Button backToMenuButton;
    
//     [Header("Zoom Controls")]
//     public float zoomSpeed = 0.1f;
//     public float minZoom = 1.0f;
//     public float maxZoom = 3.0f;

//     // Internal State
//     private int currentIndex = 0;
//     private float currentZoom = 1.0f;

//     void Start()
//     {
//         // 1. Setup Button Listeners
//         if(nextButton) nextButton.onClick.AddListener(NextPage);
//         if(prevButton) prevButton.onClick.AddListener(PrevPage);
//         if(backToMenuButton) backToMenuButton.onClick.AddListener(BackToMenu);

//         // 2. Auto-load if list is empty but path is provided
//         if (pageSprites.Count == 0 && !string.IsNullOrEmpty(resourcePath))
//         {
//             LoadImagesInternal();
//         }

//         // 3. Initialize View
//         if (pageDisplay == null)
//         {
//             Debug.LogError("EBookViewer: 'Page Display' UI is missing assignment!");
//             return;
//         }

//         if (pageSprites != null && pageSprites.Count > 0)
//         {
//             UpdatePageDisplay();
//         }
//         else
//         {
//             // If still empty, try to just fit whatever is currently on screen
//             FitContentToScreen(); 
//         }
        
//         pageDisplay.preserveAspect = true;
//     }

//     // ---------------- LOADING LOGIC ---------------- //

//     /// <summary>
//     /// Right-click the component in Inspector and choose "Load Images From Path" to run this.
//     /// </summary>
//     [ContextMenu("Load Images From Path")]
//     public void LoadImagesInternal()
//     {
//         pageSprites.Clear();

//         // 1. Clean the path
//         // Unity Resources.Load expects a path RELATIVE to the Resources folder.
//         // We strip "Assets/" and "Resources/" if the user pasted the full path.
//         string cleanPath = resourcePath;
        
//         if (cleanPath.Contains("Resources/"))
//         {
//             // Split by "Resources/" and take the second part
//             string[] parts = cleanPath.Split(new string[] { "Resources/" }, System.StringSplitOptions.None);
//             if (parts.Length > 1) cleanPath = parts[1];
//         }
        
//         // Remove trailing slash if present
//         cleanPath = cleanPath.TrimEnd('/');

//         Debug.Log($"Attempting to load sprites from Resources path: '{cleanPath}'");

//         // 2. Load All Sprites
//         Sprite[] loadedSprites = Resources.LoadAll<Sprite>(cleanPath);

//         if (loadedSprites.Length > 0)
//         {
//             // 3. Numerical Sort (1, 2, 10 instead of 1, 10, 2)
//             // We try to parse the name as an integer. If it fails, we put it at the end.
//             pageSprites = loadedSprites.OrderBy(x => 
//             {
//                 if (int.TryParse(x.name, out int number))
//                     return number;
//                 else
//                     return 999999; // Non-numbers go to the end
//             }).ToList();

//             Debug.Log($"[Success] Loaded {pageSprites.Count} pages from '{cleanPath}'. Check the 'Page Sprites' list in Inspector.");
//         }
//         else
//         {
//             Debug.LogError($"[Error] No sprites found at 'Resources/{cleanPath}'.\n" +
//                            "1. Ensure folder exists inside 'Assets/Resources/'.\n" +
//                            "2. Ensure images are set to 'Sprite (2D and UI)' in import settings.");
//         }
//     }

//     // ---------------- NAVIGATION LOGIC ---------------- //

//     public void NextPage()
//     {
//         if (pageSprites == null || pageSprites.Count == 0) return;

//         if (currentIndex < pageSprites.Count - 1)
//         {
//             currentIndex++;
//             UpdatePageDisplay();
//         }
//     }

//     public void PrevPage()
//     {
//         if (pageSprites == null || pageSprites.Count == 0) return;

//         if (currentIndex > 0)
//         {
//             currentIndex--;
//             UpdatePageDisplay();
//         }
//     }

//     public void BackToMenu()
//     {
//         SceneManager.LoadScene(menuSceneName);
//     }

//     private void UpdatePageDisplay()
//     {
//         if (pageSprites.Count == 0) return;

//         pageDisplay.sprite = pageSprites[currentIndex];

//         ResetZoom(); 
//         FitContentToScreen();

//         if(prevButton) prevButton.interactable = (currentIndex > 0);
//         if(nextButton) nextButton.interactable = (currentIndex < pageSprites.Count - 1);
//     }

//     // ---------------- ZOOM LOGIC ---------------- //

//     public void ZoomIn() => ApplyZoom(zoomSpeed);
//     public void ZoomOut() => ApplyZoom(-zoomSpeed);

//     private void ApplyZoom(float increment)
//     {
//         currentZoom += increment;
//         currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        
//         contentHolder.localScale = Vector3.one * currentZoom;
//     }

//     private void ResetZoom()
//     {
//         currentZoom = 1.0f;
//         contentHolder.localScale = Vector3.one;
//         contentHolder.anchoredPosition = Vector2.zero; 
//     }

//     private void FitContentToScreen()
//     {
//         ContentSizeFitter fitter = contentHolder.GetComponent<ContentSizeFitter>();
//         if (fitter != null && fitter.enabled)
//         {
//             fitter.enabled = false;
//         }

//         contentHolder.anchorMin = Vector2.zero;
//         contentHolder.anchorMax = Vector2.one;
//         contentHolder.sizeDelta = Vector2.zero; 
//         contentHolder.anchoredPosition = Vector2.zero;

//         RectTransform imageRect = pageDisplay.rectTransform;
//         imageRect.anchorMin = Vector2.zero;
//         imageRect.anchorMax = Vector2.one;
//         imageRect.sizeDelta = Vector2.zero; 
//         imageRect.anchoredPosition = Vector2.zero;

//         pageDisplay.preserveAspect = true;
//         imageRect.localScale = Vector3.one; 
//     }

//     // ---------------- INPUT HANDLING ---------------- //

//     private void HandleTouchZoom()
//     {
//         if (Input.touchCount == 2)
//         {
//             Touch touchZero = Input.GetTouch(0);
//             Touch touchOne = Input.GetTouch(1);

//             Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
//             Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

//             float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
//             float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

//             float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

//             ApplyZoom(-(deltaMagnitudeDiff * 0.01f));
//         }
//     }

//     private void HandleMouseZoom()
//     {
//         float scrollData = Input.GetAxis("Mouse ScrollWheel");
//         if (scrollData != 0.0f)
//         {
//             ApplyZoom(scrollData);
//         }
//     }
// }

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq; 

public class EBookViewer : MonoBehaviour
{
    [Header("Content Configuration")]
    [Tooltip("Paste your folder path here (e.g., 'Assets/Resources/Ebook_files' or just 'Ebook_files'). Files inside must be named '1', '2', '3', etc.")]
    public string resourcePath = "Assets/Resources/Ebook_files";
    
    [Tooltip("Click the Context Menu (3 dots on script -> Load Images From Path) to populate this list.")]
    public List<Sprite> pageSprites = new List<Sprite>(); 
    public string menuSceneName = "MainMenu";

    [Header("UI References")]
    public Image pageDisplay;          
    public ScrollRect scrollRect;      
    public RectTransform contentHolder; 

    [Header("Navigation Buttons")]
    public Button nextButton;
    public Button prevButton;
    public Button backToMenuButton;
    
    [Header("Zoom Controls")]
    public float zoomSpeed = 0.1f;
    public float minZoom = 1.0f;
    public float maxZoom = 3.0f;

    // Internal State
    private int currentIndex = 0;
    private float currentZoom = 1.0f;

    void Start()
    {
        // 1. Setup Button Listeners
        if(nextButton) nextButton.onClick.AddListener(NextPage);
        if(prevButton) prevButton.onClick.AddListener(PrevPage);
        if(backToMenuButton) backToMenuButton.onClick.AddListener(BackToMenu);

        // 2. Auto-load if list is empty but path is provided
        if (pageSprites.Count == 0 && !string.IsNullOrEmpty(resourcePath))
        {
            LoadImagesInternal();
        }

        // 3. Initialize View
        if (pageDisplay == null)
        {
            Debug.LogError("EBookViewer: 'Page Display' UI is missing assignment!");
            return;
        }

        if (pageSprites != null && pageSprites.Count > 0)
        {
            UpdatePageDisplay();
        }
        else
        {
            // If still empty, try to just fit whatever is currently on screen
            FitContentToScreen(); 
        }
        
        pageDisplay.preserveAspect = true;
    }

    // ---------------- LOADING LOGIC ---------------- //

    /// <summary>
    /// Right-click the component in Inspector and choose "Load Images From Path" to run this.
    /// </summary>
    [ContextMenu("Load Images From Path")]
    public void LoadImagesInternal()
    {
        pageSprites.Clear();

        // 1. Clean the path
        // Unity Resources.Load expects a path RELATIVE to the Resources folder.
        // We strip "Assets/" and "Resources/" if the user pasted the full path.
        string cleanPath = resourcePath;
        
        if (cleanPath.Contains("Resources/"))
        {
            // Split by "Resources/" and take the second part
            string[] parts = cleanPath.Split(new string[] { "Resources/" }, System.StringSplitOptions.None);
            if (parts.Length > 1) cleanPath = parts[1];
        }
        
        // Remove trailing slash if present
        cleanPath = cleanPath.TrimEnd('/');

        Debug.Log($"Attempting to load sprites from Resources path: '{cleanPath}'");

        // 2. Load All Sprites
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(cleanPath);

        if (loadedSprites.Length > 0)
        {
            // 3. Numerical Sort (1, 2, 10 instead of 1, 10, 2)
            // We try to parse the name as an integer. If it fails, we put it at the end.
            pageSprites = loadedSprites.OrderBy(x => 
            {
                if (int.TryParse(x.name, out int number))
                    return number;
                else
                    return 999999; // Non-numbers go to the end
            }).ToList();

            Debug.Log($"[Success] Loaded {pageSprites.Count} pages from '{cleanPath}'. Check the 'Page Sprites' list in Inspector.");
        }
        else
        {
            Debug.LogError($"[Error] No sprites found at 'Resources/{cleanPath}'.\n" +
                           "1. Ensure folder exists inside 'Assets/Resources/'.\n" +
                           "2. Ensure images are set to 'Sprite (2D and UI)' in import settings.");
        }
    }

    // ---------------- NAVIGATION LOGIC ---------------- //

    public void NextPage()
    {
        if (pageSprites == null || pageSprites.Count == 0) return;

        if (currentIndex < pageSprites.Count - 1)
        {
            currentIndex++;
            UpdatePageDisplay();
        }
    }

    public void PrevPage()
    {
        if (pageSprites == null || pageSprites.Count == 0) return;

        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePageDisplay();
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    private void UpdatePageDisplay()
    {
        if (pageSprites.Count == 0) return;

        pageDisplay.sprite = pageSprites[currentIndex];

        ResetZoom(); 
        FitContentToScreen();

        if(prevButton) prevButton.interactable = (currentIndex > 0);
        if(nextButton) nextButton.interactable = (currentIndex < pageSprites.Count - 1);
    }

    // ---------------- ZOOM LOGIC ---------------- //

    public void ZoomIn() => ApplyZoom(zoomSpeed);
    public void ZoomOut() => ApplyZoom(-zoomSpeed);

    private void ApplyZoom(float increment)
    {
        currentZoom += increment;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        
        contentHolder.localScale = Vector3.one * currentZoom;
    }

    private void ResetZoom()
    {
        currentZoom = 1.0f;
        contentHolder.localScale = Vector3.one;
        contentHolder.anchoredPosition = Vector2.zero; 
    }

    private void FitContentToScreen()
    {
        ContentSizeFitter fitter = contentHolder.GetComponent<ContentSizeFitter>();
        if (fitter != null && fitter.enabled)
        {
            fitter.enabled = false;
        }

        contentHolder.anchorMin = Vector2.zero;
        contentHolder.anchorMax = Vector2.one;
        contentHolder.sizeDelta = Vector2.zero; 
        contentHolder.anchoredPosition = Vector2.zero;

        RectTransform imageRect = pageDisplay.rectTransform;
        imageRect.anchorMin = Vector2.zero;
        imageRect.anchorMax = Vector2.one;
        imageRect.sizeDelta = Vector2.zero; 
        imageRect.anchoredPosition = Vector2.zero;

        pageDisplay.preserveAspect = true;
        imageRect.localScale = Vector3.one; 
    }

    // ---------------- INPUT HANDLING ---------------- //

    private void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            ApplyZoom(-(deltaMagnitudeDiff * 0.01f));
        }
    }

    private void HandleMouseZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (scrollData != 0.0f)
        {
            ApplyZoom(scrollData);
        }
    }
}