using SAS.StateMachineGraph;
using SAS.Core.BlackboardSystem;
using SAS.Core.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ResetDashing : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private BlackboardKey _isDashingKey = default;
        private Actor _actor;
        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _isDashingKey = actor.GetOrRegisterKey(FSMCharacterBlackboardKey.IsDashing);
            _actor = actor;
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _actor.SetValue(_isDashingKey, false);
        }
    }
}
