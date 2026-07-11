using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    // Uncomment when music is added.
    //[SerializeField] private AudioSource musicSource;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float masterVolume = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 1f;

    [Header("Settings")]
    [Range(0f, 0.2f)]
    [SerializeField] private float pitchVariation = 0.05f;

    private const string MasterVolumeParameter = "MasterVolume";
    private const string SFXVolumeParameter = "SFXVolume";
    private const string MusicVolumeParameter = "MusicVolume";

    public float MasterVolume => masterVolume;
    public float SFXVolume => sfxVolume;
    public float MusicVolume => musicVolume;

    private void Awake()
    {
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
    }

    private void Start()
    {
        ApplyVolumes();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (audioMixer == null)
            return;

        ApplyVolumes();
    }
#endif

    public void Play(
        AudioClip clip,
        bool randomizePitch = true,
        float volume = 1f)
    {
        if (clip == null)
            return;

        sfxSource.pitch = randomizePitch
            ? Random.Range(
                1f - pitchVariation,
                1f + pitchVariation)
            : 1f;

        sfxSource.PlayOneShot(clip, volume);

        sfxSource.pitch = 1f;
    }

    private void ApplyVolumes()
    {
        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetMusicVolume(musicVolume);
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;

        audioMixer.SetFloat(
            MasterVolumeParameter,
            LinearToDecibel(value));
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;

        audioMixer.SetFloat(
            SFXVolumeParameter,
            LinearToDecibel(value));
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;

        audioMixer.SetFloat(
            MusicVolumeParameter,
            LinearToDecibel(value));
    }

    private float LinearToDecibel(float value)
    {
        if (value <= 0.0001f)
            return -80f;

        return Mathf.Log10(value) * 20f;
    }

    /*
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null)
            return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
    */
}