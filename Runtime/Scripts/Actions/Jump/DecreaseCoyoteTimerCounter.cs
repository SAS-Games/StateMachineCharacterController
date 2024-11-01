using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class DecreaseCoyoteTimerCounter : IStateAction
    {
        private BlackboardKey _coyoteTimeCounterKey = default;
        private Actor _actor;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _actor = actor;
            _coyoteTimeCounterKey = _actor.GetOrRegisterKey(FSMCharacterBlackboardKey.CoyoteTimeCounter);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var value = _actor.GetValue<float>(_coyoteTimeCounterKey);
            value -= Time.deltaTime;
            _actor.SetValue(_coyoteTimeCounterKey, value);
        }
    }
}