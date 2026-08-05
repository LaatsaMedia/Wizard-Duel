using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManagerUI : MonoBehaviour
{
    [Header("Sliders")]
    //[SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    [Header("Labels")]
    //[SerializeField] private TMP_Text masterValue;
    [SerializeField] private TMP_Text sfxValue;
    [SerializeField] private TMP_Text musicValue;

    private void Start()
    {
        //masterSlider.value = AudioManager.Instance.MasterVolume;
        sfxSlider.value = AudioManager.Instance.SFXVolume;
        musicSlider.value = AudioManager.Instance.MusicVolume;

        //UpdateMaster(masterSlider.value);
        UpdateSFX(sfxSlider.value);
        UpdateMusic(musicSlider.value);

        //masterSlider.onValueChanged.AddListener(UpdateMaster);
        sfxSlider.onValueChanged.AddListener(UpdateSFX);
        musicSlider.onValueChanged.AddListener(UpdateMusic);
    }

    private void UpdateMaster(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);

        //if (masterValue != null)
        //    masterValue.text = $"{Mathf.RoundToInt(value * 100f)}%";
    }

    private void UpdateSFX(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);

        if (sfxValue != null)
            sfxValue.text = $"{Mathf.RoundToInt(value * 100f)}%";
    }

    private void UpdateMusic(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);

        if (musicValue != null)
            musicValue.text = $"{Mathf.RoundToInt(value * 100f)}%";
    }
}