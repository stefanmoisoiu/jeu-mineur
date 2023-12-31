using System;
using UnityEngine;

public class PMovement : MovementState
{
    [Header("References")] [SerializeField]
    private Rigidbody2D rb;

    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PRailing railing;
    [SerializeField] private PSlide slide;
    [SerializeField] private PSlipperyMovement slipperyMovement;
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PGroundStick groundStick;
    [SerializeField] private PGrappling grappling;
    [SerializeField] private PUncontrollable uncontrollable;
    [SerializeField] private PAnimator pAnimator;
    

    [Header("Movement Properties")] [SerializeField]
    private float speed = 2f;
    [SerializeField] [Range(0,1)] private float airTooFastSpeedMult = 0.5f;
    

    [SerializeField] [Range(0, 1)] private float airControl = 0.5f;
    [SerializeField] private float maxSpeed = 5f;
    public float MaxSpeed => maxSpeed;
    [SerializeField] [Range(0, 1)] private float drag = 1f;
    [SerializeField] [Range(0, 1)] private float airDragMult = 0.5f;
    [SerializeField] [Range(0, 1)] private float tooFastDragMult = 0.5f;


    [Header("Jumping Properties")] [SerializeField]
    private float jumpForce = 5f;
    [SerializeField] [Range(0,1)] private float jumpAddedVelMult = 0.5f;
    

    [SerializeField] [Range(0, 1)] private float jumpCooldownTime = 0.2f, jumpBufferTime = 0.1f, jumpCoyoteTime = 0.1f;
    private float _jumpBufferTimer, _jumpCoyoteTimer, _jumpCooldownTimer;
    public float JumpBufferTimer => _jumpBufferTimer;
    public bool CanJump => _jumpCooldownTimer <= 0;

    public bool IsFullyOnGround => grounded.IsGrounded && CanJump;
    public bool IsFullyOnCloseGround => grounded.IsGroundClose && CanJump;
    
    [Header("Fall")]
    [SerializeField] private float maxFallSpeed = 10f;
    [SerializeField] private float maxFasterFallSpeed = 10f;
    [SerializeField] private float fasterFallGravityMultiplier = 2f;
    
    [Header("Animations")]
    [SerializeField] private string airAnimationName = "Air";
    [SerializeField] private string idleAnimationName = "Idle";
    [SerializeField] private string runningAnimationName = "Running";

    [Header("Debug")] [SerializeField] private bool debug;

    public Action OnJump;
    private void OnEnable()
    {
        inputManager.OnJump += RestartBufferTimer;
        grounded.OnGroundedChanged += RestartCoyoteTimer;
    }

    private void OnDisable()
    {
        inputManager.OnJump -= RestartBufferTimer;
        grounded.OnGroundedChanged -= RestartCoyoteTimer;
        
        inputManager.OnJump -= Jump;
        inputManager.OnSecondaryAction -= TryGrapple;
    }

    protected override void OnStateEnter()
    {
        inputManager.OnJump += Jump;
        inputManager.OnSecondaryAction += TryGrapple;
    }

    protected override void OnStateExit()
    {
        inputManager.OnJump -= Jump;
        _jumpCooldownTimer = 0;
        inputManager.OnSecondaryAction -= TryGrapple;
    }
    private void TryGrapple() => grappling.TryGrapple(out _);

    private new void Update()
    {
        UpdateJumpTimers();
        base.Update();
    }

    protected override void ActiveStateUpdate()
    {
        if (grounded.IsGrounded && _jumpBufferTimer > 0)
        {
            Jump();
            return;
        }
        uncontrollable.TryFallUncontrollable(out bool uncontrollableFallSuccess);
        if (uncontrollableFallSuccess) return;
        
        slide.TryStartSlide(out bool slideSuccess);
        if (slideSuccess) return;
        
        slipperyMovement.TryStartSlipperyMovement(out bool slipperySuccess);
        if (slipperySuccess) return;
        
        railing.TryAttachToNearbyRailing(out bool railingSuccess);
        if (railingSuccess) return;
        
        pAnimator.PlayAnimation(IsFullyOnGround ? (inputManager.MoveInput != 0 ? runningAnimationName : idleAnimationName) : airAnimationName);
    }

    protected override void ActiveStateFixedUpdate()
    {
        Move();
        FallFaster();
        ClampFallSpeed();
    }
    
    #region Jumping
    private void UpdateJumpTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;
        _jumpCoyoteTimer -= Time.deltaTime;
        _jumpCooldownTimer -= Time.deltaTime;
    }
    public void RemoveCoyoteTimer() => _jumpCoyoteTimer = 0;
    private void RestartCoyoteTimer(bool wasGrounded, bool isGrounded)
    {
        if (!CanJump || isGrounded)
            return;
        _jumpCoyoteTimer = jumpCoyoteTime;
    }
    public void RemoveBufferTimer() => _jumpBufferTimer = 0;
    private void RestartBufferTimer()
    {
        if (_jumpCooldownTimer > 0 || _jumpCoyoteTimer <= 0)
            _jumpBufferTimer = jumpBufferTime;
    }
    private void Jump()
    {
        if (_jumpCooldownTimer > 0 || (_jumpCoyoteTimer <= 0 && !grounded.IsGrounded))
        {
            RestartBufferTimer();
            return;
        }

        _jumpCooldownTimer = jumpCooldownTime;
        _jumpBufferTimer = 0;
        _jumpCoyoteTimer = 0;
        
        float addedVel = Mathf.Max(0,rb.velocity.y) * jumpAddedVelMult;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce + addedVel);
        
        OnJump?.Invoke();
        PlayerMainEvents.OnPlayerJump?.Invoke();
    }

    #endregion

    #region Horizontal Movement

    #region Acceleration
    private float GetHorizontalAcceleration()
    {
        if (inputManager.MoveInput == 0) return 0;
        
        bool tooFast = Mathf.Abs(groundStick.HorizontalVelocity) > maxSpeed;
        bool movingAgainstVelocity = (int)Mathf.Sign(inputManager.MoveInput) != (int)Mathf.Sign(groundStick.HorizontalVelocity);
        
        float speedMult = IsFullyOnGround ? 1 : airControl;
        
        float targetAcceleration = 0;
        if (tooFast)
        {
            if(!movingAgainstVelocity) return 0;
            if(!IsFullyOnGround) speedMult *= airTooFastSpeedMult;
        } 
        targetAcceleration = inputManager.MoveInput * speed * speedMult;
        if (!tooFast) targetAcceleration = Mathf.Clamp(groundStick.HorizontalVelocity + targetAcceleration, -maxSpeed, maxSpeed) - groundStick.HorizontalVelocity;
        
        
        return targetAcceleration;
    }
    private float GetDrag()
    {
        float dragValue = -groundStick.HorizontalVelocity * drag * (IsFullyOnGround ? 1 : airDragMult) *
                          (Mathf.Abs(groundStick.HorizontalVelocity) > maxSpeed ? tooFastDragMult : 1);
        if (inputManager.MoveInput == 0 || Mathf.Abs(groundStick.HorizontalVelocity) > maxSpeed)
            return dragValue;
        return 0;
    }
    #endregion
    
    /// <summary>
    /// Moves the player according to the input
    /// </summary>
    private void Move()
    {
        float acceleration = GetHorizontalAcceleration() + GetDrag();
        
        if (IsFullyOnGround)
        {
            rb.velocity += groundStick.WorldRelativeVector(Vector2.right * acceleration);
        }
        else
        {
            rb.velocity += new Vector2( acceleration, 0);
        }
    }

    #endregion

    #region Falling

    private void ClampFallSpeed()
    {
        if (IsFullyOnGround) return;
        if(rb.velocity.y > 0) return;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -(IsFallingFaster() ? maxFasterFallSpeed : maxFallSpeed)));
    }
    private void FallFaster()
    {
        if (IsFullyOnGround) return;
        if(rb.velocity.y > 0) return;
        float velToAdd = (IsFallingFaster() ? 1 : 0) * (fasterFallGravityMultiplier-1) * Physics2D.gravity.y * Time.fixedDeltaTime;
        rb.velocity += Vector2.up * velToAdd;
    }
    private bool IsFallingFaster() => Vector2.Dot(Vector2.up * inputManager.Verticalnput, Vector2.down) > 0.5f;

    #endregion
    
    #region Debug
    protected override void ActiveStateOnDrawGizmos()
    {
        if (!debug) return;
        float size = 0.25f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.7f + Vector3.right * maxSpeed * size,
            transform.position + Vector3.up * 1.1f + Vector3.right * maxSpeed * size);
        Gizmos.DrawLine(transform.position + Vector3.up * 0.7f - Vector3.right * maxSpeed * size,
            transform.position + Vector3.up * 1.1f - Vector3.right * maxSpeed * size);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.8f,
            transform.position + Vector3.up * 0.8f + new Vector3(GetHorizontalAcceleration(), 0, 0) * size);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position + Vector3.up,
            transform.position + Vector3.up + Vector3.right * size * rb.velocity.x);
    }
    #endregion
}