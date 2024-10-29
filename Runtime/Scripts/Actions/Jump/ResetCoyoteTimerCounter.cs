using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ResetCoyoteTimerCounter : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _characterController;
        private BlackboardKey _coyoteTimeCounterKey = default;
        private float _coyoteTime = 0;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _characterController.TryGet(new BlackboardKey(key), out _coyoteTime);
            _coyoteTimeCounterKey = _characterController.GetOrRegisterKey(FSMCharacterBlackboardKey.CoyoteTimeCounter);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.SetValue(_coyoteTimeCounterKey, _coyoteTime);
        }
    }
}