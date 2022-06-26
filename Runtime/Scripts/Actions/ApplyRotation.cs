using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class ApplyRotation : IStateAction
    {
        private FSMCharacterController _fsmCharacterController;
        private Transform _transform;
        private ScriptableFloat _turnSmoothTime;

        private float _turnSmoothSpeed;

        void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
        {
            actor.TryGetComponent(out _fsmCharacterController);
            actor.TryGetComponent(out _transform);
            actor.TryGet(out _turnSmoothTime, key);
        }

        void IStateAction.Execute()
        {
            Vector3 horizontalMovement = _fsmCharacterController.movementVector;
            horizontalMovement.y = 0f;

            if (horizontalMovement.sqrMagnitude >= 0.02f)
            {
                float targetRotation = Mathf.Atan2(_fsmCharacterController.movementVector.x, _fsmCharacterController.movementVector.z) * Mathf.Rad2Deg;
                _transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(_transform.eulerAngles.y, targetRotation, ref _turnSmoothSpeed, _turnSmoothTime.value);
            }
        }
    }
}
