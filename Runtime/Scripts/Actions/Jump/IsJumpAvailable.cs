using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class IsJumpAvailable : ICustomCondition
    {
        private BlackboardKey _remainingJumpCountKey = default;
        private Actor _actor;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _actor = actor;
            _remainingJumpCountKey = _actor.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit() { }

        bool ICustomCondition.Evaluate()
        {
            return _actor.GetValue<int>(_remainingJumpCountKey) > 0;
        }
    }
}
