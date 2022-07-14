using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class ApplyDrag : IStateAction
    {
        private ScriptableFloat _drag;
        private Rigidbody _rigidbody;

        void IStateAction.OnInitialize(Actor actor, string tag, string key)
        {
            actor.TryGetComponent(out _rigidbody);
            actor.TryGet(out _drag, key);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _rigidbody.drag = _drag.value;
        }
    }
}

