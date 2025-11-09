using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Movement & Basic Actions")]
    public AudioClip hopSound;
    public AudioClip respawnSound;

    [Header("Deaths")]
    public AudioClip waterDeathSound;
    public AudioClip roadDeathSound;

    [Header("Game States")]
    public AudioClip timeWarningSound;
    public AudioClip gameOverSound;
    public AudioClip levelCompletedSound;

    [Header("Home Sounds")]
    public AudioClip homeSound;
    public AudioClip specialHomeSound;
    
    [Header("Background Music")]
    public AudioClip backgroundMusic;
    public bool musicEnabled = true;
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.9f;
    public float duckVolume = 0.2f; // temporay lowered volume when SFX plays

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioSource uiSource;

    private Coroutine duckCoroutine; 

    private void Awake()
    {
        // Singleton pattern so any script can call AudioManager.Instance.PlaySound(...)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Create separate channels
        sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        uiSource = gameObject.AddComponent<AudioSource>();

        // Configure separate channels
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;
        uiSource.volume = sfxVolume;

       // musicSource.loop = true;
        //musicSource.volume = 0.5f;
    }

    private void Start()
    {
        if (musicEnabled && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();

        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            sfxSource.PlayOneShot(clip); 
        }
    }

    // For UI or special sounds that should be lowered
    public void PlayWithDucking(AudioClip clip, float duckTime = 1.5f)
    {
        if (clip == null) return;

        uiSource.PlayOneShot(clip);

        // Temporarily reduce background music volume
        if (duckCoroutine != null)
        {
            StopCoroutine(duckCoroutine);
            duckCoroutine = StartCoroutine(DuckMusic(duckTime)); 
        }
    }
    private IEnumerator DuckMusic(float duration)
    {
        float originalVol = musicSource.volume;
        musicSource.volume = duckVolume;
        yield return new WaitForSeconds(duration);
        musicSource.volume = originalVol;
    }
    public void ToggleMusic()
    {
        musicEnabled= !musicEnabled;

        if (musicEnabled)
        {
            musicSource.Play();
        }
        else
        {
            StartCoroutine(FadeOut(musicSource, 0.5f));
        }
    }

    private IEnumerator FadeOut(AudioSource source, float fadeTime)
    {
        float startVol = source.volume;
        while (source.volume > 0)
        {
            source.volume -= startVol * Time.deltaTime / fadeTime;
            yield return null;
        }

        source.Stop();
        source.volume = startVol;
    }

    public void PlayRandomized(AudioClip clip, float minPitch = 0.95f, float maxPitch = 1.05f)
    {
        if (clip == null) return;
        sfxSource.pitch = Random.Range(minPitch, maxPitch);
        sfxSource.PlayOneShot(clip);
        sfxSource.pitch = 1f; 
    }
}
