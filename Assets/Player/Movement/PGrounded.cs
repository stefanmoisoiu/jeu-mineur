using System;
using System.Linq;
using UnityEngine;

public class PGrounded : MonoBehaviour
{
    [Header("Ground Check Properties")]
    [SerializeField] private LayerMask notGroundLayer;
    [SerializeField] private LayerMask groundLayer;
    // [SerializeField] private float LRPosOffset = 0.25f;
    [SerializeField] private float groundStartCheckDistance = 0f;
    [SerializeField] private float airStartCheckDistance = -0.3f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private float closeGroundCheckDistance = 0.5f;
    // [SerializeField] private PStateManager stateManager;
    // [SerializeField] private PStateManager.State[] LRCheckStates;
    
    
    [SerializeField] private PDebug debug;
    private PDebug.DebugText _debugText;
    
    [Header("Debug")]
    [SerializeField] private bool showGizmos;
    
    public bool WasGrounded { get; private set; } = true;
    public bool IsGrounded { get; private set; }
    
    public bool WasGroundClose { get; private set; }
    public bool IsGroundClose { get; private set; }
    
    private RaycastHit2D _groundHit, _closeGroundHit,_notGroundHit;
    public RaycastHit2D NotGroundHit => _notGroundHit;
    public RaycastHit2D GroundHit => _groundHit;
    public RaycastHit2D CloseGroundHit => _closeGroundHit;
    
    public Action<bool,bool> OnGroundedChanged, OnGroundCloseChanged;

    private void Start()
    {
        _debugText = () => $"Grounded: {IsGrounded} | GroundClose: {IsGroundClose}";
        debug.AddDebugText(_debugText);
    }

    private void Update()
    {
        float checkStartDistance = IsGrounded ? groundStartCheckDistance : airStartCheckDistance;
        
        _notGroundHit = Physics2D.Raycast(transform.position + Vector3.down * checkStartDistance, Vector2.down, groundCheckDistance, notGroundLayer);
        _groundHit = Physics2D.Raycast(transform.position + Vector3.down * checkStartDistance, Vector2.down, groundCheckDistance, groundLayer);
        _closeGroundHit = Physics2D.Raycast(transform.position + Vector3.down * checkStartDistance, Vector2.down, closeGroundCheckDistance, groundLayer);

        // if (LRCheck())
        // {
        //     if (_groundHit.collider == null) _groundHit = Physics2D.Raycast(transform.position + Vector3.left * LRPosOffset + Vector3.down * checkDistance, Vector2.down, groundCheckDistance, groundLayer);
        //     if (_groundHit.collider == null) _groundHit = Physics2D.Raycast(transform.position + Vector3.right * LRPosOffset + Vector3.down * checkDistance, Vector2.down, groundCheckDistance, groundLayer);
        //
        //
        //     if (_closeGroundHit.collider == null) _closeGroundHit = Physics2D.Raycast(transform.position + Vector3.left * LRPosOffset + Vector3.down * checkDistance, Vector2.down, closeGroundCheckDistance, groundLayer);
        //     if (_closeGroundHit.collider == null) _closeGroundHit = Physics2D.Raycast(transform.position + Vector3.right * LRPosOffset + Vector3.down * checkDistance, Vector2.down, closeGroundCheckDistance, groundLayer);
        // }

        IsGrounded = _groundHit.collider != null && _notGroundHit.collider == null;
        IsGroundClose = _closeGroundHit.collider != null && _notGroundHit.collider == null;
        
        if (IsGrounded != WasGrounded)
            OnGroundedChanged?.Invoke(WasGrounded,IsGrounded);
        if (IsGroundClose != WasGroundClose)
            OnGroundCloseChanged?.Invoke(WasGroundClose,IsGroundClose);
        
        if(IsGrounded && _groundHit.collider.TryGetComponent(out PlayerGroundedEvents playerGroundedEvents))
            playerGroundedEvents.GroundedChanged(WasGrounded,IsGrounded);
        if(IsGroundClose && _closeGroundHit.collider.TryGetComponent(out PlayerGroundedEvents playerCloseGroundedEvents))
            playerCloseGroundedEvents.GroundCloseChanged(WasGrounded,IsGrounded);
    }

    // private bool LRCheck() => LRCheckStates.Contains(stateManager.CurrentState);
    private void LateUpdate()
    {
        WasGrounded = IsGrounded;
        WasGroundClose = IsGroundClose;
    }
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.green;
        float checkDistance = IsGrounded ? groundStartCheckDistance : airStartCheckDistance;
        
        Vector3 centerStartPos = transform.position + Vector3.down * checkDistance;
        // Vector3 leftStartPos = transform.position + Vector3.left * LRPosOffset + Vector3.down * checkDistance;
        // Vector3 rightStartPos = transform.position + Vector3.right * LRPosOffset + Vector3.down * checkDistance;
        
        Gizmos.DrawLine(centerStartPos, centerStartPos + Vector3.down * closeGroundCheckDistance);

        // if (LRCheck())
        // {
        //     Gizmos.DrawLine(leftStartPos, leftStartPos + Vector3.down * closeGroundCheckDistance);
        //     Gizmos.DrawLine(rightStartPos, rightStartPos + Vector3.down * closeGroundCheckDistance);
        // }
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(centerStartPos, centerStartPos + Vector3.down * groundCheckDistance);
        
    }
}