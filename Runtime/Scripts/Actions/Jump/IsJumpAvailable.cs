using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class IsJumpAvailable : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private BlackboardKey _remainingJumpCountKey = default;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _remainingJumpCountKey = _fsmCharacterController.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit() { }

        bool ICustomCondition.Evaluate()
        {
            return _fsmCharacterController.GetValue<int>(_remainingJumpCountKey) > 0;
        }
    }
}
