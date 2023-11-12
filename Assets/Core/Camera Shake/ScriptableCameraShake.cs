using UnityEngine;

[CreateAssetMenu(fileName = "Camera Shake", menuName = "Camera/CameraShake", order = 1)]
public class ScriptableCameraShake : ScriptableObject
{
    [SerializeField] private float duration, magnitude, speed;

    public void Shake()
    {
        CameraShake.MovementShake(duration, magnitude, speed);
    }
}