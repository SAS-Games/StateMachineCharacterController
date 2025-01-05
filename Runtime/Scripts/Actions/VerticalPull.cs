using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;


namespace SAS.StateMachineCharacterController
{
    public class VerticalPull : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _characterController;
        private float _verticalPull;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(new BlackboardKey(FSMCharacterBlackboardKey.Gravity), out _verticalPull);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _characterController.movementVector.y = _verticalPull;
        }
    }
}
