using System;
using UnityEngine;

public class PSlide : MovementState
{
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private PGrounded grounded;
        [SerializeField] private PGroundStick groundStick;
        [SerializeField] private PAnimator animator;
        [SerializeField] private PDebug debug;
        private PDebug.DebugText _debugText;
        

        [Header("Start Slide Properties")] 
        [SerializeField] private float stopSlideAngle = 10;
        [SerializeField] private float startSlideAngle = 60;
        [SerializeField] private LayerMask slideLayer;

        [Header("Slide Properties")]
        [SerializeField] private float maxFallSpeed;
        [SerializeField] private float gravityScale = 1;
        [SerializeField] private string slideAnimationName = "Slide";
        private float _slideVelocity;
        
        [Header("Stop Slide Properties")] [SerializeField] [Range(1,3)]
        private float stopSlideVelMult = 2;

        

        private void Start()
        {
                _debugText = () => $"Slide Vel: {_slideVelocity}";
        }

        protected override void OnStateEnter()
        {
                Vector2 groundedVelocity = groundStick.GroundRelativeVector(rb.velocity) * Vector2.right;
                float gravityToAdd = Vector2.Dot(groundedVelocity, Vector2.right) * gravityScale;
                _slideVelocity = gravityToAdd;
                animator.PlayAnimation(slideAnimationName);
                debug.AddDebugText(_debugText);
        }

        protected override void OnStateExit()
        {
                debug.RemoveDebugText(_debugText);
        }

        protected override void ActiveStateUpdate()
        {
                TryStopSlide(out _);
        }

        protected override void ActiveStateFixedUpdate()
        {
                float gravityToAdd = Vector2.Dot(grounded.CloseGroundHit.normal, Vector2.right) * gravityScale;
                _slideVelocity += gravityToAdd;
                _slideVelocity = Mathf.Clamp(_slideVelocity, -maxFallSpeed, maxFallSpeed);
                rb.velocity = groundStick.WorldRelativeVector(Vector2.right * _slideVelocity);
        }

        public void TryStartSlide(out bool success)
        {
                success = false;
                if (!CanSlide()) return;
                success = true;
                stateManager.SetState(PStateManager.State.Slide);
        }
        private void TryStopSlide(out bool success)
        {
                success = false;
                if (CanSlide()) return;
                success = true;
                rb.velocity = groundStick.WorldRelativeVector(Vector2.right * _slideVelocity * stopSlideVelMult);
                stateManager.SetState(PStateManager.State.Normal);
        }

        private bool CanSlide()
        {
                if (!grounded.IsGrounded) return false;
                
                float groundAngle = Vector2.Angle(grounded.GroundHit.normal, Vector2.up);
                bool isSlideAngle = groundAngle > startSlideAngle;
                bool isTooSmallSlideAngle = groundAngle < stopSlideAngle;
                
                bool isSlideLayer = slideLayer == (slideLayer | (1 << grounded.GroundHit.collider.gameObject.layer));
                
                if (!isSlideAngle && (!isSlideLayer || isTooSmallSlideAngle)) return false;
                return true;
        }
}