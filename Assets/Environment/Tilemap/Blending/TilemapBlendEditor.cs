#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilemapBlend))]
public class TilemapBlendEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        TilemapBlend tilemapBlend = (TilemapBlend) target;
        if (GUILayout.Button("Blend"))
            tilemapBlend.Blend();
    }
}
#endif