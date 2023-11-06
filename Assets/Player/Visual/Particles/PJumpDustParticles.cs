using UnityEngine;

public class PJumpDustParticles : MovementState
{
    [SerializeField] private ParticleSystem dustParticles;
    [SerializeField] private PMovement movement;

    protected override void OnStateEnter()
    {
        movement.OnJump += OnJump;
    }

    protected override void OnStateExit()
    {
        movement.OnJump -= OnJump;
        dustParticles.Stop();
    }

    private void OnJump()
    {
        dustParticles.Play();
    }
}