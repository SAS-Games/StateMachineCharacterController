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
        private float _coyoteTime = 0;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _fsmCharacterController.TryGet(new BlackboardKey(key), out _coyoteTime);
            _coyoteTimeCounterKey = _fsmCharacterController.GetOrRegisterKey("CoyoteTimeCounter");
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var value = _fsmCharacterController.GetValue<float>(_coyoteTimeCounterKey);
            value -= Time.deltaTime;
            _fsmCharacterController.SetValue(_coyoteTimeCounterKey, _coyoteTime);
        }
    }
}