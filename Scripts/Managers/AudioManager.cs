using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("AudioManager");
                instance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    [Header("Music Clips")]
    public AudioClip backgroundMusic;
    public AudioClip menuMusic;
    
    [Header("SFX Clips")]
    public AudioClip buttonClick;
    public AudioClip correctAnswer;
    public AudioClip wrongAnswer;
    public AudioClip successFanfare;
    public AudioClip blockPlaced;
    public AudioClip blockTap;
    
    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 0.7f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        SetupAudioSources();
    }
    
    void SetupAudioSources()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
        
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        
        UpdateVolumes();
    }
    
    void UpdateVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = GameManager.Instance.musicEnabled ? musicVolume : 0f;
        }
        
        if (sfxSource != null)
        {
            sfxSource.volume = GameManager.Instance.soundEnabled ? sfxVolume : 0f;
        }
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && GameManager.Instance.musicEnabled)
        {
            if (musicSource.clip != clip)
            {
                musicSource.clip = clip;
                musicSource.Play();
            }
        }
    }
    
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
    
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && GameManager.Instance.soundEnabled && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
    
    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }
    
    public void PlayCorrectAnswer()
    {
        PlaySFX(correctAnswer);
    }
    
    public void PlayWrongAnswer()
    {
        PlaySFX(wrongAnswer);
    }
    
    public void PlaySuccess()
    {
        PlaySFX(successFanfare);
    }
    
    public void PlayBlockPlaced()
    {
        PlaySFX(blockPlaced);
    }
    
    public void PlayBlockTap()
    {
        PlaySFX(blockTap);
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = GameManager.Instance.musicEnabled ? musicVolume : 0f;
        }
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null)
        {
            sfxSource.volume = GameManager.Instance.soundEnabled ? sfxVolume : 0f;
        }
    }
    
    public void ToggleMusic()
    {
        GameManager.Instance.ToggleMusic();
        UpdateVolumes();
    }
    
    public void ToggleSound()
    {
        GameManager.Instance.ToggleSound();
        UpdateVolumes();
    }
}