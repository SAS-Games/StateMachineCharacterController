using SAS.ScriptableTypes;
using SAS.StateMachineGraph;

namespace SAS.StateMachineCharacterController
{
	public class HorizontalMovement : IStateAction
	{
		private FSMCharacterController _characterController;
		private ScriptableFloat _speed = default;

		void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
		{
			actor.TryGetComponent(out _characterController);
			actor.TryGet(out _speed, key);
		}

        void IStateAction.Execute()
        {
			_characterController.movementVector.x = _speed.value * _characterController.movementInput.x;
			_characterController.movementVector.z = _speed.value * _characterController.movementInput.z;
		}
    }
}