using SAS.StateMachineGraph;
using SAS.Core.BlackboardSystem;
using SAS.Core.TagSystem;
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
        private float _gravity;
        private float _rotationSpeed = 10f; // Speed of rotation smoothing
        private Quaternion _targetRotation;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.TryGet(out _upwardMovementConfig, key);
            actor.TryGetComponent(out _fsmCharacterController);
            actor.TryGet<float>(new BlackboardKey(FSMCharacterBlackboardKey.MoveSpeed), out _moveSpeed);
            actor.TryGet(new BlackboardKey(FSMCharacterBlackboardKey.Gravity), out _gravity);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                _gravityContributionMultiplier = 0;
                _verticalMovement = _upwardMovementConfig.jumpForce;
                _fsmCharacterController.IsTouchingLayerSide(_fsmCharacterController.WallLayer, out var raycastHit);
                _fsmCharacterController.movementVector = raycastHit.normal * _moveSpeed;

                // Set target rotation to face away from the wall
                _targetRotation = Quaternion.LookRotation(-raycastHit.normal, Vector3.up);
            }

            // Smoothly rotate towards the target rotation every frame
            _fsmCharacterController.transform.rotation = Quaternion.Lerp(_fsmCharacterController.transform.rotation, _targetRotation, Time.deltaTime * _rotationSpeed);

            _gravityContributionMultiplier += _upwardMovementConfig.gravityComebackMultiplier;
            _gravityContributionMultiplier *= _upwardMovementConfig.gravityDivider; // Reduce the gravity effect
            _verticalMovement += _gravity * _upwardMovementConfig.gravityMultiplier * Time.deltaTime * _gravityContributionMultiplier;
            _fsmCharacterController.movementVector.y = _verticalMovement;
        }
    }
}
