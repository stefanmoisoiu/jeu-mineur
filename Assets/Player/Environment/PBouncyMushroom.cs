using UnityEngine;

public class PBouncyMushroom : MovementState
{
    [Header("References")]
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private PPickaxe pickaxe;
    [SerializeField] private Rigidbody2D rb;
    
    
    [Header("Properties")]
    [SerializeField] private float uncontrolledBounceLength;
    [SerializeField] [Range(0,1)] private float bounceVelConservation = 0.5f;
    

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
        Debug.Log($"Bounce {velocity}");
        rb.velocity = other.transform.up * velocity;
        _uncontrolledBounceLength = uncontrolledBounceLength;
    }
    
    protected override void OnStateEnter()
    {
        _uncontrolledBounceLength = uncontrolledBounceLength;
        // rb.velocity = _force;
        pickaxe.ResetPickaxe();
    }

    protected override void ActiveStateUpdate()
    {
        _uncontrolledBounceLength -= Time.deltaTime;
        
        if (_uncontrolledBounceLength <= 0)
            stateManager.SetState(PStateManager.State.Normal);
    }
}