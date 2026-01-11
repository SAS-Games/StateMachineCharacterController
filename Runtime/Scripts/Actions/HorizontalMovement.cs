using SAS.StateMachineGraph;
using SAS.Core.TagSystem;
using SAS.Core.BlackboardSystem;

namespace SAS.StateMachineCharacterController
{
    public class HorizontalMovement : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private float _speed = default;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(new BlackboardKey(key), out _speed);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _fsmCharacterController.movementVector.x = _speed * _fsmCharacterController.movementInput.x;
            _fsmCharacterController.movementVector.z = _speed * _fsmCharacterController.movementInput.z;
        }
    }
}