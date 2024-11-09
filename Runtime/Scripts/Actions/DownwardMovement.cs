using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class DownwardMovement : IStateAction
    {
        private FSMCharacterController _characterController;
        private DownwardMovementConfig _downwardMovementConfig = default;
        private float _verticalMovement;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.TryGet(out _downwardMovementConfig, key);
            actor.TryGetComponent(out _characterController);

            EventBus<BounceForeAppliedEvent>.Register(new EventBinding<BounceForeAppliedEvent>(val =>
            {
                _verticalMovement = val.force.y;
                Debug.Log(_verticalMovement);
            }));

        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                _verticalMovement = _characterController.movementVector.y;
                return;
            }
            _verticalMovement += Physics.gravity.y * _downwardMovementConfig.gravityMultiplier * Time.deltaTime;
            _verticalMovement = Mathf.Clamp(_verticalMovement, _downwardMovementConfig.fallSpeedRange.min, _downwardMovementConfig.fallSpeedRange.max);

            _characterController.movementVector.y = _verticalMovement;
        }
    }
}
