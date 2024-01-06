using System;
using UnityEngine;

public class PPipe : MovementState
{
    [Header("References")]
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PGrounded grounded;
    
    
    [SerializeField] private PMovement movement;
    [SerializeField] private PPickaxe pickaxe;
    
    
    
    private static readonly string StartPipeTag = "StartPipe";
    private static readonly string EndPipeTag = "EndPipe";
    
    private Pipe _currentPipe;

    [Header("Properties")]
    [SerializeField] private float moveSpeed = 20;
    [SerializeField] private float exitSpeed = 10;
    [SerializeField] private float exitVerticalSpeed = 2;
    [SerializeField] private float exitJumpVerticalSpeed = 5;
    [SerializeField] private float enterPipeCooldown = 0.2f;
    [SerializeField] private float highJumpBufferTime = 0.2f;
    private float _highJumpBufferTimer;
    
    private float _enterPipeCooldownTimer;
    
    [Header("Audio")]
    [SerializeField] private ScriptableSFX enterPipeSFX;
    [SerializeField] private ScriptableSFX exitPipeSFX;

    
    
    
    public Action OnEnterPipe, OnExitPipe;
    private void OnEnable()
    {
        colliderEvents.OnTriggerEnterValue += TryEnterPipe;
        inputManager.OnJump += TryLateHighJump;
    }

    private void OnDisable()
    {
        colliderEvents.OnTriggerEnterValue -= TryEnterPipe;
        inputManager.OnJump -= TryLateHighJump;

        if (_currentPipe != null)
        {
            _currentPipe.OnPipePositionChanged -= UpdatePipePosition;
            _currentPipe.OnPipeExited -= ExitPipe;
        }
    }

    private new void Update()
    {
        _enterPipeCooldownTimer -= Time.deltaTime;
        _highJumpBufferTimer -= Time.deltaTime;
        base.Update();
    }
    private void TryLateHighJump()
    {
        if (_highJumpBufferTimer <= 0) return;
        rb.velocity += Vector2.up * exitJumpVerticalSpeed;
        _highJumpBufferTimer = 0;
        movement.RemoveCoyoteTimer();
        grounded.UpdateGrounded();
    }

    protected override void OnStateEnter()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        col.enabled = false;
        enterPipeSFX.Play();
    }

    protected override void OnStateExit()
    {
        if (_currentPipe != null)
        {
            _currentPipe.OnPipePositionChanged -= UpdatePipePosition;
            _currentPipe.OnPipeExited -= ExitPipe;
            _currentPipe.StopMovingStretchPosition();
            _currentPipe = null;
        }
        pickaxe.ResetPickaxe();
        _enterPipeCooldownTimer = enterPipeCooldown;
        rb.isKinematic = false;
        col.enabled = true;
        exitPipeSFX.Play();
    }

    private void TryEnterPipe(Collider2D other)
    {
        if (IsActiveState) return;
        if (_enterPipeCooldownTimer > 0) return;
        if(!other.transform.parent.TryGetComponent(out Pipe pipe)) return;

        bool forward;
        
        if (other.CompareTag(StartPipeTag)) forward = true;
        else if (other.CompareTag(EndPipeTag)) forward = false;
        else return;
        
        _currentPipe = pipe;
        
        _currentPipe.OnPipePositionChanged += UpdatePipePosition;
        _currentPipe.OnPipeExited += ExitPipe;
        
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        col.enabled = false;
        
        _currentPipe.MoveStretchPosition(forward, moveSpeed);
        stateManager.SetState(PStateManager.State.Pipe);
        
        OnEnterPipe?.Invoke();
    }

    private void UpdatePipePosition(Vector2 position)
    {
        
    }
    private void ExitPipe(Vector2 exitPosition, Vector2 exitDirection)
    {
        rb.transform.position = exitPosition;
        rb.velocity = exitDirection * exitSpeed;
        rb.velocity += Vector2.up * (movement.JumpBufferTimer > 0 ? exitJumpVerticalSpeed : exitVerticalSpeed);
        
        _currentPipe.OnPipePositionChanged -= UpdatePipePosition;
        _currentPipe.OnPipeExited -= ExitPipe;
        
        if(movement.JumpBufferTimer <= 0) _highJumpBufferTimer = highJumpBufferTime;
        movement.RemoveCoyoteTimer();
        movement.RemoveBufferTimer();
        
        _currentPipe = null;
        
        stateManager.SetState(PStateManager.State.Normal);
        
        OnExitPipe?.Invoke();
    }
}