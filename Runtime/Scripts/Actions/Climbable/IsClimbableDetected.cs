using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class IsClimbableDetected : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit() { }

        bool ICustomCondition.Evaluate()
        {
            if (_fsmCharacterController.IsTouchingLayerSide(Vector3.right, _fsmCharacterController.ClimbableLayer, out var raycastHit))
                return true;
            else
                return _fsmCharacterController.IsTouchingLayerSide(Vector3.left, _fsmCharacterController.ClimbableLayer, out raycastHit);
        }
    }
}