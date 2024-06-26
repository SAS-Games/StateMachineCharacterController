using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class UpwardMovement : IStateAction
	{
		private FSMCharacterController _characterController;
		private UpwardMovementConfig _upwardMovementConfig = default;

		private float _gravityContributionMultiplier;
		private float _verticalMovement;

		void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
		{
			actor.TryGet(out _upwardMovementConfig, key);
			actor.TryGetComponent(out _characterController);
		}

		void IStateAction.Execute(ActionExecuteEvent executeEvent)
		{
			if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
				_gravityContributionMultiplier = 0;
				_verticalMovement = _upwardMovementConfig.jumpForce;
				return;
            }
			_gravityContributionMultiplier += _upwardMovementConfig.gravityComebackMultiplier;
			_gravityContributionMultiplier *= _upwardMovementConfig.gravityDivider; //Reduce the gravity effect
			_verticalMovement += Physics.gravity.y * _upwardMovementConfig.gravityMultiplier * Time.deltaTime * _gravityContributionMultiplier;
			
			_characterController.movementVector.y = _verticalMovement;
		}
	}
}
