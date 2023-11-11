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
    [SerializeField] private PPickaxe pickaxe;
    private Dynamite _dynamite;
    
    [Header("Detach Properties")]
    [SerializeField] private float detachJumpForce = 5;
    [SerializeField] private Vector2 checkSize;
    [SerializeField] private LayerMask detachLayer;
    [SerializeField] private float attachCooldown = 0.3f;
    private float _attachCooldown;
    
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
        _dynamite.OnExplode += Detach;
        inputManager.OnJump += Detach;
        
        _dynamite.Use();
        
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        
        rb.transform.position = _dynamite.AttachPoint.position;
        rb.isKinematic = true;
        
        pickaxe.ResetPickaxe();
    }
    protected override void OnStateExit()
    {
        _dynamite.OnExplode -= Detach;
        inputManager.OnJump -= Detach;
        
        rb.gravityScale = _startGravityScale;
        
        pickaxe.ResetPickaxe();
    }

    private new void Update()
    {
        _attachCooldown -= Time.deltaTime;
        base.Update();
    }
    protected override void ActiveStateUpdate()
    {
        rb.transform.position = _dynamite.AttachPoint.position;
        
        if(Physics2D.BoxCast(transform.position, checkSize, 0, Vector2.down, 0, detachLayer).collider != null)
            Detach();
    }

    private void CheckDynamite(Collider2D other)
    {
        if (_attachCooldown > 0) return;
        if (!other.TryGetComponent(out Dynamite dynamite)) return;
        if (IsActiveState && _dynamite == dynamite) return;
        if (Physics2D.BoxCast(transform.position, checkSize, 0, Vector2.down, 0, detachLayer).collider != null) return;

        _dynamite = dynamite;
        rb.transform.position = _dynamite.AttachPoint.position;
        stateManager.SetState(PStateManager.State.Dynamite);
    }
    private void Detach()
    {
        rb.isKinematic = false;
        rb.velocity = _dynamite.GetCurrentVelocity() + Vector2.up * detachJumpForce;
        _attachCooldown = attachCooldown;
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
