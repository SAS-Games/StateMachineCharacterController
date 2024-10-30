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
    }
    [RequireComponent(typeof(Actor)), DisallowMultipleComponent]
    public class FSMCharacterController : MonoBehaviour
    {
        [FieldRequiresSelf] private CharacterController _characterController;
        [SerializeField] private BlackboardData m_BlackboardData = default;

        /* [NonSerialized]*/
        internal Vector3 movementVector;
        /* [NonSerialized]*/
        internal Vector3 movementInput;
        public float NormalizedMoveInput => movementInput.magnitude;

        private Actor _actor;
        private int NormalizedMoveInputHash = Animator.StringToHash("MoveInput");
        public Vector3 VerticalVelocity => _characterController.velocity.Multiply(0.0f, 1.0f, 0.0f);

        private Blackboard _blackboard = new Blackboard();


        private void Awake()
        {
            this.Initialize();
            m_BlackboardData?.SetValuesOnBlackboard(_blackboard);
            Actor.Initialize();
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

        public bool TryGet<T>(BlackboardKey key, out T value)
        {
            return _blackboard.TryGetValue(key, out value);
        }

        public BlackboardKey GetOrRegisterKey(string keyName)
        {
            return _blackboard.GetOrRegisterKey(keyName);
        }

        public bool IsFacingRight()
        {
            float yRotation = transform.localEulerAngles.y;
            if (Mathf.Abs(yRotation) < 0.0001f)
                yRotation = 0;
            const float facingThreshold = 5f;  // Small threshold for smoother rotations

            if (movementInput.x > _characterController.minMoveDistance) // Adding a small tolerance for input drift
                return true;
            else if (movementInput.x < -_characterController.minMoveDistance)
                return false;
            else if ((yRotation >= 0 && yRotation <= facingThreshold) || (yRotation >= 360 - facingThreshold && yRotation <= 360))
                return true; // Facing right
            return false; // Facing left
        }

        internal T GetValue<T>(BlackboardKey key)
        {
            return _blackboard.GetValue<T>(key);
        }

        internal void SetValue<T>(BlackboardKey key, T v)
        {
            _blackboard.SetValue(key, v);
        }
    }
}
