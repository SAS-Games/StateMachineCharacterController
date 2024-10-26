using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using SAS.Utilities.BlackboardSystem;


namespace SAS.StateMachineCharacterController
{
    public class VerticalPull : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _characterController;
        private float _verticalPull;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            _characterController.TryGet(new BlackboardKey(key), out _verticalPull);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.movementVector.y = _verticalPull;
        }
    }
}
