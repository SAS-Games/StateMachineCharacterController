using SAS.StateMachineGraph;
using SAS.Core.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class WallJumpCondition : ICustomCondition
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
            return _fsmCharacterController.IsTouchingLayerSide(_fsmCharacterController.WallLayer, out var raycastHit);
        }
    }
}
