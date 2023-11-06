using System;
using UnityEngine;

public class PPickaxeDashTilt : MovementState
{
    [Header("References")]
    [SerializeField] private PPickaxeDash pickaxeDash;
    [SerializeField] private Transform graphicsCenterPivot;
    [SerializeField] private Transform graphicsBottomPivot;
    
    [Header("Rotation Properties")]
    [SerializeField] private float tiltLerpSpeed = 10;
    [SerializeField] private float lerpBackSpeed = 10;

    protected override void ActiveStateUpdate()
    {
        float targetAngle = Mathf.Atan2(pickaxeDash.DashDir.y, pickaxeDash.DashDir.x) * Mathf.Rad2Deg;
        if (targetAngle > 90) targetAngle -= 180;
        if (targetAngle < -90) targetAngle += 180;
        graphicsCenterPivot.rotation = Quaternion.Lerp(graphicsCenterPivot.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime * tiltLerpSpeed);
        graphicsBottomPivot.localRotation = Quaternion.Lerp(graphicsBottomPivot.localRotation, Quaternion.identity, Time.deltaTime * lerpBackSpeed);
    }
}
