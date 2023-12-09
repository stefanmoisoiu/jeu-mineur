using UnityEngine;

public class PJumpLandSFX : MovementState
{
        [SerializeField] private PMovement movement;
        [SerializeField] private PGrounded grounded;
        
        [SerializeField] private ScriptableSFX jumpSfx, landSfx;

        protected override void OnStateEnter()
        {
                movement.OnJump += PlayJumpSfx;
                grounded.OnGroundedChanged += Land;
        }

        protected override void OnStateExit()
        {
                movement.OnJump -= PlayJumpSfx;
                grounded.OnGroundedChanged -= Land;
        }

        private void PlayJumpSfx() => jumpSfx.Play();

        private void Land(bool wasGrounded, bool isGrounded)
        {
                if(!wasGrounded && isGrounded) landSfx.Play();
        }
}