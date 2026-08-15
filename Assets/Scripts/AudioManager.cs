using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] public AudioSource wireLoopSource;
    public AudioClip timerTick;
    public AudioClip timerEnd;
    public AudioClip geigerClick;
    public AudioClip deviceBreak;
    public AudioClip wireSizzle;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlaySFX(AudioClip clip, float volumeScale = 1.0f)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, volumeScale);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }


}

