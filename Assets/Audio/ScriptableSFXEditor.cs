#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScriptableSFX))]
public class ScriptableSFXEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ScriptableSFX sfx = (ScriptableSFX) target;
        if (GUILayout.Button("Get Audio Clips In Folder"))
        {
            sfx.GetAudioClipsInFolder();
        }
    }
}
#endif