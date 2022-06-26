using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class CanSlide : IStateAction
	{
		private CharacterController _characterController;
		private CharacterControllerColliderHit _controllerColliderHit;
		private Actor _actor;

		void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
		{
			_actor = actor;
			actor.TryGetComponent(out _controllerColliderHit);
			actor.TryGetComponent(out _characterController);
		}

        void IStateAction.Execute()
		{
			if (_controllerColliderHit.LastHit == null)
			{
				_actor.SetBool("IsSliding", false);
				return;
			}

			float stepHeight = _controllerColliderHit.LastHit.point.y - _controllerColliderHit.transform.position.y;
			bool isWalkableStep = stepHeight <= _characterController.stepOffset;

			float currentSlope = Vector3.Angle(Vector3.up, _controllerColliderHit.LastHit.normal);
			bool isSlopeTooSteep = currentSlope >= _characterController.slopeLimit;

			if (!isSlopeTooSteep)
				_actor.SetBool("IsSliding", false);
			else
				_actor.SetBool("IsSliding", !isWalkableStep);
		}
    }
}
