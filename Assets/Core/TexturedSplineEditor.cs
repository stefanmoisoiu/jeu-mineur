#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TexturedSpline))]
public class TexturedSplineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TexturedSpline spline = (TexturedSpline) target;
        if (GUILayout.Button("Rebuild"))
        {
            spline.Rebuild();
        }
    }
}
#endif