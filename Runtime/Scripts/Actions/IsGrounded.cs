using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class IsGrounded : ICustomCondition
    {
        [FieldRequiresSelf] private CharacterController _characterController;
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        [FieldRequiresSelf] private Transform _characterTransform;
        private float _coyoteTimeThreshold = 0.016f; // Grace period in seconds
        private float lastGroundedTime;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _coyoteTimeThreshold = Time.fixedDeltaTime;
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit() { }

        bool ICustomCondition.Evaluate()
        {
            return _characterController.isGrounded;
            if (_fsmCharacterController.IsGrounded)
            {
                lastGroundedTime = Time.time;
                return true;
            }

            // Coyote time check: allow ground state if recently grounded
            if (Time.time - lastGroundedTime < _coyoteTimeThreshold)
                return true;

            return false;
        }
    }
}
