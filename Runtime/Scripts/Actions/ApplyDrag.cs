using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class ApplyDrag : IStateAction
    {
        private ScriptableFloat _drag;
        private Rigidbody _rigidbody;

        void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
        {
            actor.TryGetComponent(out _rigidbody);
            actor.TryGet(out _drag, key);
        }

        void IStateAction.Execute(Actor actor)
        {
            _rigidbody.drag = _drag.runtimeValue;
        }
    }
}

