using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
	public class VerticalPull : IStateAction
	{
		[FieldRequiresSelf] private FSMCharacterController _characterController;
		private ScriptableReadOnlyFloat _verticalPull;

		void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
		{
			actor.Initialize(this);
			actor.TryGet(out _verticalPull, key);
		}

		void IStateAction.Execute()
		{
			_characterController.movementVector.y = _verticalPull.value;
		}
	}
}
