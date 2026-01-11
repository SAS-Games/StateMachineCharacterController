using SAS.StateMachineGraph;
using SAS.Core.BlackboardSystem;
using SAS.Core.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ResetCoyoteTimerCounter : IStateAction
    {
        private BlackboardKey _coyoteTimeCounterKey = default;
        private Actor _actor;
        private float _coyoteTime = 0;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _actor = actor;
            _actor.TryGet(new BlackboardKey(key), out _coyoteTime);
            _coyoteTimeCounterKey = _actor.GetOrRegisterKey(FSMCharacterBlackboardKey.CoyoteTimeCounter);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _actor.SetValue(_coyoteTimeCounterKey, _coyoteTime);
        }
    }
}