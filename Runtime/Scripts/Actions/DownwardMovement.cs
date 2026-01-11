using SAS.StateMachineGraph;
using SAS.Core.BlackboardSystem;
using SAS.Core.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class DownwardMovement : IStateAction
    {
        private FSMCharacterController _characterController;
        private DownwardMovementConfig _downwardMovementConfig = default;
        private float _verticalMovement;
        private float _gravity;
        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.TryGet(out _downwardMovementConfig, key);
            actor.TryGetComponent(out _characterController);
            actor.TryGet(new BlackboardKey(FSMCharacterBlackboardKey.Gravity), out _gravity);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                _verticalMovement = _characterController.movementVector.y;
                return;
            }
            _verticalMovement += _gravity * _downwardMovementConfig.gravityMultiplier * Time.deltaTime;
            _verticalMovement = Mathf.Clamp(_verticalMovement, _downwardMovementConfig.fallSpeedRange.min, _downwardMovementConfig.fallSpeedRange.max);

            _characterController.movementVector.y = _verticalMovement;
        }
    }
}
