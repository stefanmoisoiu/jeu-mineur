#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilemapEdgeFill))]
public class TilemapEdgeFillEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        TilemapEdgeFill tilemapEdgeFill = (TilemapEdgeFill) target;
        if (GUILayout.Button("Connect"))
            tilemapEdgeFill.Connect();
    }
}
#endif