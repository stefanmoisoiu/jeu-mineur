using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUnconsciousTilt : MovementState
{
    [Header("References")]
    [SerializeField] private PGrounded grounded;
    [SerializeField] private Transform graphicsCenterPivot;
    [SerializeField] private Transform graphicsBottomPivot;

    protected override void ActiveStateUpdate()
    {
        if (grounded.IsGrounded)
        {
            graphicsCenterPivot.localRotation = Quaternion.identity;
            graphicsBottomPivot.up = grounded.GroundHit.normal;
        }
        else
        {
            graphicsCenterPivot.localRotation = Quaternion.identity;
            graphicsBottomPivot.localRotation = Quaternion.identity;
        }
    }
}
