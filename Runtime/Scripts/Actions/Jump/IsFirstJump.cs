using SAS.StateMachineGraph;
using SAS.Core.BlackboardSystem;
using SAS.Core.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class IsFirstJump : ICustomCondition
    {
        private BlackboardKey _remainingJumpCountKey = default;
        private BlackboardKey _maxJumpCountKey = default;
        private Actor _actor;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _actor = actor;
            _remainingJumpCountKey = actor.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
            _maxJumpCountKey = actor.GetOrRegisterKey(FSMCharacterBlackboardKey.MaxJumpCount);
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit() { }

        bool ICustomCondition.Evaluate()
        {
            return _actor.GetValue<int>(_remainingJumpCountKey) == _actor.GetValue<int>(_maxJumpCountKey);
        }
    }
}
