using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadSavedVolumeOnStart : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup masterGroup, musicGroup, sfxGroup;
    
    private static readonly string MasterVolumeSaveKey = "masterVolume";
    private static readonly string MusicVolumeSaveKey = "musicVolume";
    private static readonly string SfxVolumeSaveKey = "sfxVolume";

    private void Start()
    {
        IEnumerator LoadOnStart()
        {
            yield return new WaitForSeconds(0.1f);
            Load();
        }
        Load();
        // StartCoroutine(LoadOnStart());
    }

    public void Load()
    {
        try
        {
            float advancement = 1 - Mathf.Pow(1 - ES3.Load<ushort>(MasterVolumeSaveKey) / 10f, 4);
            float volume = Mathf.Lerp(-80, 0, advancement);
            masterGroup.audioMixer.SetFloat("masterVolume", volume);
        }
        catch (Exception e)
        {
            // ignored
        }
        
        try
        {
            float advancement = 1 - Mathf.Pow(1 - ES3.Load<ushort>(MusicVolumeSaveKey) / 10f, 4);
            float volume = Mathf.Lerp(-80, 0, advancement);
            musicGroup.audioMixer.SetFloat("musicVolume", volume);
        }
        catch (Exception e)
        {
            // ignored
        }
        
        try
        {
            float advancement = 1 - Mathf.Pow(1 - ES3.Load<ushort>(SfxVolumeSaveKey) / 10f, 4);
            float volume = Mathf.Lerp(-80, 0, advancement);
            sfxGroup.audioMixer.SetFloat("sfxVolume", volume);
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}
