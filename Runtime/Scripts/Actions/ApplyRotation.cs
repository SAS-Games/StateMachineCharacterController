using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class ApplyRotation : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        [FieldRequiresSelf] private Transform _transform;
        private ScriptableReadOnlyFloat _turnSmoothTime;

        private float _turnSmoothSpeed;
        private float _minMoveDistance;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(out _turnSmoothTime, key);
            _minMoveDistance = actor.GetComponent<CharacterController>().minMoveDistance;
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            Vector3 horizontalMovement = _fsmCharacterController.movementVector;
            horizontalMovement.y = 0f;

            if (horizontalMovement.sqrMagnitude >= _minMoveDistance)
            {
                float targetRotation = Mathf.Atan2(_fsmCharacterController.movementVector.x, _fsmCharacterController.movementVector.z) * Mathf.Rad2Deg;
                _transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetRotation, ref _turnSmoothSpeed, _turnSmoothTime.value);
            }
        }
    }
}
