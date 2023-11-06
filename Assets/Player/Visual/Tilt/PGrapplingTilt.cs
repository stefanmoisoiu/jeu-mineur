using UnityEngine;

public class PGrapplingTilt : MovementState
{
    [Header("References")]
    [SerializeField] private PGrappling grappling;
    [SerializeField] private Transform graphicsCenterPivot;
    [SerializeField] private Transform graphicsBottomPivot;
    
    [Header("Rotation Properties")]
    [SerializeField] private float tiltLerpSpeed = 10;
    [SerializeField] private float lerpBackSpeed = 10;
    

    protected override void ActiveStateUpdate()
    {
        Vector2 dir = grappling.GetPerpendicularDirectionToPoint();
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        graphicsCenterPivot.rotation = Quaternion.Lerp(graphicsCenterPivot.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * tiltLerpSpeed);
        graphicsBottomPivot.localRotation = Quaternion.Lerp(graphicsBottomPivot.localRotation, Quaternion.identity, Time.deltaTime * lerpBackSpeed);
    }
}
