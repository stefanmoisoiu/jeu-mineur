using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;
    
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    
    [SerializeField] private TMP_Text masterVolumeText;
    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;
    
    

    
    
    [Header("Properties")]
    [SerializeField] private string settingsSceneName = "Settings Menu";
    
    private static readonly string MasterVolumeSaveKey = "masterVolume";
    private static readonly string MusicVolumeSaveKey = "musicVolume";
    private static readonly string SfxVolumeSaveKey = "sfxVolume";
    [SerializeField] private ScriptableSFX previewSFX;

    [SerializeField] private Selectable mainFocus;
    
    
    
    public static Action OnOpenSettings, OnCloseSettings;
    public static bool SettingsOpen { get; private set; }

    private void OnEnable()
    {
        SettingsOpen = true;
        OnOpenSettings?.Invoke();
        
        mainFocus.Select();
        
        LoadSavedSettings();
    }

    private void OnDisable()
    {
        SettingsOpen = false;
        OnCloseSettings?.Invoke();
        
        SaveSettings();
    }

    public void CloseSettingsButton()
    {
        SceneManager.UnloadSceneAsync(settingsSceneName);
    }

    private void LoadSavedSettings()
    {
        try
        {
            ushort masterVolume = ES3.Load<ushort>(MasterVolumeSaveKey);
            SetMasterVolume(masterVolume);
        }
        catch
        {
            SetMasterVolume(masterSlider.maxValue);
        }
        
        try
        {
            ushort musicVolume = ES3.Load<ushort>(MusicVolumeSaveKey);
            SetMusicVolume(musicVolume);
        }
        catch
        {
            SetMusicVolume(musicSlider.maxValue);
        }
        
        try
        {
            ushort sfxVolume = ES3.Load<ushort>(SfxVolumeSaveKey);
            SetSFXVolume(sfxVolume);
        }
        catch
        {
            SetSFXVolume(sfxSlider.maxValue);
        }
    }
    private void SaveSettings()
    {
        ES3.Save(MasterVolumeSaveKey, (ushort)masterSlider.value);
        ES3.Save(MusicVolumeSaveKey, (ushort)musicSlider.value);
        ES3.Save(SfxVolumeSaveKey, (ushort)sfxSlider.value);
    }
    
    public void MasterVolumeChanged()
    {
        SetMasterVolume(masterSlider.value);
        previewSFX.Play();
    }
    public void MusicVolumeChanged()
    {
        SetMusicVolume(musicSlider.value);
    }
    public void SFXVolumeChanged()
    {
        SetSFXVolume(sfxSlider.value);
        previewSFX.Play();
    }
    
    private void SetSFXVolume(float value)
    {
        float advancement = 1 - Mathf.Pow(1 - value / 10, 4);
        float volume = Mathf.Lerp(-80, 0, advancement);
        sfxGroup.audioMixer.SetFloat("sfxVolume", volume);
        
        sfxSlider.SetValueWithoutNotify(value);
        sfxVolumeText.text = sfxSlider.value.ToString();
    }
    private void SetMusicVolume(float value)
    {
        float advancement = 1 - Mathf.Pow(1 - value / 10, 4);
        float volume = Mathf.Lerp(-80, 0, advancement);
        musicGroup.audioMixer.SetFloat("musicVolume", volume);
        
        musicSlider.SetValueWithoutNotify(value);
        musicVolumeText.text = musicSlider.value.ToString();
    }
    private void SetMasterVolume(float value)
    {
        float advancement = 1 - Mathf.Pow(1 - value / 10, 4);
        float volume = Mathf.Lerp(-80, 0, advancement);
        masterGroup.audioMixer.SetFloat("masterVolume", volume);
        
        masterSlider.SetValueWithoutNotify(value);
        masterVolumeText.text = masterSlider.value.ToString();
    }
}
