using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class Slide : IStateAction
	{
		private FSMCharacterController _fsmCharacterController;
		private CharacterControllerColliderHit _controllerColliderHit;
		private ScriptableFloat _slideSpeed;

		public void OnInitialize(Actor actor, string tag, string key, State state)
		{
			actor.TryGetComponent(out _fsmCharacterController);
			actor.TryGetComponent(out _controllerColliderHit);
			actor.TryGet(out _slideSpeed, key);
		}

		public void Execute(Actor actor)
		{
			float speed = -Physics.gravity.y * _slideSpeed.runtimeValue;
			Vector3 hitNormal = _controllerColliderHit.LastHit.normal;
			Vector3 slideDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
			Vector3.OrthoNormalize(ref hitNormal, ref slideDirection);

			_fsmCharacterController.movementVector = slideDirection * speed;
		}
	}
}
