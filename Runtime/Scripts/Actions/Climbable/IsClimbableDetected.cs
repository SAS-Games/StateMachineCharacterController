using SAS.StateMachineGraph;
using SAS.Core.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class IsClimbableDetected : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private Transform _orientation;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _orientation = _fsmCharacterController.transform;
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit() { }

        bool ICustomCondition.Evaluate()
        {
            // Use orientation transform's forward direction for wall detection
            Vector3 checkDirection = _orientation.forward;
            bool hitFound = _fsmCharacterController.IsTouchingLayerSide(checkDirection, _fsmCharacterController.ClimbableLayer, out RaycastHit hit);

            if (hitFound)
            {
                // Store the hit data for climbing state to use
                //_fsmCharacterController.LastClimbHit = hit;
                return true;
            }
            return false;
        }
    }
}