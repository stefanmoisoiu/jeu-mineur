using System;
using UnityEngine;

public class PWallStick : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PMovement movement;
    
    [SerializeField] private PAnimator animator;
    
    
    [Header("Wall Stick Properties")]
    [SerializeField] private float jumpHForce = 5f;
    [SerializeField] private float jumpForce = 10f;
    
    [Header("Can Wall Stick Properties")]
    [SerializeField] private float wallStickCheckMaxDistance = 0.5f;
    [SerializeField] private float playerRadius = 0.5f;
    [SerializeField] private LayerMask wallStickLayerMask;
    [SerializeField] private float angleRange = 15f;
    private int _wallSticklookDirection;
    public int WallStickLookDirection => _wallSticklookDirection;
    
    [Header("Animations")]
    [SerializeField] private string wallStickAnimationName = "WallStick";

    
    
    [Header("Debug")]
    [SerializeField] private bool showGizmos;
    
    private Transform _wallTransform;
    
    public Action<Transform,Vector3> OnWallStick;
    public Action OnWallUnStick;
    
    protected override void OnStateEnter()
    {
        inputManager.OnJump += UnStick;
        if(grounded.IsGrounded) stateManager.SetState(PStateManager.State.Normal);
        else if(movement.JumpBufferTimer > 0) UnStick();
    }
    protected override void OnStateExit()
    {
        rb.isKinematic = false;
        OnWallUnStick?.Invoke();
        inputManager.OnJump -= UnStick;
    }
    private void UnStick()
    {
        rb.isKinematic = false;
        rb.velocity = new Vector2(_wallSticklookDirection * jumpHForce, jumpForce);
        stateManager.SetState(PStateManager.State.Normal);
        
        if (_wallTransform == null) return;
        if (_wallTransform.TryGetComponent(out PlayerWallStickEvents wallStickEvents))
            wallStickEvents.WallUnStick();
        _wallTransform = null;
    }
    private bool WallStickLeft(out RaycastHit2D raycastHit)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, wallStickCheckMaxDistance, wallStickLayerMask);
        raycastHit = hit;
        return hit.collider != null && Vector2.Angle(hit.normal, Vector2.right) < angleRange;
    }
    private bool WallStickRight(out RaycastHit2D raycastHit)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, wallStickCheckMaxDistance, wallStickLayerMask);
        raycastHit = hit;
        return hit.collider != null && Vector2.Angle(hit.normal, Vector2.left) < angleRange;
    }
    public void TryWallStick(out bool success)
    {
        success = false;
        if (stateManager.CurrentState == PStateManager.State.WallStick) return;
        
        bool leftSuccess = WallStickLeft(out RaycastHit2D leftHit);
        bool rightSuccess = WallStickRight(out RaycastHit2D rightHit);
        if(!leftSuccess && !rightSuccess) return;
        
        _wallSticklookDirection = leftSuccess ? 1 : -1;
        if ((int)Mathf.Sign(rb.velocity.x) == (int)Mathf.Sign(_wallSticklookDirection)) return;
        
        success = true;
        
        animator.PlayAnimation(wallStickAnimationName);
        
        stateManager.SetState(PStateManager.State.WallStick);
        
        Vector2 wallPosition = leftSuccess ? leftHit.point : rightHit.point;
        _wallTransform = leftSuccess ? leftHit.collider.transform : rightHit.collider.transform;
        
        OnWallStick?.Invoke(_wallTransform, wallPosition);
        if (_wallTransform.TryGetComponent(out PlayerWallStickEvents wallStickEvents))
            wallStickEvents.WallStick();
        
        rb.position = wallPosition + _wallSticklookDirection * Vector2.right * playerRadius;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    protected override void ActiveStateUpdate()
    {
        if(grounded.IsGrounded)
            stateManager.SetState(PStateManager.State.Normal);
        
        WallStickLeft(out RaycastHit2D leftHit);
        bool leftTransformIsWall = leftHit.collider != null && leftHit.collider.transform == _wallTransform;
        WallStickRight(out RaycastHit2D rightHit);
        bool rightTransformIsWall = rightHit.collider != null && rightHit.collider.transform == _wallTransform;
        if (!leftTransformIsWall && !rightTransformIsWall)
            stateManager.SetState(PStateManager.State.Normal);
    }
    
    private new void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.cyan;
        Vector3 position = transform.position;
        Gizmos.DrawLine(position + Vector3.left * wallStickCheckMaxDistance, position + Vector3.right * wallStickCheckMaxDistance);
        base.OnDrawGizmos();
    }
}