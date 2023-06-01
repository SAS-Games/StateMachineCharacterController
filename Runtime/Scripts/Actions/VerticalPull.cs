using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
	public class VerticalPull : IStateAction
	{
		[FieldRequiresSelf] private FSMCharacterController _characterController;
		private ScriptableReadOnlyFloat _verticalPull;

		void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
		{
			actor.Initialize(this);
			actor.TryGet(out _verticalPull, key);
		}

		void IStateAction.Execute(ActionExecuteEvent executeEvent)
		{
			_characterController.movementVector.y = _verticalPull.value;
		}
	}
}
