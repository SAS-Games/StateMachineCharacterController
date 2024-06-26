using SAS.ScriptableTypes;
using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class Slide : IStateAction
	{
		[FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
		[FieldRequiresChild] private CharacterControllerColliderHit _controllerColliderHit;
		private ScriptableReadOnlyFloat _slideSpeed;

		public void OnInitialize(Actor actor, Tag tag, string key)
		{
			actor.Initialize(this);
			actor.TryGet(out _slideSpeed, key);
		}

		public void Execute(ActionExecuteEvent executeEvent)
		{
			float speed = -Physics.gravity.y * _slideSpeed.value;
			Vector3 hitNormal = _controllerColliderHit.LastHit.normal;
			Vector3 slideDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
			Vector3.OrthoNormalize(ref hitNormal, ref slideDirection);

			_fsmCharacterController.movementVector = slideDirection * speed;
		}
	}
}
