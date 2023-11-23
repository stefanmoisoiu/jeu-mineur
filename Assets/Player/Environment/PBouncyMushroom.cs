using UnityEngine;

public class PBouncyMushroom : MovementState
{
    [Header("References")]
    [SerializeField] private ColliderEvents colliderEvents;
    [SerializeField] private PPickaxe pickaxe;
    [SerializeField] private Rigidbody2D rb;
    
    
    [Header("Properties")]
    [SerializeField] private float uncontrolledBounceLength;

    private float _uncontrolledBounceLength;


    private void Start()
    {
        colliderEvents.OnTriggerEnterValue += TryBounce;
    }

    private void TryBounce(Collider2D other)
    {
        if(!other.gameObject.TryGetComponent(out BouncyMushroom bouncyMushroom)) return;
        stateManager.SetState(PStateManager.State.BouncyMushroom);
        rb.velocity = other.transform.up * bouncyMushroom.Force;
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