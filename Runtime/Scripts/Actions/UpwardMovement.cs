using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class UpwardMovement : IStateAction
	{
		private FSMCharacterController _characterController;
		private UpwardMovementConfig _upwardMovementConfig = default;

		private float _gravityContributionMultiplier;
		private float _verticalMovement;

		void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
		{
			actor.TryGet(out _upwardMovementConfig, key);
			actor.TryGetComponent(out _characterController);
			actor.OnStateEnter += _ =>
			{
				if (state == actor.CurrentState)
				{
					_gravityContributionMultiplier = 0;
					_verticalMovement = _upwardMovementConfig.jumpForce;
				}
			};
		}

		void IStateAction.Execute(Actor actor)
		{
			_gravityContributionMultiplier += _upwardMovementConfig.gravityComebackMultiplier;
			_gravityContributionMultiplier *= _upwardMovementConfig.gravityDivider; //Reduce the gravity effect
			_verticalMovement += Physics.gravity.y * _upwardMovementConfig.gravityMultiplier * Time.deltaTime * _gravityContributionMultiplier;
			
			_characterController.movementVector.y = _verticalMovement;
		}
	}
}
