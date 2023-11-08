﻿using System;
using UnityEngine;

public class PGrounded : MonoBehaviour
{
    [Header("Ground Check Properties")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.25f;
    [SerializeField] private float closeGroundCheckDistance = 0.5f;
    [SerializeField] private PDebug debug;
    private PDebug.DebugText _debugText;
    
    [Header("Debug")]
    [SerializeField] private bool showGizmos;
    
    public bool WasGrounded { get; private set; }
    public bool IsGrounded { get; private set; }
    
    public bool WasGroundClose { get; private set; }
    public bool IsGroundClose { get; private set; }
    
    private RaycastHit2D _groundHit, _closeGroundHit;
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
        _groundHit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        _closeGroundHit = Physics2D.Raycast(transform.position, Vector2.down, closeGroundCheckDistance, groundLayer);
        
        IsGrounded = _groundHit.collider != null;
        IsGroundClose = _closeGroundHit.collider != null;
        
        if (IsGrounded != WasGrounded)
            OnGroundedChanged?.Invoke(WasGrounded,IsGrounded);
        if (IsGroundClose != WasGroundClose)
            OnGroundCloseChanged?.Invoke(WasGroundClose,IsGroundClose);
        
        if(IsGrounded && _groundHit.collider.TryGetComponent(out PlayerGroundedEvents playerGroundedEvents))
            playerGroundedEvents.GroundedChanged(WasGrounded,IsGrounded);
        if(IsGroundClose && _closeGroundHit.collider.TryGetComponent(out PlayerGroundedEvents playerCloseGroundedEvents))
            playerCloseGroundedEvents.GroundCloseChanged(WasGrounded,IsGrounded);
    }

    private void LateUpdate()
    {
        WasGrounded = IsGrounded;
        WasGroundClose = IsGroundClose;
    }
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * closeGroundCheckDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}