using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class UpwardMovement : IStateAction
    {
        private FSMCharacterController _characterController;
        private UpwardMovementConfig _upwardMovementConfig = default;

        private float _gravityContributionMultiplier;
        private float _verticalMovement;
        private float _extraForce;
        private float _gravity;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.TryGet(out _upwardMovementConfig, key);
            actor.TryGetComponent(out _characterController);
            actor.TryGet(new BlackboardKey(FSMCharacterBlackboardKey.Gravity), out _gravity);
            EventBus<ExternalForeAppliedEvent>.Register(new EventBinding<ExternalForeAppliedEvent>(val =>
            {
                _extraForce = val.force.y;
            }));
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                _gravityContributionMultiplier = 0;
                _verticalMovement = _upwardMovementConfig.jumpForce + _extraForce;
                _extraForce = 0;
                return;
            }
            _gravityContributionMultiplier += _upwardMovementConfig.gravityComebackMultiplier;
            _gravityContributionMultiplier *= _upwardMovementConfig.gravityDivider; //Reduce the gravity effect
            //Note that deltaTime is used even though it's going to be used in ApplyMovementVectorAction, this is because it represents an acceleration, not a speed
            _verticalMovement += _gravity * _upwardMovementConfig.gravityMultiplier * Time.deltaTime * _gravityContributionMultiplier;
            _characterController.movementVector.y = _verticalMovement;
        }
    }
}
