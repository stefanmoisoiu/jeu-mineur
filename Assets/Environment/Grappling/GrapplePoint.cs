using System;
using UnityEngine;
using UnityEngine.Events;

public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private Transform grapplePivotPoint;
    [SerializeField] private Transform grappleRopePoint;
    [SerializeField] private float targetGrappleDistance;
    [SerializeField] private float maxMoveAngle = 45f;
    
    public Transform GrapplePivotPoint => grapplePivotPoint;
    public Transform GrappleRopePoint => grappleRopePoint;
    public float TargetGrappleDistance => targetGrappleDistance;
    public float MaxMoveAngle => maxMoveAngle;
    
    public Action OnAttach, OnDetach;
    public UnityEvent UnityOnAttach, UnityOnDetach;

    public void Attach()
    {
        OnAttach?.Invoke();
        UnityOnAttach?.Invoke();
    }

    public void Detach()
    {
        OnDetach?.Invoke();
        UnityOnDetach?.Invoke();
    }
    private void OnDrawGizmos()
    {
        Vector3 startPos = GrapplePivotPoint.position;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(startPos, targetGrappleDistance);
        
        
        Quaternion rightMaxAngle = Quaternion.Euler(0,0,maxMoveAngle);
        Quaternion leftMaxAngle = Quaternion.Euler(0,0,-maxMoveAngle);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPos, startPos + rightMaxAngle * Vector2.down * targetGrappleDistance);
        Gizmos.DrawLine(startPos, startPos + leftMaxAngle * Vector2.down * targetGrappleDistance);
    }
}
