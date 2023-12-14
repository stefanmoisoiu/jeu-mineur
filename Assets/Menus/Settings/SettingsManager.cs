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

    [SerializeField] private Vector2 masterVolumeRange = new (-80, 0);
    [SerializeField] private Vector2 musicVolumeRange = new (-80, 0);
    [SerializeField] private Vector2 sfxVolumeRange = new (-80, 0);
    
    
    public static Action OnOpenSettings, OnCloseSettings;
    public static bool SettingsOpen { get; private set; }

    private void OnEnable()
    {
        SettingsOpen = true;
        OnOpenSettings?.Invoke();
    }

    private void OnDisable()
    {
        SettingsOpen = false;
        OnCloseSettings?.Invoke();
    }

    public void CloseSettingsButton()
    {
        SceneManager.UnloadSceneAsync(settingsSceneName);
    }
    
    public void UpdateMasterVolume()
    {
        masterGroup.audioMixer.SetFloat("masterVolume", Mathf.Lerp(masterVolumeRange.x,masterVolumeRange.y,1-Mathf.Pow(1-masterSlider.value / masterSlider.maxValue,4)));
        masterVolumeText.text = masterSlider.value.ToString();
    }
    public void UpdateMusicVolume()
    {
        musicGroup.audioMixer.SetFloat("musicVolume", Mathf.Lerp(musicVolumeRange.x,musicVolumeRange.y, 1-Mathf.Pow(1-musicSlider.value / musicSlider.maxValue,4)));
        musicVolumeText.text = musicSlider.value.ToString();
    }
    public void UpdateSFXVolume()
    {
        sfxGroup.audioMixer.SetFloat("sfxVolume", Mathf.Lerp(sfxVolumeRange.x,sfxVolumeRange.y, 1-Mathf.Pow(1-sfxSlider.value / sfxSlider.maxValue,4)));
        sfxVolumeText.text = sfxSlider.value.ToString();
    }
}
