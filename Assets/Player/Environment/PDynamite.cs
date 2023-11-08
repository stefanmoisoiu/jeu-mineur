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
    
    [Header("Properties")]
    [SerializeField] private float detachJumpForce = 5;

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
        //if(Vector3.Distance(rb.position,_dynamite.AttachPoint.position) > 0.1f) Detach();
        rb.position = _dynamite.AttachPoint.position;
    }

    protected override void ActiveStateFixedUpdate()
    {
        //rb.velocity = _dynamite.GetCurrentVelocity();
    }

    protected override void OnStateExit()
    {
        _dynamite.OnExplode -= DynamiteExploded;
        inputManager.OnJump -= Detach;
        
        rb.gravityScale = _startGravityScale;
        rb.isKinematic = false;
    }

    private void CheckDynamite(Collider2D other)
    {
        if (!other.TryGetComponent(out Dynamite dynamite)) return;
        if(IsActiveState && _dynamite == dynamite) return;

        _dynamite = dynamite;
        stateManager.SetState(PStateManager.State.Dynamite);
    }

    private void DynamiteExploded()
    {
        rb.AddForce(_dynamite.GetVelocityExplosion(), ForceMode2D.Impulse);
        Detach();
    }
    private void Detach()
    {
        rb.AddForce(Vector2.up * detachJumpForce, ForceMode2D.Impulse);
        stateManager.SetState(PStateManager.State.Normal);
    }
}
