using UnityEngine;

public class PJumpLandSFX : MovementState
{
        [SerializeField] private PMovement movement;
        [SerializeField] private PGrounded grounded;
        
        [SerializeField] private ScriptableSFX jumpSfx, landSfx;

        protected override void OnStateEnter()
        {
                movement.OnJump += Play;
                grounded.OnGroundedChanged += Land;
        }

        protected override void OnStateExit()
        {
                movement.OnJump -= Play;
                grounded.OnGroundedChanged -= Land;
        }

        private void Play() => jumpSfx.Play();

        private void Land(bool wasGrounded, bool isGrounded)
        {
                if(!wasGrounded && isGrounded) landSfx.Play();
        }
}