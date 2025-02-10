using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class ApplyRotationSideWays : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        [FieldRequiresSelf] private Transform _transform;
        private float _turnSmoothTime;

        private float _turnSmoothSpeed;
        private float _minMoveDistance;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(new BlackboardKey(key), out _turnSmoothTime);
            _minMoveDistance = actor.GetComponent<CharacterController>().minMoveDistance;
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            Vector3 horizontalMovement = _fsmCharacterController.movementVector;
            horizontalMovement.y = 0f;

           // if (executeEvent == ActionExecuteEvent.OnStateEnter || horizontalMovement.sqrMagnitude >= _minMoveDistance)
            {
                float targetRotationY = _fsmCharacterController.isFacingRight ? 90 : -90;
                float smoothRotationY = Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetRotationY, ref _turnSmoothSpeed, _turnSmoothTime);
                _transform.eulerAngles = new Vector3(0f, smoothRotationY, 0f);
            }
        }

    }
}
