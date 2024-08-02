using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class AnimatorVerticalSpeed : IStateAction
    {
        [FieldRequiresChild] private Animator _animator;
        [FieldRequiresChild] private FSMCharacterController _characterController;
        private UpwardMovementConfig _upwardMovementConfig;
        private int _parameterHash;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _parameterHash = Animator.StringToHash(key);
            actor.TryGet(out _upwardMovementConfig);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var jumpSpeed = _upwardMovementConfig.jumpForce;
            float normalisedSpeed = _characterController.VerticalVelocity.y.Remap(-jumpSpeed, jumpSpeed, -1.0f, 1.0f); ;
            _animator.SetFloat(_parameterHash, normalisedSpeed);
        }
    }
}
