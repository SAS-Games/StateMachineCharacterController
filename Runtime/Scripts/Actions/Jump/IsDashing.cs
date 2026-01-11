using SAS.StateMachineGraph;
using SAS.Core.BlackboardSystem;
using SAS.Core.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class IsDashing : ICustomCondition
    {
        private BlackboardKey _isDashingKey = default;
        private Actor _actor;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            _actor = actor;
            _isDashingKey = actor.GetOrRegisterKey(FSMCharacterBlackboardKey.IsDashing);
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit() { }

        bool ICustomCondition.Evaluate()
        {
            return _actor.GetValue<bool>(_isDashingKey);
        }
    }
}
