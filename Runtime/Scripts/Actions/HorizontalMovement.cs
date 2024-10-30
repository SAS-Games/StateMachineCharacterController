using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using SAS.Utilities.BlackboardSystem;

namespace SAS.StateMachineCharacterController
{
    public class HorizontalMovement : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _characterController;
        private float _speed = default;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _characterController.TryGet(new BlackboardKey(key), out _speed);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.movementVector.x = _speed * _characterController.movementInput.x;
            _characterController.movementVector.z = _speed * _characterController.movementInput.z;
        }
    }
}