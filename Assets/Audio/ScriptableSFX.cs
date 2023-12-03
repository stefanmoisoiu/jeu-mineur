using UnityEngine;

[CreateAssetMenu(fileName = "SFX", menuName = "Audio/SFX", order = 1)]
public class ScriptableSFX : ScriptableObject
{
    public AudioClip[] clips;
    [Range(0,2)]public float volume = 1f;
    [Range(0,2)]public float pitch = 1f;
    [Range(-0.5f,0.5f)]public float pitchVariation = 0.1f;

    public void Play()
    {
        if (clips == null) return;
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        SFXManager.Play(clip, volume, pitch + Random.Range(-pitchVariation, pitchVariation));
    }
    
    #if UNITY_EDITOR
    public void GetAudioClipsInFolder()
    {
        string path = UnityEditor.AssetDatabase.GetAssetPath(this);
        path = path.Substring(0, path.LastIndexOf('/'));
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:AudioClip", new[] {path});
        Debug.Log($"Found {guids.Length} audio clips in folder {path}");
        clips = new AudioClip[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            clips[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
        }
        // Mark this ScriptableObject as dirty because we have made changes that we want to save.
        UnityEditor.EditorUtility.SetDirty(this);

        // Save all changes made in the editor.
        UnityEditor.AssetDatabase.SaveAssets();
    }
    #endif
}