using UnityEngine;
using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class DecreaseCoyoteTimerCounter : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private BlackboardKey _coyoteTimeCounterKey = default;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _coyoteTimeCounterKey = _fsmCharacterController.GetOrRegisterKey(FSMCharacterBlackboardKey.CoyoteTimeCounter);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var value = _fsmCharacterController.GetValue<float>(_coyoteTimeCounterKey);
            value -= Time.deltaTime;
            _fsmCharacterController.SetValue(_coyoteTimeCounterKey, value);
        }
    }
}