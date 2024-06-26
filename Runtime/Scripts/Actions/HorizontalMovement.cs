using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
	public class HorizontalMovement : IStateAction
	{
		[FieldRequiresSelf] private FSMCharacterController _characterController;
		private ScriptableReadOnlyFloat _speed = default;

		void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
		{
			actor.Initialize(this);
			actor.TryGet(out _speed, key);
		}

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
			_characterController.movementVector.x = _speed.value * _characterController.movementInput.x;
			_characterController.movementVector.z = _speed.value * _characterController.movementInput.z;
		}
    }
}