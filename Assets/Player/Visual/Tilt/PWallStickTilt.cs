using UnityEngine;

public class PWallStickTilt : MovementState
{
    [SerializeField] private Transform graphicsCenterPivot, graphicsBottomPivot;
    
    protected override void OnStateEnter()
    {
        graphicsCenterPivot.localRotation = Quaternion.identity;
        graphicsBottomPivot.localRotation = Quaternion.identity;
    }
}
