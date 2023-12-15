using System;
using System.Collections;
using UnityEngine;

public class PPickaxeDash : MovementState
{
    [Header("References")]
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PPickaxe pickaxe;
    [SerializeField] private PWallStick wallStick;
    [SerializeField] private PRailing railing;
    // [SerializeField] private PWallBounce wallBounce;
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PGrappling grappling;
    // [SerializeField] private PFlipSprite flipSprite;
    [SerializeField] private PAnimator animator;
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Pickaxe Dash Properties")]
    [SerializeField] private float dashVel = 10f;
    [SerializeField] private AnimationCurve dashVelCurve;
    [SerializeField] private float dashLength = .75f;
    [SerializeField] [Range(0, 1)] private float minDashVelDot = 0.5f;
    [SerializeField] [Range(0, 1)] private float dashAddedVelMult = 0.5f;
    [SerializeField] private float startWallStickCheckTime = 0.2f;
    
    
    [Header("Animation Properties")]
    [SerializeField] private string dashAnimationName = "Pickaxe Dash";
    
    private float _startGravityScale;
    
    private Coroutine _dashCoroutine;
    
    public Action OnStartDash, OnEndDash;

    private Vector2 _dashDir;
    public Vector2 DashDir => _dashDir;

    private void Start()
    {
        _startGravityScale = rb.gravityScale;
    }

    private void OnDisable()
    {
        pickaxe.OnPickaxeUsed -= PickaxeDash;
        inputManager.OnSecondaryAction -= TryGrapple;
    }

    protected override void OnStateEnter()
    {
        pickaxe.OnPickaxeUsed += PickaxeDash;
        inputManager.OnSecondaryAction += TryGrapple;
    }
    protected override void OnStateExit()
    {
        if (_dashCoroutine != null) StopCoroutine(_dashCoroutine);
        pickaxe.OnPickaxeUsed -= PickaxeDash;
        rb.gravityScale = _startGravityScale;
        inputManager.OnSecondaryAction -= TryGrapple;
    }
    private void TryGrapple() => grappling.TryGrapple(out _);

    private IEnumerator DashCoroutine()
    {   
        railing.TryAttachToNearbyRailing(out bool startRailingSuccess);
        if (startRailingSuccess) yield break;
        
        
        _dashDir = inputManager.GetLookDirection();
        
        if (_dashDir == Vector2.zero)
        {
            stateManager.SetState(PStateManager.State.Normal);
            yield break;
        }
        
        OnStartDash?.Invoke();

        float dot = Mathf.Max(minDashVelDot, Vector2.Dot(rb.velocity.normalized, _dashDir));
        float addedVel = dot * rb.velocity.magnitude * dashAddedVelMult;
        
        animator.PlayAnimation(dashAnimationName);
        
        float timer = 0;
        while (timer < dashLength)
        {
            timer += Time.deltaTime;
            
            if (grounded.IsGrounded)
            {
                stateManager.SetState(PStateManager.State.Normal);
                yield break;
            }

            if (timer > startWallStickCheckTime)
            {
                wallStick.TryWallStick(out bool wallStickSuccess);
                if (wallStickSuccess) yield break;
            }
            
            railing.TryAttachToNearbyRailing(out bool railingSuccess);
            if (railingSuccess) yield break;
            
            rb.velocity = _dashDir * dashVelCurve.Evaluate(timer / dashLength) * dashVel + _dashDir * addedVel;
            rb.gravityScale = Mathf.Lerp(0,_startGravityScale,Mathf.Clamp01( timer / dashLength));
            
            yield return null;
        }
        OnEndDash?.Invoke();
        
        stateManager.SetState(PStateManager.State.Normal);
        rb.gravityScale = _startGravityScale;
    }
    private void PickaxeDash(int hitsRemaining)
    {
        if(grounded.IsGrounded) return;
        if (_dashCoroutine != null) StopCoroutine(_dashCoroutine);
        
        if(IsActiveState) stateManager.SetState(PStateManager.State.Normal);
        stateManager.SetState(PStateManager.State.PickaxeDash);
        _dashCoroutine = StartCoroutine(DashCoroutine());
    }
}