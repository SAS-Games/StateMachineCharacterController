using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class DownwardMovement : IStateAction
	{
		private FSMCharacterController _characterController;
		private DownwardMovementConfig _downwardMovementConfig = default;
		private float _verticalMovement;

		void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
		{
			actor.TryGet(out _downwardMovementConfig, key);
			actor.TryGetComponent(out _characterController);
			state.OnEnterEvent += () =>
			{
				_verticalMovement = _characterController.movementVector.y;
			};
		}

		void IStateAction.Execute()
		{
			_verticalMovement += Physics.gravity.y * _downwardMovementConfig.gravityMultiplier * Time.deltaTime;
			_verticalMovement = Mathf.Clamp(_verticalMovement, _downwardMovementConfig.fallSpeedRange.min, _downwardMovementConfig.fallSpeedRange.max);

			_characterController.movementVector.y = _verticalMovement;
		}
	}
}
