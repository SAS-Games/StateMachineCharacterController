using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class CanSlide : ICustomCondition
	{
		private CharacterController _characterController;
		private CharacterControllerColliderHit _controllerColliderHit;
		private Actor _actor;

		void ICustomCondition.OnInitialize(Actor actor)
		{
			_actor = actor;
			actor.TryGetComponent(out _controllerColliderHit);
			actor.TryGetComponent(out _characterController);
		}

        void ICustomCondition.OnStateEnter()
        {
        }

        void ICustomCondition.OnStateExit()
        {
        }

        bool ICustomCondition.Evaluate()
        {
			if (_controllerColliderHit.LastHit == null)
				return false;

			float stepHeight = _controllerColliderHit.LastHit.point.y - _controllerColliderHit.transform.position.y;
			bool isWalkableStep = stepHeight <= _characterController.stepOffset;

			float currentSlope = Vector3.Angle(Vector3.up, _controllerColliderHit.LastHit.normal);
			bool isSlopeTooSteep = currentSlope >= _characterController.slopeLimit;

			if (!isSlopeTooSteep)
				return false;
			else
				return !isWalkableStep;
		}
    }
}
