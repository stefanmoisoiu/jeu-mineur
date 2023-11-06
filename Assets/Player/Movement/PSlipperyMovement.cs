using UnityEngine;

public class PSlipperyMovement : MovementState
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private PGroundStick groundStick;
    [SerializeField] private PGrounded grounded;
    
    [Header("Start Slippery Movement Properties")]
    [SerializeField] private LayerMask slipperyLayer;
    
    [Header("Slippery Movement Properties")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 5f;

    protected override void ActiveStateUpdate()
    {
        TryStopSlipperyMovement(out _);
    }

    protected override void ActiveStateFixedUpdate()
    {
        rb.velocity += groundStick.WorldRelativeVector(Vector2.right * GetAcceleration());
    }
    
    private float GetAcceleration()
    {
        float accel = acceleration * inputManager.MoveInput * Time.fixedDeltaTime;

        float absHVelocity = Mathf.Abs(groundStick.HorizontalVelocity);
        int HVelSign = (int)Mathf.Sign(groundStick.HorizontalVelocity);
        int targetMoveSign = (int)Mathf.Sign(inputManager.MoveInput);
        
        if (absHVelocity >= maxSpeed)
        {
            if (HVelSign == targetMoveSign)
                accel = 0;
        }
        else if (Mathf.Abs(accel + groundStick.HorizontalVelocity) > maxSpeed)
            accel = HVelSign * (maxSpeed - absHVelocity);
        
        return accel;
    }

    public void TryStartSlipperyMovement(out bool success)
    {
        success = false;
        if (!CanSlipperyMovement()) return;
        success = true;
        stateManager.SetState(PStateManager.State.SlipperyMovement);
    }
    private void TryStopSlipperyMovement(out bool success)
    {
        success = false;
        if (CanSlipperyMovement()) return;
        success = true;
        stateManager.SetState(PStateManager.State.Normal);
    }
    
    private bool CanSlipperyMovement()
    {
        if(!grounded.IsGrounded) return false;
        
        bool isSlipperyLayer = slipperyLayer == (slipperyLayer | (1 << grounded.GroundHit.collider.gameObject.layer));
        return isSlipperyLayer;
    }
}