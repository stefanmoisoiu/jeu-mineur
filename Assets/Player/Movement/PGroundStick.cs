using System;
using UnityEngine;

public class PGroundStick : MovementState
{
        [Header("References")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private CapsuleCollider2D capsuleCollider;
        [SerializeField] private PMovement pMovement;
        [SerializeField] private PGrounded grounded;
        [SerializeField] private PDebug debug;
        private PDebug.DebugText _debugText;
        
        [Header("Stick Properties")]
        [SerializeField] private float onGroundColliderHeight = 0.5f;
        private float _startPlayerCapsuleOffsetY;
        private float _startPlayerCapsuleHeight;
        private float _playerHeight = 1;
        [SerializeField] private FloatSpringComponent stickToGroundSpring;
        [SerializeField] private LayerMask oneWayPlatformLayer;
        
        
        
        
        
        private Quaternion _currentUpQuaternion;
        public Quaternion CurrentUpQuaternion => _currentUpQuaternion;
        public Action<Quaternion,Quaternion> OnUpQuaternionChanged;
        public float HorizontalVelocity => GroundRelativeVector(rb.velocity).x;
        
        public Action OnStartStick, OnEndStick;

        private void Start()
        {
                _startPlayerCapsuleHeight = capsuleCollider.size.y;
                _startPlayerCapsuleOffsetY = capsuleCollider.offset.y;
                _debugText = () => $"Stick To Ground: {StickToGround()} | HVel: {Mathf.Round(HorizontalVelocity * 100) / 100}";
        }

        private bool StickToGround()
        {
                if (pMovement.IsActiveState) return pMovement.IsFullyOnGround;
                return grounded.IsGrounded;
        }

        protected override void OnStateEnter()
        {
                grounded.OnGroundedChanged += LandedVelocityUpdate;
                OnUpQuaternionChanged += UpdateVelocityQuaternion;
                debug.AddDebugText(_debugText);
        }

        protected override void OnStateExit()
        {
                grounded.OnGroundedChanged -= LandedVelocityUpdate;
                OnUpQuaternionChanged -= UpdateVelocityQuaternion;
                
                capsuleCollider.size = new Vector2(capsuleCollider.size.x, _startPlayerCapsuleHeight);
                capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, _startPlayerCapsuleOffsetY);
                debug.RemoveDebugText(_debugText);
        }

        protected override void ActiveStateUpdate()
        {
                UpdateColliderSize();
        }

        protected override void ActiveStateFixedUpdate()
        {
                UpdateUpQuaternion();
                RemoveYVelocityGrounded();
                SnapToGround();
        }
        public Vector2 GroundRelativeVector(Vector2 worldRelativeVector)
        {
                return Quaternion.Inverse(_currentUpQuaternion) * worldRelativeVector;
        }
        public Vector2 WorldRelativeVector(Vector2 groundRelativeVector)
        {
                return _currentUpQuaternion * groundRelativeVector;
        }

        private void RemoveYVelocityGrounded()
        {
                if (!StickToGround()) return;
                Vector2 groundRelativeVelocity = GroundRelativeVector(rb.velocity) * Vector2.right;
                rb.velocity = WorldRelativeVector(groundRelativeVelocity);
                Debug.DrawRay(transform.position, WorldRelativeVector(groundRelativeVelocity), Color.red);

        }
        /// <summary>
        /// Snaps the player to the ground when fully on close ground.
        /// </summary>
        private void SnapToGround()
        {
                stickToGroundSpring.currentPosition = rb.position.y;
                if (!StickToGround())
                {
                        stickToGroundSpring.velocity = 0;
                        return;
                }
                float targetY = grounded.CloseGroundHit.point.y + _playerHeight / 2;
                stickToGroundSpring.target = targetY;
                stickToGroundSpring.UpdateSpring(Time.fixedDeltaTime);
                rb.position += Vector2.up * stickToGroundSpring.velocity;
        }
        /// <summary>
        /// Updates the collider size according to the ground.
        /// </summary>
        private void UpdateColliderSize()
        {
                bool stickToGround = StickToGround();
                capsuleCollider.size = new Vector2(capsuleCollider.size.x, stickToGround ? onGroundColliderHeight : _startPlayerCapsuleHeight);
                capsuleCollider.offset = new Vector2(capsuleCollider.offset.x, stickToGround ? onGroundColliderHeight / 2 : _startPlayerCapsuleOffsetY);
        }
        
           #region Staying on ground
            /// <summary>
            /// Removes the vertical velocity when landing.
            /// </summary>
            private void LandedVelocityUpdate(bool wasGrounded, bool isGrounded)
            {
                    if (wasGrounded || !isGrounded) return;
                    int layer = grounded.GroundHit.collider.gameObject.layer;
                    if (oneWayPlatformLayer != (oneWayPlatformLayer | (1 << layer)))
                            rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            #endregion
            
            #region Up Quaternion
            /// <summary>
            /// Updates the upQuaternion according to the ground normal.
            /// </summary>
            private void UpdateUpQuaternion()
            {
                if (StickToGround())
                {
                    Quaternion groundUpQuaternion = Quaternion.LookRotation(Vector3.forward, grounded.GroundHit.normal);
                    if (groundUpQuaternion == _currentUpQuaternion) return;
                    _currentUpQuaternion = groundUpQuaternion;
                    OnUpQuaternionChanged?.Invoke(_currentUpQuaternion, groundUpQuaternion);
                }
                else
                {
                    Quaternion upQuaternion = Quaternion.LookRotation(Vector3.forward, Vector3.up);
                    if (_currentUpQuaternion == upQuaternion) return;
                    _currentUpQuaternion = upQuaternion;
                    OnUpQuaternionChanged?.Invoke(_currentUpQuaternion, upQuaternion);
                }
            }
            /// <summary>
            /// Updates the velocity according to the new upQuaternion. This is used to keep the player's velocity when going on a slope.
            /// </summary>
            private void UpdateVelocityQuaternion(Quaternion previousUpQuaternion, Quaternion newUpQuaternion)
            {
                if (!StickToGround()) return;
                rb.velocity =
                    Quaternion.FromToRotation(previousUpQuaternion * Vector3.up, newUpQuaternion * Vector3.up) *
                    rb.velocity;
            }
            #endregion
}