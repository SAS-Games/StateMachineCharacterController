using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class DashMovement : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private DashMovementConfig _dashMovementConfig = default;
        private BlackboardKey _isDashingKey = default;
        private Actor _actor;
        private Vector3 _dashDirection = Vector2.zero;
        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(out _dashMovementConfig);
            _isDashingKey = actor.GetOrRegisterKey(FSMCharacterBlackboardKey.IsDashing);
            _actor = actor;
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                _actor.SetValue(_isDashingKey, true);

                _dashDirection = new Vector3(_fsmCharacterController.movementInput.x, _fsmCharacterController.movementInput.y).normalized;
                if (_dashMovementConfig.forwardDirection || _dashDirection == Vector3.zero)
                    _dashDirection = _fsmCharacterController.transform.forward;//isFacingRight ? Vector3.right : Vector3.left;
                return;
            }

            _fsmCharacterController.movementVector.x = _dashMovementConfig.horizontalSpeed * _dashDirection.x;
            _fsmCharacterController.movementVector.z = _dashMovementConfig.horizontalSpeed * _dashDirection.z;
            _fsmCharacterController.movementVector.y = _dashMovementConfig.verticalSpeed * _dashDirection.y;
        }
    }
}
