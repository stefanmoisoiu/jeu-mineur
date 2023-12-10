using UnityEngine;

public class PGravity : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PMovement pMovement;
    [SerializeField] private PGrounded grounded;
    
    
    private float _startGravityScale;

    private void Start()
    {
        _startGravityScale = rb.gravityScale;
    }

    protected override void OnStateExit()
    {
        rb.gravityScale = _startGravityScale;
    }

    protected override void ActiveStateUpdate()
    {
        UpdateGravityScale();
    }

    /// <summary>
    /// Updates the gravity scale according to the ground.
    /// </summary>
    private void UpdateGravityScale()
    {
        rb.gravityScale = StickToGround() ? 0 : _startGravityScale;
    }
    
    private bool StickToGround()
    {
        if (pMovement.IsActiveState) return pMovement.IsFullyOnGround;
        return grounded.IsGrounded;
    }
}
