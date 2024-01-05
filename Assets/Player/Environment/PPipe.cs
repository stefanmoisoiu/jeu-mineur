using System;
using UnityEngine;

public class PPipe : MovementState
{
    [Header("References")]
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private Rigidbody2D rb;
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
    private float _enterPipeCooldownTimer;
    
    
    public Action OnEnterPipe, OnExitPipe;
    private void OnEnable()
    {
        colliderEvents.OnTriggerEnterValue += TryEnterPipe;
    }

    private void OnDisable()
    {
        colliderEvents.OnTriggerEnterValue -= TryEnterPipe;

        if (_currentPipe != null)
        {
            _currentPipe.OnPipePositionChanged -= UpdatePipePosition;
            _currentPipe.OnPipeExited -= ExitPipe;
        }
    }

    private new void Update()
    {
        _enterPipeCooldownTimer -= Time.deltaTime;
        base.Update();
    }

    protected override void OnStateEnter()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    protected override void OnStateExit()
    {
        pickaxe.ResetPickaxe();
        _enterPipeCooldownTimer = enterPipeCooldown;
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
        
        
        _currentPipe.MoveStretchPosition(forward, moveSpeed);
        stateManager.SetState(PStateManager.State.Pipe);
        
        OnEnterPipe?.Invoke();
    }

    private void UpdatePipePosition(Vector2 position)
    {
        Debug.Log(position);
    }
    private void ExitPipe(Vector2 exitPosition, Vector2 exitDirection)
    {
        rb.transform.position = exitPosition;
        rb.position = exitPosition;
        rb.isKinematic = false;
        
        rb.velocity = exitDirection * exitSpeed;
        rb.velocity += Vector2.up * (movement.JumpBufferTimer > 0 ? exitJumpVerticalSpeed : exitVerticalSpeed);
        
        _currentPipe.OnPipePositionChanged -= UpdatePipePosition;
        _currentPipe.OnPipeExited -= ExitPipe;
        
        _currentPipe = null;
        
        stateManager.SetState(PStateManager.State.Normal);
        
        OnExitPipe?.Invoke();
    }
}