using UnityEngine;

public class PMoveDustParticles : MovementState
{
    [SerializeField] private ParticleSystem dustParticles;
    [SerializeField] private PGrounded grounded;
    

    protected override void OnStateEnter()
    {
        grounded.OnGroundedChanged += OnGroundedChanged;
        OnGroundedChanged(grounded.WasGrounded, grounded.IsGrounded);
    }
    protected override void OnStateExit()
    {
        grounded.OnGroundedChanged -= OnGroundedChanged;
        dustParticles.Stop();
    }
    
    private void OnGroundedChanged(bool wasGrounded, bool isGrounded)
    {
        if (isGrounded)
        {
            dustParticles.Play();
        }
        else
        {
            dustParticles.Stop();
        }
    }
}