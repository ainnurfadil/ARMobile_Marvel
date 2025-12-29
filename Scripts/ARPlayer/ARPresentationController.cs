using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARPresentationController : MonoBehaviour
{
    [Header("Target 3D Model")]
    [Tooltip("Drag object 3D Anda yang memiliki komponen Animator ke sini")]
    public Animator targetAnimator;
    [Tooltip("Nama State animasi di Animator Controller (default biasanya 'Take 001' atau 'Mixamo_com')")]
    public string animationStateName = "MainAnimation"; 

    [Header("Timeline Configuration")]
    [Tooltip("Masukkan detik-detik penting (marker) di sini. Contoh: 0, 5, 12.5, 20")]
    public List<float> timeMarkers = new List<float>();

    [Header("UI References")]
    public Button btnBack;
    public Button btnPrevious;
    public Button btnPlay;
    public Button btnNext;
    public Image imgPlayIcon;

    [Header("UI Assets")]
    public Sprite iconPlay;
    public Sprite iconPause;

    // Internal State
    private bool isPlaying = true;
    private float animationLength;
    private int currentMarkerIndex = 0;

    void Start()
    {
        // 1. Setup Button Listeners
        btnPlay.onClick.AddListener(TogglePlayPause);
        btnNext.onClick.AddListener(SkipToNext);
        btnPrevious.onClick.AddListener(SkipToPrevious);
        btnBack.onClick.AddListener(GoBackScene);

        // 2. Initial State Check
        if (targetAnimator != null)
        {
            // Update ikon awal
            UpdatePlayIcon();
            
            // Ambil panjang animasi untuk kalkulasi (opsional, untuk safety)
            StartCoroutine(GetAnimationLength());
        }
        else
        {
            Debug.LogError("Target Animator belum di-assign di Inspector!");
        }
    }

    // Coroutine untuk memastikan Animator sudah siap sebelum mengambil info
    IEnumerator GetAnimationLength()
    {
        yield return new WaitForEndOfFrame();
        if (targetAnimator != null)
        {
            AnimatorStateInfo stateInfo = targetAnimator.GetCurrentAnimatorStateInfo(0);
            animationLength = stateInfo.length;
        }
    }

    void Update()
    {
        // Opsional: Jika Anda ingin slider UI bergerak otomatis, logic-nya bisa ditaruh disini
    }

    #region Core Functions

    // Fungsi Play/Pause
    public void TogglePlayPause()
    {
        isPlaying = !isPlaying;

        if (isPlaying)
        {
            targetAnimator.speed = 1; // Lanjut main
        }
        else
        {
            targetAnimator.speed = 0; // Freeze animasi (Pause)
        }

        UpdatePlayIcon();
    }

    // Fungsi Next (Skip ke marker selanjutnya)
    public void SkipToNext()
    {
        float currentTime = GetCurrentTime();
        
        // Cari marker pertama yang waktunya LEBIH BESAR dari waktu sekarang
        float targetTime = -1f;
        
        foreach(float marker in timeMarkers)
        {
            if(marker > currentTime + 0.5f) // Tambah offset dikit biar gak stuck di marker yang sama
            {
                targetTime = marker;
                break;
            }
        }

        // Jika ketemu marker di depan, lompat kesana
        if (targetTime != -1f)
        {
            JumpToTime(targetTime);
        }
        else
        {
            // Jika tidak ada marker lagi, mungkin loop ke awal atau biarkan saja
            Debug.Log("End of timeline markers");
        }
    }

    // Fungsi Previous (Skip ke marker sebelumnya)
    public void SkipToPrevious()
    {
        float currentTime = GetCurrentTime();

        // Cari marker yang waktunya LEBIH KECIL dari waktu sekarang (paling dekat)
        float targetTime = 0f; // Default ke awal (0)

        for (int i = timeMarkers.Count - 1; i >= 0; i--)
        {
            if (timeMarkers[i] < currentTime - 0.5f)
            {
                targetTime = timeMarkers[i];
                break;
            }
        }

        JumpToTime(targetTime);
    }

    // Fungsi Back Scene
    public void GoBackScene()
    {
        // Ganti index atau nama scene sesuai scene menu/sebelumnya
        // Pastikan scene tersebut sudah ada di Build Settings
        if(SceneManager.sceneCountInBuildSettings > 1)
        {
             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            Debug.LogWarning("Tidak ada scene sebelumnya di Build Settings.");
        }
    }

    #endregion

    #region Helper Functions

    // Helper untuk melompat ke detik tertentu
    private void JumpToTime(float timeInSeconds)
    {
        if (targetAnimator == null || animationLength <= 0) return;

        // Normalisasi waktu (0.0 sampai 1.0) karena Play() butuh normalized time
        float normalizedTime = timeInSeconds / animationLength;
        
        // Clamp agar tidak error
        normalizedTime = Mathf.Clamp01(normalizedTime);

        // Lompat animasi
        targetAnimator.Play(animationStateName, 0, normalizedTime);
        
        // Opsional: Jika di-pause saat skip, apakah mau auto-play atau tetap pause?
        // Code ini membiarkan status play/pause seperti sebelumnya.
    }

    // Helper untuk dapat waktu sekarang dalam detik
    private float GetCurrentTime()
    {
        if (targetAnimator == null) return 0f;
        
        AnimatorStateInfo stateInfo = targetAnimator.GetCurrentAnimatorStateInfo(0);
        // stateInfo.normalizedTime bisa lebih dari 1 jika looping, kita ambil modulo 1
        return (stateInfo.normalizedTime % 1) * stateInfo.length;
    }

    // Update Icon Button
    private void UpdatePlayIcon()
    {
        if (imgPlayIcon != null)
        {
            imgPlayIcon.sprite = isPlaying ? iconPause : iconPlay;
        }
    }

    #endregion
}