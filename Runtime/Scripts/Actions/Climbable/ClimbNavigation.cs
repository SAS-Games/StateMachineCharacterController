using SAS.StateMachineGraph;
using SAS.Core.BlackboardSystem;
using SAS.Core.TagSystem;

namespace SAS.StateMachineCharacterController
{
    public class ClimbNavigation : IStateAction
    {
        private FSMCharacterController _fsmCharacterController;
        private float _speed = default;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.TryGetComponent(out _fsmCharacterController);
            actor.TryGet(new BlackboardKey(key), out _speed);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            var verticalMovement = _fsmCharacterController.movementVector;
            verticalMovement.y = _speed * _fsmCharacterController.movementInput.y;
            _fsmCharacterController.movementVector = verticalMovement;
        }
    }
}
