using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ResetRemainingJumpCount : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _characterController;
        private BlackboardKey _remainingJumpCountKey = default;
        private int _maxJumpCount = 0;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _characterController.TryGet(new BlackboardKey(key), out _maxJumpCount);
            _remainingJumpCountKey = _characterController.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.SetValue(_remainingJumpCountKey, _maxJumpCount);
        }
    }
}