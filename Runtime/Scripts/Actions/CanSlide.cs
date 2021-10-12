using SAS.StateMachineGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class CanSlide : IStateAction
	{
		private CharacterController _characterController;
		private CharacterControllerColliderHit _controllerColliderHit;

		void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
		{
			actor.TryGetComponent(out _controllerColliderHit);
			actor.TryGetComponent(out _characterController);
		}

        void IStateAction.Execute(Actor actor)
		{
			if (_controllerColliderHit.LastHit == null)
			{
				actor.SetBool("IsSliding", false);
				return;
			}

			float stepHeight = _controllerColliderHit.LastHit.point.y - _controllerColliderHit.transform.position.y;
			bool isWalkableStep = stepHeight <= _characterController.stepOffset;

			float currentSlope = Vector3.Angle(Vector3.up, _controllerColliderHit.LastHit.normal);
			bool isSlopeTooSteep = currentSlope >= _characterController.slopeLimit;

			if (!isSlopeTooSteep)
				actor.SetBool("IsSliding", false);
			else
				actor.SetBool("IsSliding", !isWalkableStep);
		}
    }
}
