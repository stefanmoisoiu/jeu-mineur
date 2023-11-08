using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDynamite : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private PInputManager inputManager;
    private Dynamite _dynamite;
    
    [Header("Detach Properties")]
    [SerializeField] private float detachJumpForce = 5;
    [SerializeField] private Vector2 checkSize;
    [SerializeField] private LayerMask detachLayer;
    
    [Header("Debug")] [SerializeField]
    private bool debug;

    

    private float _startGravityScale;

    private void Start()
    {
        _startGravityScale = rb.gravityScale;
        colliderEvents.OnTriggerEnterValue += CheckDynamite;
    }

    protected override void OnStateEnter()
    {
        _dynamite.OnExplode += DynamiteExploded;
        inputManager.OnJump += Detach;
        
        _dynamite.Use();
        
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        
        rb.transform.position = _dynamite.AttachPoint.position;
        rb.isKinematic = true;
    }

    protected override void ActiveStateUpdate()
    {
        rb.transform.position = _dynamite.AttachPoint.position;
        
        if(Physics2D.BoxCast(transform.position, checkSize, 0, Vector2.down, 0, detachLayer))
            Detach();
    }

    protected override void OnStateExit()
    {
        _dynamite.OnExplode -= DynamiteExploded;
        inputManager.OnJump -= Detach;
        
        rb.gravityScale = _startGravityScale;
    }

    private void CheckDynamite(Collider2D other)
    {
        if (!other.TryGetComponent(out Dynamite dynamite)) return;
        if(IsActiveState && _dynamite == dynamite) return;

        _dynamite = dynamite;
        rb.transform.position = _dynamite.AttachPoint.position;
        stateManager.SetState(PStateManager.State.Dynamite);
    }

    private void DynamiteExploded()
    {
        rb.isKinematic = false;
        rb.velocity += _dynamite.GetCurrentVelocity();
        Detach();
    }
    private void Detach()
    {
        rb.isKinematic = false;
        rb.velocity += Vector2.up * detachJumpForce;
        stateManager.SetState(PStateManager.State.Normal);
    }

    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if(!debug) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, checkSize);
    }
}
