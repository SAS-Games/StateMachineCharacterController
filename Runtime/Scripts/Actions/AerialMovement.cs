using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class AerialMovement : IStateAction
	{
		private AerialMovementConfig _aerialMovementConfig;
		private FSMCharacterController _fsmCharacterController;

		void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
		{
			actor.TryGetComponent(out _fsmCharacterController);
			actor.TryGet(out _aerialMovementConfig);
		}

		void IStateAction.Execute(Actor actor)
		{
			Vector3 velocity = _fsmCharacterController.movementVector;
			Vector3 input = _fsmCharacterController.movementInput;

			SetVelocityPerAxis(ref velocity.x, input.x, _aerialMovementConfig.acceleration, _aerialMovementConfig.speed);
			SetVelocityPerAxis(ref velocity.z, input.z, _aerialMovementConfig.acceleration, _aerialMovementConfig.speed);

			_fsmCharacterController.movementVector = velocity;
		}

		private void SetVelocityPerAxis(ref float currentAxisSpeed, float axisInput, float acceleration, float targetSpeed)
		{
			if (axisInput == 0f)
			{
				if (currentAxisSpeed != 0f)
					ApplyAirResistance(ref currentAxisSpeed);
			}
			else
			{
				(float absVel, float absInput) = (Mathf.Abs(currentAxisSpeed), Mathf.Abs(axisInput));
				(float signVel, float signInput) = (Mathf.Sign(currentAxisSpeed), Mathf.Sign(axisInput));
				targetSpeed *= absInput;

				if (signVel != signInput || absVel < targetSpeed)
				{
					currentAxisSpeed += axisInput * acceleration;
					currentAxisSpeed = Mathf.Clamp(currentAxisSpeed, -targetSpeed, targetSpeed);
				}
				else
				{
					ApplyAirResistance(ref currentAxisSpeed);
				}
			}
		}

		private void ApplyAirResistance(ref float value)
		{
			float sign = Mathf.Sign(value);

			value -= sign * _aerialMovementConfig.airResistance * Time.deltaTime;
			if (Mathf.Sign(value) != sign)
				value = 0;
		}
    }
}