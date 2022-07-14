using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class AnimatorMoveSpeed : IStateAction
	{
		[FieldRequiresChild] private Animator _animator;
		[FieldRequiresChild] private FSMCharacterController _characterController;
		private int _parameterHash;

		void IStateAction.OnInitialize(Actor actor, string tag, string key)
		{
			actor.Initialize(this);
			_parameterHash = Animator.StringToHash("NormalizedSpeed");
		}

		void IStateAction.Execute(ActionExecuteEvent executeEvent)
		{
			float normalisedSpeed = _characterController.NormalizedMoveInput;
			_animator.SetFloat(_parameterHash, normalisedSpeed);
		}
	}
}
