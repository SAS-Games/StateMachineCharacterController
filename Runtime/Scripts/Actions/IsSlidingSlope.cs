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
            float sphereCastVerticalOffset = _characterController.height * 0.5f - _characterController.radius;
            Vector3 origin = _characterTransform.position - Vector3.down * sphereCastVerticalOffset;

            const float sphereCastRadiusOffset = 0.01f;
            const float sphereCastDistance = 0.05f;

            if (Physics.SphereCast(origin, _characterController.radius - sphereCastRadiusOffset, Vector3.down, out RaycastHit hit, sphereCastDistance, ignoreLayer, QueryTriggerInteraction.Ignore))
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                return angle > _characterController.slopeLimit;
            }

            return false;
        }
    }
}
