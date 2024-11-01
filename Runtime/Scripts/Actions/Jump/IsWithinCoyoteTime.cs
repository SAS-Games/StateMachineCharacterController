using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class IsWithinCoyoteTime : ICustomCondition
    {
        private BlackboardKey _coyoteTimeCounterKey = default;
        private Actor _actor;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _actor = actor;
            _coyoteTimeCounterKey = _actor.GetOrRegisterKey("CoyoteTimeCounter");
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit()
        {
            _actor.SetValue(_coyoteTimeCounterKey, 0f);
        }

        bool ICustomCondition.Evaluate()
        {
            return _actor.GetValue<float>(_coyoteTimeCounterKey) > 0;
        }
    }
}
