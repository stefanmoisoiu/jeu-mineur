using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PGrappling : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PPickaxe pickaxe;
    [SerializeField] private PAnimator animator;
    
    private GrapplePoint _attachedGrapplePoint;
    public GrapplePoint AttachedGrapplePoint => _attachedGrapplePoint;
    [SerializeField] private PDebug debug;
    private PDebug.DebugText _debugText;
    
    [Header("Grappling Detection Properties")]
    [SerializeField] private float detectionRadius = 15f;
    [SerializeField] private LayerMask grappleObstacleCheckMask;
    
    [Header("Start Grappling Properties")]
    [SerializeField] private float startVelocityMult = 3f;
    
    [Header("Grappling Properties")]
    [SerializeField] private FloatSpringComponent grapplingDistanceSpring;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityScale = 0.5f;
    [SerializeField] [Range(0,1)] private float damping = 0.95f;
    [SerializeField] [Range(0,1)] private float angleTooLargeDamping = 0.95f;
    
    [Header("Detaching")]
    [SerializeField] private float detachJumpForce = 10f;
    [SerializeField] private float detachVelocityMult = 1.25f;
    
    [Header("Animation")]
    [SerializeField] private float stopAnimationVelocity = 1f;
    [SerializeField] private string accelerationAnimationName = "Grappling Acceleration";
    [SerializeField] private string decelerationAnimationName = "Grappling Deceleration";
    
    
    
    private float _horizontalVelocity;
    private float _verticalVelocity;
    
    public float HorizontalVelocity => _horizontalVelocity;
    public float VerticalVelocity => _verticalVelocity;
    
    private float _baseRigidbodyGravityScale;
    
    [Header("Debug")]
    [SerializeField] private bool showGizmos;

    private void Start()
    {
        _debugText = () => $"Grappling HVel: {Mathf.Round(_horizontalVelocity * 100) / 100} | VVel: {Mathf.Round(_verticalVelocity * 100) / 100}";
    }

    protected override void OnStateEnter()
    {
        rb.gravityScale = 0;
        inputManager.OnJump += Detach;
        inputManager.OnSecondaryAction += Detach;
        debug.AddDebugText(_debugText);
    }

    protected override void OnStateExit()
    {
        rb.gravityScale = _baseRigidbodyGravityScale;
        inputManager.OnJump -= Detach;
        inputManager.OnSecondaryAction -= Detach;
        debug.RemoveDebugText(_debugText);
    }
    protected override void ActiveStateFixedUpdate()
    {
        GrapplePoint[] grapplePoints = GetGrapplePoints();
        if(!grapplePoints.Contains(AttachedGrapplePoint))
        {
            Detach();
            return;
        }
        
        float previousHorizontalVelocity = _horizontalVelocity;
        _horizontalVelocity += GetGravityForce();
        _horizontalVelocity += GetDampingForce();
        _horizontalVelocity += GetHorizontalAcceleration();
        
        _verticalVelocity += GetSpringForce();
        
        rb.velocity = GetPerpendicularDirectionToPoint() * _horizontalVelocity + GetDirectionToPoint() * _verticalVelocity;
        
        if(Mathf.Abs(_horizontalVelocity) > stopAnimationVelocity) animator.PlayAnimation(Mathf.Abs(_horizontalVelocity) - Mathf.Abs(previousHorizontalVelocity) > 0 ? accelerationAnimationName : decelerationAnimationName);
    }

    private float GetSpringForce()
    {
        grapplingDistanceSpring.target = _attachedGrapplePoint.TargetGrappleDistance;
        grapplingDistanceSpring.currentPosition = Vector2.Distance(transform.position, _attachedGrapplePoint.GrapplePivotPoint.position);
        float force = grapplingDistanceSpring.UpdateSpring(Time.fixedDeltaTime);
        
        return -force;
    }
    private float GetHorizontalAcceleration()
    {
        Vector2 direction = GetDirectionToPoint();
        
        float angle = Vector2.Angle(direction, Vector2.down);
        float angleAdvancement = Mathf.Clamp01(angle / _attachedGrapplePoint.MaxMoveAngle);
        
        float acceleration = angleAdvancement * inputManager.MoveInput * moveSpeed * Time.fixedDeltaTime;
        
        return acceleration;
    }
    private float GetGravityForce()
    {
        Vector2 perpendicularDirection = GetPerpendicularDirectionToPoint();
        float gravityForce = Vector2.Dot(perpendicularDirection, Physics2D.gravity) * gravityScale * Time.fixedDeltaTime;
        return gravityForce;
    }
    private float GetDampingForce()
    {
        Vector2 direction = GetDirectionToPoint();
        
        float angle = Vector2.Angle(direction, Vector2.down);
        float angleAdvancement = Mathf.Clamp01(angle / _attachedGrapplePoint.MaxMoveAngle);
        
        float targetDamping = angleAdvancement > 1 ? angleTooLargeDamping : damping;
        
        return -_horizontalVelocity * (1 - targetDamping);
    }
    
    private void Detach()
    {
        _attachedGrapplePoint.Detach();
        
        rb.gravityScale = _baseRigidbodyGravityScale;
        
        rb.velocity *= detachVelocityMult;
        rb.velocity += Vector2.up * detachJumpForce;
        
        stateManager.SetState(PStateManager.State.Normal);
    }
    
    public Vector2 GetDirectionToPoint() => (_attachedGrapplePoint.GrapplePivotPoint.position - transform.position).normalized;
    public Vector2 GetPerpendicularDirectionToPoint() => Quaternion.Euler(0,0,-90) * GetDirectionToPoint();
    public void TryGrapple(out bool success)
    {
        success = false;

        if (IsActiveState) return;
        
        GrapplePoint[] availableGrapplePoints = GetGrapplePoints();
        if (availableGrapplePoints.Length == 0) return;
        
        success = true;
        
        _attachedGrapplePoint = GetClosestGrapplePoint(availableGrapplePoints);
        _attachedGrapplePoint.Attach();
        
        pickaxe.ResetPickaxe();
        
        Vector3 perpendicularDirection = GetPerpendicularDirectionToPoint();
        float velocityConversion = Vector2.Dot(perpendicularDirection, rb.velocity.normalized);
        _horizontalVelocity = startVelocityMult * rb.velocity.magnitude * Mathf.Sign(velocityConversion);
        
        stateManager.SetState(PStateManager.State.Grappling);
    }
    private GrapplePoint[] GetGrapplePoints()
    {
        List<GrapplePoint> grapplePoints = new();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius,grappleObstacleCheckMask);
        foreach (Collider2D collider in colliders)
        {
            if(!collider.TryGetComponent(out GrapplePoint grapplePoint)) continue;
            
            Vector2 direction = grapplePoint.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius,grappleObstacleCheckMask);
            
            if (hit.collider == null) continue;
            if (hit.collider.gameObject != grapplePoint.gameObject) continue;
            grapplePoints.Add(grapplePoint);
        }
        return grapplePoints.ToArray();
    }
    private GrapplePoint GetClosestGrapplePoint(GrapplePoint[] grapplePoints)
    {
        if(grapplePoints.Length == 0) return null;
        float closestDistance = float.MaxValue;
        GrapplePoint closestGrapplePoint = null;
        foreach (GrapplePoint grapplePoint in grapplePoints)
        {
            float distance = Vector2.Distance(transform.position, grapplePoint.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGrapplePoint = grapplePoint;
            }
        }
        return closestGrapplePoint;
    }
    private new void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    protected override void ActiveStateOnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, GetPerpendicularDirectionToPoint() * _horizontalVelocity);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, GetDirectionToPoint() * _verticalVelocity);
    }
}