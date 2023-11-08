using System;
using UnityEngine;

public class PRailing : MovementState
{
    [Header("References")]
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PPickaxe pickaxe;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PDebug debug;
    private PDebug.DebugText _debugText;
    
    
    [Header("Railing Detection")]
    [SerializeField] private float nearbyRailingRadius = 1f;
    [SerializeField] private LayerMask railingLayer;
    [SerializeField] private float railingAttachCooldown = 0.3f;
    private float _railingAttachCooldownTimer;
    
    
    private Railing _railing;
    
    [Header("Railing Properties")]
    [SerializeField] private float maxVel = 10f;
    [SerializeField] private float jumpVel = 10f;
    [SerializeField] [Range(0,1)] private float railingDrag = 0.8f;
    
    [Header("Debug")]
    [SerializeField] private bool showGizmos;
    
    private float _t;
    private float _internalVelocity;

    private void Start()
    {
        _debugText = () => $"Railing Vel: {Mathf.Round(_internalVelocity * 100) / 100}";
    }

    protected override void OnStateEnter()
    {
        if(_railing == null)
        {
            stateManager.SetState(PStateManager.State.Normal);
            return;
        }
        
        inputManager.OnJump += JumpAndDetach;
        inputManager.OnDownPress += DetachFromRailing;
        
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        
        debug.AddDebugText(_debugText);
        
    }
    protected override void OnStateExit()
    {
        rb.isKinematic = false;
        inputManager.OnJump -= JumpAndDetach;
        inputManager.OnDownPress -= DetachFromRailing;
        
        debug.RemoveDebugText(_debugText);
    }

    protected override void ActiveStateUpdate()
    {
        _internalVelocity += _railing.GetSpeed(_t,_internalVelocity) * Time.deltaTime;
        
        float railingMaxVel = _railing.WorldToRailingVelocity(maxVel);
        _internalVelocity = Mathf.Clamp(_internalVelocity, -railingMaxVel, railingMaxVel);
        
        _t += _internalVelocity;
        if (_t is > 1 or < 0)
        {
            if (_railing.DetachOnEnd)
            {
                DetachFromRailing();
                return;
            }
            else
            {
                _internalVelocity = 0;
                _t = Mathf.Clamp01(_t);
            }
        }
    }
    protected override void ActiveStateFixedUpdate()
    {
        _internalVelocity -= _internalVelocity * (1 - railingDrag);
        rb.MovePosition(_railing.GetPosition(_t));
    }

    private new void Update()
    {
        _railingAttachCooldownTimer -= Time.deltaTime;
        base.Update();
    }

    public void TryAttachToNearbyRailing(out bool success)
    {
        success = false;
        if (_railingAttachCooldownTimer > 0) return;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, nearbyRailingRadius, railingLayer);
        if (colliders.Length == 0) return;
        foreach (Collider2D col in colliders)
        {
            if (railingLayer != (railingLayer | (1 << col.gameObject.layer))) continue;
            if(!col.TryGetComponent(out Railing railing)) continue;
            
            Vector2 attachPoint = railing.GetPosition(railing.GetClosestPoint(transform.position));
            
            if (Vector2.Dot(rb.velocity.normalized, attachPoint - rb.position) < 0) continue;
            
            AttachToRailing(railing);
            
            success = true;
            return;
        }
    }
    public void AttachToRailing(Railing railing)
    {
        _railing = railing;
        _t = _railing.GetClosestPoint(transform.position);
        float addedVel = Vector2.Dot(rb.velocity.normalized, _railing.GetForward(_t)) * rb.velocity.magnitude;
        _internalVelocity = railing.WorldToRailingVelocity(addedVel);
        pickaxe.ResetPickaxe();
        stateManager.SetState(PStateManager.State.Railing);
    }
    public void DetachFromRailing()
    {
        if (_railing == null) return;
        _railingAttachCooldownTimer = railingAttachCooldown;
        
        rb.isKinematic = false;
        rb.velocity +=  _railing.RailingToWorldVelocity(_internalVelocity) * (Vector2)_railing.GetForward(_t);
        
        _railing = null;
        stateManager.SetState(PStateManager.State.Normal);
    }
    public void JumpAndDetach()
    {
        rb.isKinematic = false;
        rb.velocity = Vector2.up * jumpVel;
        
        DetachFromRailing();
    }

    private new void OnDrawGizmos()
    {
        if (!showGizmos) return;
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,nearbyRailingRadius);
    }
}