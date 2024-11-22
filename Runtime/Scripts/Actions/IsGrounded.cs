using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class IsGrounded : ICustomCondition
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
            return _fsmCharacterController.IsGrounded;
        }
    }
}
