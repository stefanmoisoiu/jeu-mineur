using UnityEngine;

public class PBouncyMushroom : MovementState
{
    [Header("References")]
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private PPickaxe pickaxe;
    [SerializeField] private PAnimator animator;
    
    [SerializeField] private Rigidbody2D rb;
    
    
    [Header("Properties")]
    [SerializeField] private float uncontrolledBounceLength;
    [SerializeField] [Range(0,1)] private float bounceVelConservation = 0.5f;
    [SerializeField] private string bounceAnimationName = "Bounce";
    [SerializeField] private float maxProjectedVelocity = 3f;
    
    

    private float _uncontrolledBounceLength;


    private void Start()
    {
        colliderEvents.OnTriggerEnterValue += TryBounce;
    }

    private void TryBounce(Collider2D other)
    {
        if (IsActiveState) return;
        if(!other.gameObject.TryGetComponent(out BouncyMushroom bouncyMushroom)) return;
        stateManager.SetState(PStateManager.State.BouncyMushroom);

        float velocity = Mathf.Sqrt(rb.velocity.magnitude) * bounceVelConservation + bouncyMushroom.Force;

        Vector2 direction = other.transform.up;
        Vector2 addedVelocity = bouncyMushroom.AdditiveForce ? Vector3.ProjectOnPlane(rb.velocity, direction) : Vector2.zero;
        addedVelocity = Vector2.ClampMagnitude(addedVelocity, maxProjectedVelocity);
        rb.velocity = direction * velocity + addedVelocity;
        _uncontrolledBounceLength = uncontrolledBounceLength;
    }
    
    protected override void OnStateEnter()
    {
        _uncontrolledBounceLength = uncontrolledBounceLength;
        animator.PlayAnimation(bounceAnimationName);
        pickaxe.ResetPickaxe();
    }

    protected override void ActiveStateUpdate()
    {
        _uncontrolledBounceLength -= Time.deltaTime;
        
        if (_uncontrolledBounceLength <= 0)
            stateManager.SetState(PStateManager.State.Normal);
    }
}