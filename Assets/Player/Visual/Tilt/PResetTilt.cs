using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PResetTilt : MovementState
{
    [Header("References")]
    [SerializeField] private Transform graphicsCenterPivot;
    [SerializeField] private Transform graphicsBottomPivot;
    [Header("Properties")]
    [SerializeField] private float lerpBackSpeed = 5;
    
    protected override void ActiveStateUpdate()
    {
        graphicsCenterPivot.localRotation = Quaternion.Lerp(graphicsCenterPivot.localRotation, Quaternion.identity, Time.deltaTime * lerpBackSpeed);
        graphicsBottomPivot.localRotation = Quaternion.Lerp(graphicsBottomPivot.localRotation, Quaternion.identity, Time.deltaTime * lerpBackSpeed);
    }
}
