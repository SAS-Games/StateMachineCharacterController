using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class HeadHitDetector : ICustomCondition
    {
        [FieldRequiresChild] private Collider _bodyCollider;
        private CustomRaycast _raycast;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            actor.Initialize(this);
            actor.TryGet(out _raycast);
        }

        void ICustomCondition.OnStateEnter() { }

        void ICustomCondition.OnStateExit() { }

        bool ICustomCondition.Evaluate()
        {
            if (_raycast.Raycast(_bodyCollider.bounds.center, _bodyCollider.bounds.extents.y))
                return true;

            return false;
        }
    }
}
