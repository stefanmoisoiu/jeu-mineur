#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SFXManager))]
public class SFXManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SFXManager sfxManager = (SFXManager)target;
        if (GUILayout.Button("Update Audio Sources")) sfxManager.UpdateAudioSources();
    }
}
#endif