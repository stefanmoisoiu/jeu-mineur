#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColliderEvents))]
public class ColliderEventsEditor : Editor
{
    private bool _showUnityEvents = false;
    public override void OnInspectorGUI()
    {
        if (EditorGUILayout.Foldout(_showUnityEvents, "Unity Events"))
        {
            _showUnityEvents = true;
            base.OnInspectorGUI();
        }
        else _showUnityEvents = false;
    }
}
#endif