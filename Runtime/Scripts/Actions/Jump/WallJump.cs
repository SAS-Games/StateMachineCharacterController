using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class WallJump : IStateAction
    {
        private FSMCharacterController _fsmCharacterController;
        private UpwardMovementConfig _upwardMovementConfig = default;

        private float _gravityContributionMultiplier;
        private float _verticalMovement;
        private float _moveSpeed;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.TryGet(out _upwardMovementConfig, key);
            actor.TryGetComponent(out _fsmCharacterController);
            _fsmCharacterController.TryGet<float>(new Utilities.BlackboardSystem.BlackboardKey(FSMCharacterBlackboardKey.MoveSpeed), out _moveSpeed);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                _gravityContributionMultiplier = 0;
                _verticalMovement = _upwardMovementConfig.jumpForce;
                _fsmCharacterController.IsTouchingLayerSide(_fsmCharacterController.WallLayer, out var raycastHit);
                _fsmCharacterController.movementVector = raycastHit.normal * _moveSpeed;
                return;
            }
            _gravityContributionMultiplier += _upwardMovementConfig.gravityComebackMultiplier;
            _gravityContributionMultiplier *= _upwardMovementConfig.gravityDivider; //Reduce the gravity effect
            _verticalMovement += Physics.gravity.y * _upwardMovementConfig.gravityMultiplier * Time.deltaTime * _gravityContributionMultiplier;
            _fsmCharacterController.movementVector.y = _verticalMovement;
        }
    }
}