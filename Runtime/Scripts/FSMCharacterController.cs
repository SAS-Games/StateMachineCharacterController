using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using System;
using UnityEngine;
using SAS.Utilities.BlackboardSystem;

namespace SAS.StateMachineCharacterController
{
    public static class FSMCharacterBlackboardKey
    {
        public const string CoyoteTimeCounter = "CoyoteTimeCounter";
        public const string RemainingJumpCount = "RemainingJumpCount";
        public const string MoveSpeed = "MoveSpeed";
    }

    [RequireComponent(typeof(Actor)), DisallowMultipleComponent]
    public class FSMCharacterController : MonoBehaviour
    {
        [FieldRequiresSelf] private CharacterController _characterController;
        [SerializeField] private BlackboardData m_BlackboardData = default;
        [field: SerializeField] public LayerMask WallLayer { get; private set; }

        /* [NonSerialized]*/
        internal Vector3 movementVector;
        /* [NonSerialized]*/
        internal Vector3 movementInput;
        internal bool isFacingRight;

        public float NormalizedMoveInput => movementInput.magnitude;

        private Actor _actor;
        private int NormalizedMoveInputHash = Animator.StringToHash("MoveInput");
        public Vector3 VerticalVelocity => _characterController.velocity.Multiply(0.0f, 1.0f, 0.0f);

        private Blackboard _blackboard = new Blackboard();
        private Transform _transform;


        private void Awake()
        {
            this.Initialize();
            //todo: move the m_BlackboardData to the actor class 
            m_BlackboardData?.SetValuesOnBlackboard(_blackboard);
            Actor.Initialize();
            _transform = transform;
            SetFacingDirection();
        }

        public Actor Actor
        {
            get
            {
                if (_actor == null)
                    _actor = GetComponent<Actor>();
                return _actor;
            }
        }

        private bool _isGrounded = true;
        public bool IsGrounded
        {
            get { return _isGrounded; }
            internal set
            {
                _isGrounded = value;
                Actor.SetBool("IsGrounded", _isGrounded);
            }
        }

        public void OnMove(float normalizedMoveInput)
        {
            Actor.SetFloat(NormalizedMoveInputHash, (float)Math.Round(normalizedMoveInput, 2));
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

        public void SetFacingDirection()
        {
            float yRotation = _transform.localEulerAngles.y;
            if (Mathf.Abs(yRotation) < 0.0001f)
                yRotation = 0;
            const float facingThreshold = 5f;  // Small threshold for smoother rotations
            isFacingRight = (yRotation >= 0 && yRotation <= facingThreshold) || (yRotation >= 360 - facingThreshold && yRotation <= 360);
        }

        public bool TryGet<T>(BlackboardKey key, out T value)
        {
            return _blackboard.TryGetValue(key, out value);
        }

        internal T GetValue<T>(BlackboardKey key)
        {
            return _blackboard.GetValue<T>(key);
        }

        public BlackboardKey GetOrRegisterKey(string keyName)
        {
            return _blackboard.GetOrRegisterKey(keyName);
        }

        internal void SetValue<T>(BlackboardKey key, T v)
        {
            _blackboard.SetValue(key, v);
        }

        public bool IsTouchingLayerSide(LayerMask layerMask, out RaycastHit hitInfo, float maxSlopeAngle = 0.1f)
        {
            // Calculate check radius using CharacterController radius and skinWidth
            float checkRadius = _characterController.radius + _characterController.skinWidth + 0.01f;
            Vector3 characterPosition = _transform.position + Vector3.up * (_characterController.height / 2);

            // Determine the direction to cast based on the character's facing direction
            Vector3 direction = isFacingRight ? Vector3.right : Vector3.left;

            // SphereCast to the side of the character to detect walls
            bool hit = Physics.SphereCast(characterPosition, _characterController.skinWidth, direction, out hitInfo, checkRadius, layerMask);

            // Check if the hit normal meets the slope tolerance criteria
            if (hit && Vector3.Dot(hitInfo.normal, Vector3.up) < maxSlopeAngle)
                return true;

            return false;
        }
    }
}
