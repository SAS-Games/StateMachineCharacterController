using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class ApplyMovementVector : IStateAction
    {
        private FSMCharacterController _fsmCharacterController;
        private CharacterController _characterController;
        void IStateAction.OnInitialize(Actor actor, string tag, string key)
        {
            actor.TryGetComponent(out _fsmCharacterController);
            actor.TryGetComponent(out _characterController);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.Move(_fsmCharacterController.movementVector * Time.deltaTime);
            _fsmCharacterController.movementVector = _characterController.velocity;
        }
    }
}
