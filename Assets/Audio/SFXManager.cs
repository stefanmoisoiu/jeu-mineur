using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int audioSourceCount;
    [SerializeField] private AudioMixerGroup mixerGroup;


    [SerializeField] private AudioSource[] audioSources;
    private ushort _currentAudioSourceIndex;
    
    private static SFXManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void UpdateAudioSources()
    {
        if (audioSources != null) foreach (AudioSource audioSource in audioSources) DestroyImmediate(audioSource);
        
        audioSources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSourceCount; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
            audioSources[i].outputAudioMixerGroup = mixerGroup;
        }
    }
    private AudioSource GetAudioSource()
    {
        AudioSource src = audioSources[_currentAudioSourceIndex];
        _currentAudioSourceIndex++;
        if (_currentAudioSourceIndex >= audioSources.Length) _currentAudioSourceIndex = 0;
        
        return src;
    }
    public static void Play(AudioClip clip, float volume, float pitch)
    {
        AudioSource src = _instance.GetAudioSource();
        src.pitch = pitch;
        src.PlayOneShot(clip, volume);
    }
}