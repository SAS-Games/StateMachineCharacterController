using SAS.StateMachineGraph;
using SAS.Core.BlackboardSystem;
using SAS.Core.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ResetRemainingJumpCount : IStateAction
    {
        private BlackboardKey _remainingJumpCountKey = default;
        private Actor _actor;
        private BlackboardKey _maxJumpCountKey = default;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _actor = actor;
            _maxJumpCountKey = _actor.GetOrRegisterKey(FSMCharacterBlackboardKey.MaxJumpCount);
            _remainingJumpCountKey = _actor.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _actor.SetValue(_remainingJumpCountKey, _actor.GetValue<int>(_maxJumpCountKey));
        }
    }
}