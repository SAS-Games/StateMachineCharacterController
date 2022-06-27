using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class GroundDetector : IStateAction
    {
        private FSMCharacterController _fsmCharacterController;
        private CharacterController _characterController;

        void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
        {
            actor.TryGetComponent(out _fsmCharacterController);
            actor.TryGetComponent(out _characterController);
        }

        void IStateAction.Execute()
        {
            _fsmCharacterController.IsGrounded = _characterController.isGrounded;
        }
    }
}
