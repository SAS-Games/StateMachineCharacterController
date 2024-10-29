using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class IsWithinCoyoteTime : ICustomCondition
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private BlackboardKey _coyoteTimeCounterKey = default;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _coyoteTimeCounterKey = _fsmCharacterController.GetOrRegisterKey("CoyoteTimeCounter");
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit()
        {
            _fsmCharacterController.SetValue(_coyoteTimeCounterKey, 0f);
        }

        bool ICustomCondition.Evaluate()
        {
            return _fsmCharacterController.GetValue<float>(_coyoteTimeCounterKey) > 0;
        }
    }
}
