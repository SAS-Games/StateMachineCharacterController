using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class ApplyRotation : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        [FieldRequiresSelf] private Transform _transform;
        private float _turnSmoothTime;

        private float _turnSmoothSpeed;
        private float _minMoveDistance;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _fsmCharacterController.TryGet(new BlackboardKey(key), out _turnSmoothTime);
            _minMoveDistance = actor.GetComponent<CharacterController>().minMoveDistance;
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            Vector3 horizontalMovement = _fsmCharacterController.movementVector;
            horizontalMovement.y = 0f;

            if (horizontalMovement.sqrMagnitude >= _minMoveDistance)
            {
                float targetRotationY = _fsmCharacterController.movementInput.x < 0 ? 180f : 0f;
                float smoothRotationY = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetRotationY, ref _turnSmoothSpeed, _turnSmoothTime);
                _transform.eulerAngles = new Vector3(0f, smoothRotationY, 0f);
            }
        }

    }
}
