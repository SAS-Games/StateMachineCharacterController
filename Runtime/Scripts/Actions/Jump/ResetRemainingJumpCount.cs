using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ResetRemainingJumpCount : IStateAction
    {
        private BlackboardKey _remainingJumpCountKey = default;
        private Actor _actor;
        private int _maxJumpCount = 0;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _actor = actor;
            _actor.TryGet(new BlackboardKey(key), out _maxJumpCount);
            _remainingJumpCountKey = _actor.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _actor.SetValue(_remainingJumpCountKey, _maxJumpCount);
        }
    }
}