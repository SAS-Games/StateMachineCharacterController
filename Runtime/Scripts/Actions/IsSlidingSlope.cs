using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class IsSlidingSlope : ICustomCondition
	{
		[FieldRequiresSelf] private CharacterController _characterController;
		[FieldRequiresSelf] private Transform _characterTransform;
		private int ignoreLayer;

		void ICustomCondition.OnInitialize(Actor actor)
		{
			actor.Initialize(this);
			ignoreLayer = ~LayerMask.GetMask("Player");
		}

		void ICustomCondition.OnStateEnter() { }

		void ICustomCondition.OnStateExit() { }

		bool ICustomCondition.Evaluate()
		{
			var sphereCastVerticalOffset = _characterController.height * 0.5f - _characterController.radius;
			var origin = _characterTransform.position - Vector3.down * sphereCastVerticalOffset;

			if (Physics.SphereCast(origin, _characterController.radius - 0.01f, Vector3.down, out var hit, 0.05f, ignoreLayer, QueryTriggerInteraction.Ignore))
			{
				var angle = Vector3.Angle(Vector3.up, hit.normal);
				return angle > _characterController.slopeLimit;
			}
			return false;
		}
	}
}
