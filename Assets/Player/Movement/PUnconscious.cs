using UnityEngine;

public class PUnconscious : MovementState
{
    [Header("References")]
    [SerializeField] private PInputManager inputManager;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PAnimator animator;
    
    

    [Header("Unconscious Properties")]
    [SerializeField] private int unconsciousShakeCount = 5;
    [SerializeField] [Range(0,1)] private float inputThreshold = 0.3f;
    
    [Header("Animations")]
    [SerializeField] private string unconsciousAnimation = "Dead";
    
    [Header("Camera Shake")]
    [SerializeField] private ScriptableCameraShake shakeCameraShake;

    
    
    private int _unconsciousShakeCount;
    private bool _shakeReset = true;

    protected override void OnStateEnter()
    {
        _unconsciousShakeCount = unconsciousShakeCount;
        _shakeReset = true;
        
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        animator.PlayAnimation(unconsciousAnimation);

        inputManager.OnJump += Shake;
    }

    protected override void OnStateExit()
    {
        rb.isKinematic = false;
        inputManager.OnJump -= Shake;
    }

    protected override void ActiveStateUpdate()
    {
        int movementDir = GetMovementDir();
        
        
        if (movementDir == 0 && !_shakeReset) _shakeReset = true;

        if (!_shakeReset) return;
        if (movementDir == 0) return;
        Shake();
    }

    private void Shake()
    {
        if (_unconsciousShakeCount <= 0) return;
        _shakeReset = false;
        _unconsciousShakeCount--;
        shakeCameraShake.Shake();
        if (_unconsciousShakeCount <= 0)
        {
            stateManager.SetState(PStateManager.State.Normal);
            return;
        }
    }
    private int GetMovementDir()
    {
        if(Mathf.Abs(inputManager.MoveInput) < inputThreshold) return 0;
        return inputManager.MoveInput > 0 ? 1 : -1;
    }
}
