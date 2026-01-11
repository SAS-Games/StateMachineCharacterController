using SAS.StateMachineGraph;
using SAS.Core.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class AnimatorMoveSpeed : IStateAction
    {
        [FieldRequiresChild] private Animator _animator;
        [FieldRequiresChild] private FSMCharacterController _characterController;
        private int _parameterHash;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _parameterHash = Animator.StringToHash(key);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _animator.SetFloat(_parameterHash, _characterController.Speed);
        }
    }
}
