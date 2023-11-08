using System.Collections;
using UnityEngine;

public class PWallBounce : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PAnimator animator;
    
    
    [Header("Bounce Check Properties")]
    [SerializeField] private float bounceCheckMaxDistance = 0.6f;
    [SerializeField] private LayerMask bounceCheckLayerMask;
    [SerializeField] private float bounceForce = 10f;
    
    
    [Header("Bounce Properties")]
    [SerializeField] private float bounceUncontrollableTime = 0.5f;
    
    [Header("Animation Properties")]
    [SerializeField] private string bounceAnimationName = "Wall Bounce";
    
    
    [Header("Debug")]
    [SerializeField] private bool showGizmos;
    
    private Coroutine _bounceCoroutine;

    protected override void OnStateEnter()
    {
        animator.PlayAnimation(bounceAnimationName);
        _bounceCoroutine = StartCoroutine(BounceCoroutine());
    }
    protected override void OnStateExit()
    {
        if(_bounceCoroutine != null) StopCoroutine(_bounceCoroutine);
    }
    private IEnumerator BounceCoroutine()
    {
        yield return new WaitForSeconds(bounceUncontrollableTime);
        stateManager.SetState(PStateManager.State.Normal);
    }

    public void TryBounce(out bool success)
    {
        success = false;
        
        bool leftSuccess = BounceLeft(out RaycastHit2D leftHit);
        bool rightSuccess = BounceRight(out RaycastHit2D rightHit);
        if(!leftSuccess && !rightSuccess) return;
        
        float jumpDirection = leftSuccess ? 1 : -1;
        if (rb.velocity.x * jumpDirection > 0) return;
        success = true;

        float bounce = bounceForce;
        if(leftSuccess && leftHit.transform.TryGetComponent(out WallBounce wallBounceLeft))
            bounce *= wallBounceLeft.BounceForceMult;
        else if (rightSuccess && rightHit.transform.TryGetComponent(out WallBounce wallBounceRight))
            bounce *= wallBounceRight.BounceForceMult;

        Vector2 force = new Vector2(jumpDirection * 0.5f,1).normalized * bounce;
        stateManager.SetState(PStateManager.State.WallBounce);
        rb.velocity = force;
    }
    private bool BounceLeft(out RaycastHit2D raycastHit)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, bounceCheckMaxDistance, bounceCheckLayerMask);
        raycastHit = hit;
        return hit.collider != null;
    }
    private bool BounceRight(out RaycastHit2D raycastHit)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, bounceCheckMaxDistance, bounceCheckLayerMask);
        raycastHit = hit;
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.left * bounceCheckMaxDistance);
        Gizmos.DrawRay(transform.position, Vector2.right * bounceCheckMaxDistance);
    }
}