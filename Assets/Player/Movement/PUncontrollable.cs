using System;
using System.Linq;
using UnityEngine;

public class PUncontrollable : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PMovement movement;
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PGroundStick groundStick;
    [SerializeField] private PAnimator animator;
    [SerializeField] private PUnconscious unconscious;
    
    
    [Header("Start Fall Properties")]
    [SerializeField] private float fallDistanceUncontrollable = 10f;

    [SerializeField] private PStateManager.State[] resetFallHeightStates;
    private float _startUncontrollableHeight;
    
    [Header("Fall Properties")]
    [SerializeField] private float minFallHVel = 2f;
    [SerializeField] private float accelerationPerSecond = 10f;
    [SerializeField] private float maxFallSpeed = 35;
    [SerializeField] private float fallDistanceToUnconscious = 30f;
    
    
    [Header("Wall Bounce Properties")]
    [SerializeField] private float sideCheckDistance = 0.3f;
    [SerializeField] private LayerMask sideCheckMask;

    [Header("Start Slide Properties")]
    [SerializeField] private float slideMinAngle = 50;
    [SerializeField] [Range(0,1)] private float startSlideVelMult = 0.5f;
    
    [Header("Slide Properties")]
    [SerializeField] private float slideAcceleration = 10f;
    
    [Header("Stop Slide Properties")]
    [SerializeField] [Range(0,1)] private float stopSlideVelMult = 1f;
    
    [Header("Debug")]
    [SerializeField] private bool debug;
    
    private float _maxHeight;
    private float _startHVel;
    private bool _goingRight;
    private float _startGravityScale;
    public bool GoingRight => _goingRight;
    
    [Header("Animations")]
    [SerializeField] private string fallAnimation = "Fall";
    [SerializeField] private string slideAnimation = "Dead";

    public Action OnStartUncontrollable, OnStartSlide;
    
    private void Start()
    {
        _startGravityScale = rb.gravityScale;
        _maxHeight = transform.position.y;
    }
    private new void Update()
    {
        if(movement.IsFullyOnGround || transform.position.y > _maxHeight || resetFallHeightStates.Contains(stateManager.CurrentState))
            _maxHeight = transform.position.y;
        base.Update();
    }
    public void TryFallUncontrollable(out bool success)
    {
        success = false;
        if(grounded.IsGrounded) return;
        if(_maxHeight - transform.position.y < fallDistanceUncontrollable) return;
        success = true;
        
        StartUncontrollable();
    }
    public void StartUncontrollable()
    {
        _startHVel = Mathf.Max(minFallHVel,Mathf.Abs(rb.velocity.x));
        _goingRight = rb.velocity.x > 0;
        
        _startUncontrollableHeight = transform.position.y;
        
        stateManager.SetState(PStateManager.State.UncontrollableFall);
        OnStartUncontrollable?.Invoke();
    }

    private void TryStopUncontrollableFall(out bool success)
    {
        success = false;
        if (rb.velocity.y > 0) return;
        if (!grounded.IsGrounded) return;
        if (IsOnSlope()) return;
        
        success = true;
        
        if (Mathf.Abs(transform.position.y - _startUncontrollableHeight) > fallDistanceToUnconscious)
            stateManager.SetState(PStateManager.State.Unconscious);
        else
            stateManager.SetState(PStateManager.State.Normal);
    }

    protected override void OnStateEnter()
    {
        grounded.OnGroundedChanged += TryStartSlide;
        grounded.OnGroundedChanged += TryStopSlide;
        
        rb.gravityScale = 0;
        animator.PlayAnimation(fallAnimation);
    }
    protected override void ActiveStateUpdate()
    {
        TryStopUncontrollableFall(out bool success);
        if (success) return;
        
        CheckWallBounce();
        if(IsOnSlope()) UpdateSlideVelocity();
        else UpdateFallVelocity();
    }
    protected override void OnStateExit()
    {
        grounded.OnGroundedChanged -= TryStartSlide;
        grounded.OnGroundedChanged -= TryStopSlide;
        
        _maxHeight = transform.position.y;
        rb.gravityScale = _startGravityScale;
    }

    private void UpdateFallVelocity()
    {
        // X Vel
        rb.velocity = new Vector2(_startHVel * (_goingRight ? 1 : -1), rb.velocity.y);
        
        // Y Vel
        float fallSpeed = accelerationPerSecond * Time.deltaTime;
        rb.velocity += Vector2.down * fallSpeed;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
    }
    private void TryStartSlide(bool wasGrounded, bool isGrounded)
    {
        if(wasGrounded || !isGrounded) return;
        if (!IsOnSlope()) return;
        
        Vector3 vel = Vector2.right * rb.velocity.magnitude;
        rb.velocity = groundStick.WorldRelativeVector(grounded.GroundHit.normal.x > 0 ? vel : -vel);
        rb.velocity *= startSlideVelMult;
        
        animator.PlayAnimation(slideAnimation);
        
        OnStartSlide?.Invoke();
    }
    private void TryStopSlide(bool wasGrounded, bool isGrounded)
    {
        if(!wasGrounded || isGrounded) return;
        rb.velocity *= stopSlideVelMult;
        
        animator.PlayAnimation(fallAnimation);
        
        OnStartUncontrollable?.Invoke();
    }
    private void UpdateSlideVelocity()
    {
        float slideVel = rb.velocity.magnitude;
        slideVel += slideAcceleration * Time.deltaTime;
        
        Vector2 downSlopeVel = groundStick.WorldRelativeVector(grounded.GroundHit.normal.x > 0 ? Vector2.right : Vector2.left);
        
        rb.velocity = downSlopeVel * slideVel;
    }

    private void CheckWallBounce()
    {
        RaycastHit2D leftHit = LeftHit();
        RaycastHit2D rightHit = RightHit();

        if (leftHit.collider != null && !_goingRight) _goingRight = true;
        else if (rightHit.collider != null && _goingRight) _goingRight = false;
    }

    private bool IsOnSlope()
    {
        if(!grounded.IsGrounded) return false;
        float angle = Vector2.Angle(grounded.GroundHit.normal, Vector2.up);
        return angle > slideMinAngle;
    }

    private RaycastHit2D LeftHit() => Physics2D.Raycast(transform.position, Vector3.left, sideCheckDistance,sideCheckMask);
    private RaycastHit2D RightHit() => Physics2D.Raycast(transform.position, Vector3.right, sideCheckDistance,sideCheckMask);
    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!debug) return;
        Gizmos.color = Color.red;
        Vector3 dir = Vector3.right * sideCheckDistance;
        Gizmos.DrawLine(transform.position - dir, transform.position + dir);
    }
}
