using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PUncontrollableFall : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PMovement movement;
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PAnimator animator;
    
    [Header("Properties")]
    [SerializeField] private float fallDistanceUncontrollable = 5f;
    [SerializeField] private float fallSpeed = 10;
    [SerializeField] private float sideCheckDistance = 0.5f;
    [SerializeField] private LayerMask sideCheckMask;
    
    
    [Header("Debug")]
    [SerializeField] private bool debug;
    
    private float _maxHeight;
    private float _startHVel;
    private bool _goingRight;
    public bool GoingRight => _goingRight;
    
    [Header("Animations")]
    [SerializeField] private string fallAnimation = "Fall";

    private void Start()
    {
        _maxHeight = transform.position.y;
    }
    private new void Update()
    {
        if(movement.IsFullyOnGround || transform.position.y > _maxHeight)
            _maxHeight = transform.position.y;
        base.Update();
    }
    public void TryUncontrollableFall(out bool success)
    {
        success = false;
        if(_maxHeight - transform.position.y < fallDistanceUncontrollable) return;
        success = true;
        stateManager.SetState(PStateManager.State.UncontrollableFall);
    }

    protected override void OnStateEnter()
    {
        grounded.OnGroundedChanged += TryStopUncontrollableFall;
        _startHVel = Mathf.Abs(rb.velocity.x);
        _goingRight = rb.velocity.x > 0;
        rb.isKinematic = true;
        rb.velocity = new Vector2(_startHVel * (_goingRight ? 1 : -1), -fallSpeed);
        animator.PlayAnimation(fallAnimation);
    }

    protected override void OnStateExit()
    {
        grounded.OnGroundCloseChanged -= TryStopUncontrollableFall;
        _maxHeight = transform.position.y;
        rb.isKinematic = false;
        // rb.velocity = Vector2.zero;
        // rb.position = grounded.CloseGroundHit.point + Vector2.up * 0.5f;
    }

    protected override void ActiveStateUpdate()
    {
        CheckHitWall();
    }

    private void TryStopUncontrollableFall(bool _, bool isGrounded)
    {
        if (isGrounded)
            stateManager.SetState(PStateManager.State.Normal);
    }

    private void CheckHitWall()
    {
        RaycastHit2D leftHit = LeftHit();
        RaycastHit2D rightHit = RightHit();

        if (leftHit.collider != null && !_goingRight)
        {
            _goingRight = true;
            rb.velocity = new Vector2(_startHVel, -fallSpeed);
        }
        else if (rightHit.collider != null && _goingRight)
        {
            _goingRight = false;
            rb.velocity = new Vector2(-_startHVel, -fallSpeed);
        }
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
