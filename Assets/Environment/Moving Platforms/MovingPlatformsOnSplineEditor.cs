#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MovingPlatformsOnSpline))]
public class MovingPlatformsOnSplineEditor : Editor
{
    private float lastTimeSinceStartup;
    private void OnSceneGUI()
    {
        MovingPlatformsOnSpline movingPlatformsOnSpline = (MovingPlatformsOnSpline) target;
        float deltaTime = (float)EditorApplication.timeSinceStartup - lastTimeSinceStartup;
        lastTimeSinceStartup = (float)EditorApplication.timeSinceStartup;
        movingPlatformsOnSpline.MovePlatformEditor(deltaTime);
    }
}
#endif