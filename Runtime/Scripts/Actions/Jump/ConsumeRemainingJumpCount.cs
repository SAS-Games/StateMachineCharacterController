using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ConsumeRemainingJumpCount : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private BlackboardKey _remainingJumpCountKey = default;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _remainingJumpCountKey = _fsmCharacterController.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var value = _fsmCharacterController.GetValue<int>(_remainingJumpCountKey);
            value--;
            _fsmCharacterController.SetValue(_remainingJumpCountKey, value);
        }
    }
}