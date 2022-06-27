using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class HeadHitDetector : ICustomCondition
    {
        [FieldRequiresChild] private FSMCharacterController _characterController;
        [FieldRequiresChild] private Collider _bodyCollider;
        private CustomRaycast _raycast;
        private Actor _actor;

        void ICustomCondition.OnInitialize(Actor actor)
        {
            _actor = actor;
            _actor.Initialize(this);
            actor.TryGet(out _raycast, "HeadHit");
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
