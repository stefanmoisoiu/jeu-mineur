using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(CopyTiles))]
public class CopyTilesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CopyTiles myScript = (CopyTiles) target;
        if (GUILayout.Button("Copy"))
        {
            myScript.Copy();
        }
    }
}
#endif