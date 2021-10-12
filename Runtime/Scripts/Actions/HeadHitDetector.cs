using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class HeadHitDetector : IStateAction
    {
        private FSMCharacterController _characterController;
        private CustomRaycast _raycast;
        private Collider _bodyCollider;

        void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
        {
            actor.TryGetComponent(out _characterController);
            actor.TryGetComponentInChildren(out _bodyCollider, tag);
            actor.TryGet(out _raycast, key);
        }

        void IStateAction.Execute(Actor actor)
        {
            if (_raycast.Raycast(_bodyCollider.bounds.center, _bodyCollider.bounds.extents.y))
            {
                actor.SetTrigger("HasHitHead");
            }
        }
    }
}
