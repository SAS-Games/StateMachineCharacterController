using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class DecreaseRemainigJumpCount : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private BlackboardKey _remainigJumpCountKey = default;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _remainigJumpCountKey = _fsmCharacterController.GetOrRegisterKey(FSMCharacterBlackboardKey.RemainingJumpCount );
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var value = _fsmCharacterController.GetValue<int>(_remainigJumpCountKey);
            value--;
            _fsmCharacterController.SetValue(_remainigJumpCountKey, value);
        }
    }
}