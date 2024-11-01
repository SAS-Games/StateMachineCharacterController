using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ConsumeRemainingJumpCount : IStateAction
    {
        private BlackboardKey _remainingJumpCountKey = default;
        private Actor _actor;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _actor = actor;
            _remainingJumpCountKey = actor.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var value = _actor.GetValue<int>(_remainingJumpCountKey);
            value--;
            _actor.SetValue(_remainingJumpCountKey, value);
        }
    }
}