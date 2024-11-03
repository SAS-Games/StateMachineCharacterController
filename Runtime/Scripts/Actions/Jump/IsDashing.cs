using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;
using System.Diagnostics;

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
            UnityEngine.Debug.Log(_actor.GetValue<bool>(_isDashingKey));
            return _actor.GetValue<bool>(_isDashingKey);
        }
    }
}
