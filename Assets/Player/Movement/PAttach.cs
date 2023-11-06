using System;
using UnityEngine;

public class PAttach : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PPickaxeDash pickaxeDash;
    [SerializeField] private PDebug debug;
    private PDebug.DebugText _debugText;
    
    
    private Transform attachedObject;
    private Vector3 previousAttachedObjectPosition;
    private Vector2 attachedObjectVelocity,previousAttachedObjectVelocity;

    private void Start()
    {
        grounded.OnGroundedChanged += delegate(bool wasGrounded, bool isGrounded)
        {
            if (wasGrounded && !isGrounded) Detach();
            if (!wasGrounded && isGrounded) Attach(grounded.GroundHit.transform);
        };
        pickaxeDash.OnStartDash += Detach;
        pickaxeDash.OnEndDash += Detach;
        
        _debugText = () => $"Attached Object: {(attachedObject != null ? attachedObject.name : "None")}";
    }

    public void Attach(Transform newAttachedObject)
    {
        if (attachedObject == null) debug.AddDebugText(_debugText);
        
        attachedObject = newAttachedObject;
        previousAttachedObjectPosition = attachedObject.position;
        attachedObjectVelocity = Vector3.zero;
        previousAttachedObjectVelocity = Vector3.zero;
    }
    public void Detach()
    {
        if (attachedObject != null)
        {
            Vector2 velocityToAdd = (Vector2)(attachedObject.position - previousAttachedObjectPosition) / Time.fixedDeltaTime;
            rb.velocity += velocityToAdd;
        }
        attachedObject = null;
        debug.RemoveDebugText(_debugText);
    }

    private void FixedUpdate()
    {
        if (attachedObject == null) return;
        if (attachedObject != grounded.GroundHit.transform) Attach(grounded.GroundHit.transform);
        
        attachedObjectVelocity = (attachedObject.position - previousAttachedObjectPosition) / Time.fixedDeltaTime;
        previousAttachedObjectPosition = attachedObject.position;
        
        Vector2 attachedObjectAcceleration = (attachedObjectVelocity - previousAttachedObjectVelocity) / Time.fixedDeltaTime;
        
        previousAttachedObjectVelocity = attachedObjectVelocity;
        
        Debug.DrawRay(attachedObject.position, attachedObjectAcceleration, Color.red);
        Debug.DrawRay(attachedObject.position, attachedObjectVelocity, Color.green);
        
        rb.position += attachedObjectVelocity * Time.fixedDeltaTime;
    }
}
