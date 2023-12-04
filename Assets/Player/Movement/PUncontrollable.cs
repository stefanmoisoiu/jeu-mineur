using UnityEngine;

public class PUncontrollable : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PMovement movement;
    [SerializeField] private PGrounded grounded;
    [SerializeField] private PAnimator animator;
    
    [Header("Properties")]
    [SerializeField] private float minHVel = 1f;

    
    [SerializeField] private float fallDistanceUncontrollable = 5f;
    [SerializeField] private float accelerationPerSecond = 3f;
    [SerializeField] private float maxFallSpeed = 30;
    [SerializeField] private float sideCheckDistance = 0.5f;
    [SerializeField] private LayerMask sideCheckMask;
    
    
    [Header("Debug")]
    [SerializeField] private bool debug;
    
    private float _maxHeight;
    private float _startHVel;
    private bool _goingRight;
    private float _currentFallSpeed;
    private float _startGravityScale;
    public bool GoingRight => _goingRight;
    
    [Header("Animations")]
    [SerializeField] private string fallAnimation = "Fall";

    private void Start()
    {
        _startGravityScale = rb.gravityScale;
        _maxHeight = transform.position.y;
    }
    private new void Update()
    {
        if(movement.IsFullyOnGround || transform.position.y > _maxHeight)
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
    public void TryUncontrollable(out bool success)
    {
        success = false;
        if(grounded.IsGrounded) return;
        success = true;
        
        StartUncontrollable();
    }
    public void StartUncontrollable()
    {
        _startHVel = Mathf.Max(minHVel,Mathf.Abs(rb.velocity.x));
        _goingRight = rb.velocity.x > 0;
        _currentFallSpeed = -rb.velocity.y;
        if(_currentFallSpeed > 0) _currentFallSpeed = Mathf.Min(maxFallSpeed,_currentFallSpeed);
        
        Debug.Log($"Uncontrollable Fall: {_startHVel} | {_goingRight} | {_currentFallSpeed}");
        
        stateManager.SetState(PStateManager.State.UncontrollableFall);
    }

    protected override void OnStateEnter()
    {
        grounded.OnGroundedChanged += TryStopUncontrollableFall;
        
        rb.gravityScale = 0;
        rb.velocity = new Vector2(_startHVel * (_goingRight ? 1 : -1), -_currentFallSpeed);
        animator.PlayAnimation(fallAnimation);
    }

    protected override void OnStateExit()
    {
        grounded.OnGroundedChanged -= TryStopUncontrollableFall;
        _maxHeight = transform.position.y;
        rb.gravityScale = _startGravityScale;
    }

    protected override void ActiveStateUpdate()
    {
        _currentFallSpeed += accelerationPerSecond * Time.deltaTime;
        _currentFallSpeed = Mathf.Min(maxFallSpeed,_currentFallSpeed);
        rb.velocity = new Vector2(_startHVel * (_goingRight ? 1 : -1), -_currentFallSpeed);
        
        CheckHitWall();
    }

    private void TryStopUncontrollableFall(bool wasGrounded, bool isGrounded)
    {
        if(_currentFallSpeed < 0) return;
        if(!isGrounded) return;
        stateManager.SetState(PStateManager.State.Normal);
    }

    private void CheckHitWall()
    {
        RaycastHit2D leftHit = LeftHit();
        RaycastHit2D rightHit = RightHit();

        if (leftHit.collider != null && !_goingRight)
        {
            _goingRight = true;
            rb.velocity = new Vector2(_startHVel, -maxFallSpeed);
        }
        else if (rightHit.collider != null && _goingRight)
        {
            _goingRight = false;
            rb.velocity = new Vector2(-_startHVel, -maxFallSpeed);
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
