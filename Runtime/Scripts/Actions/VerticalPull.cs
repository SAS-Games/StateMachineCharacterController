using SAS.ScriptableTypes;
using SAS.StateMachineGraph;

namespace SAS.StateMachineCharacterController
{
	public class VerticalPull : IStateAction
	{
		private  FSMCharacterController _characterController;
		private ScriptableFloat _verticalPull;

        void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
        {
			actor.TryGet(out _verticalPull, key);
			actor.TryGetComponent(out _characterController);
		}

        void IStateAction.Execute(Actor actor)
        {
			_characterController.movementVector.y = _verticalPull.runtimeValue;
		}
    }
}
