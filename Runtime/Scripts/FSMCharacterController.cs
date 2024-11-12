using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using System;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public static class FSMCharacterBlackboardKey
    {
        public const string CoyoteTimeCounter = "CoyoteTimeCounter";
        public const string RemainingJumpCount = "RemainingJumpCount";
        public const string MoveSpeed = "MoveSpeed";
        public const string IsDashing = "IsDashing";
    }

    [RequireComponent(typeof(Actor)), DisallowMultipleComponent]
    public class FSMCharacterController : MonoBehaviour
    {
        [FieldRequiresSelf] private CharacterController _characterController;
        [field: SerializeField] public LayerMask WallLayer { get; private set; }
        [field: SerializeField] public LayerMask ClimbableLayer { get; private set; }
        [SerializeField] private LayerMask m_GroundLayer;

        /* [NonSerialized]*/
        public Vector3 movementVector;
        /* [NonSerialized]*/
        internal Vector3 movementInput;

        internal bool isFacingRight;
        private Transform _transform;

        public float Speed { get; private set; }
        public float NormalizedMoveInput => movementInput.magnitude;

        private int NormalizedMoveInputHash = Animator.StringToHash("MoveInput");
        public Vector3 VerticalVelocity => _characterController.velocity.Multiply(0.0f, 1.0f, 0.0f);

        private Actor _actor;
        public Actor Actor
        {
            get
            {
                if (_actor == null)
                    _actor = GetComponent<Actor>();
                return _actor;
            }
        }

        public bool IsGrounded => _characterController.isGrounded;

        private void Awake()
        {
            this.Initialize();
            _transform = transform;
            SetFacingDirection();
        }

        public void OnMove(float normalizedMoveInput)
        {
            Speed = (float)Math.Round(normalizedMoveInput, 2);
            Actor.SetFloat(NormalizedMoveInputHash, Speed);
        }

        public void OnJumpInitiated()
        {
            Actor.SetTrigger("Jump");
            Actor.SetBool("JumpHold", true);
        }

        public void OnJumpCanceled()
        {
            Actor.SetBool("JumpHold", false);
        }

        public void OnDashInitiated()
        {
            Actor.SetTrigger("Dash");
        }

        public void OnClimbInitiated()
        {
            Actor.SetBool("Climb", true);
        }
        public void OnClimbCanceled()
        {
            Actor.SetBool("Climb", false);
        }

        public void SetFacingDirection()
        {
            float yRotation = _transform.localEulerAngles.y;
            if (Mathf.Abs(yRotation) < 0.0001f)
                yRotation = 0;

            const float facingThreshold = 5f;  // Small threshold for smoother rotations
            isFacingRight = (yRotation >= 0 && yRotation <= facingThreshold) || (yRotation >= 360 - facingThreshold && yRotation <= 360);
        }

        public bool IsTouchingLayerSide(LayerMask layerMask, out RaycastHit hitInfo, float maxSlopeAngle = 0.1f)
        {
            return IsTouchingLayerSide(isFacingRight ? Vector3.right : Vector3.left, layerMask, out hitInfo, maxSlopeAngle);
        }

        public bool IsTouchingLayerSide(Vector3 facingDirection, LayerMask layerMask, out RaycastHit hitInfo, float maxSlopeAngle = 0.1f)
        {
            // Calculate check radius using CharacterController radius and skinWidth
            float checkRadius = _characterController.radius + _characterController.skinWidth + 0.01f;
            Vector3 characterPosition = _transform.position + Vector3.up * (_characterController.height / 2);

            // SphereCast to the side of the character to detect walls
            bool hit = Physics.SphereCast(characterPosition, _characterController.skinWidth, facingDirection, out hitInfo, checkRadius, layerMask);

            // Check if the hit normal meets the slope tolerance criteria
            if (hit && Vector3.Dot(hitInfo.normal, Vector3.up) < maxSlopeAngle)
                return true;

            return false;
        }
    }
}

