using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ResetRemainigJumpCount : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _characterController;
        private BlackboardKey _remainigJumpCountKey = default;
        private int _maxJumpCount = 0;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _characterController.TryGet(new BlackboardKey(key), out _maxJumpCount);
            _remainigJumpCountKey = _characterController.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.SetValue(_remainigJumpCountKey, _maxJumpCount);
        }
    }
}