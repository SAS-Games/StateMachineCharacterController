using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class HeadHitDetector : IStateAction
    {
        private FSMCharacterController _characterController;
        private CustomRaycast _raycast;
        private Collider _bodyCollider;
        private Actor _actor;

        void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
        {
            _actor = actor;
            actor.TryGetComponent(out _characterController);
            actor.TryGetComponentInChildren(out _bodyCollider, tag);
            actor.TryGet(out _raycast, key);
        }

        void IStateAction.Execute()
        {
            if (_raycast.Raycast(_bodyCollider.bounds.center, _bodyCollider.bounds.extents.y))
            {
                _actor.SetTrigger("HasHitHead");
            }
        }
    }
}
