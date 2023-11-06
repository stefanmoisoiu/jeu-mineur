using UnityEngine;

public class PMovementTilt : MovementState
{
    [Header("References")]
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private Transform graphicsCenterPivot;
    [SerializeField] private Transform graphicsBottomPivot;
    

    [Header("Rotation Properties")]
    [SerializeField] private float tiltAngle = 10f;
    [SerializeField] private float tiltLerpSpeed = 10;
    [SerializeField] private float lerpBackSpeed = 10;

    protected override void ActiveStateUpdate()
    {
        float targetAngle = -inputManager.MoveInput * tiltAngle;
        graphicsBottomPivot.rotation = Quaternion.Lerp(graphicsBottomPivot.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * tiltLerpSpeed);
        graphicsCenterPivot.localRotation = Quaternion.Lerp(graphicsCenterPivot.localRotation, Quaternion.identity, Time.deltaTime * lerpBackSpeed);
    }
}
