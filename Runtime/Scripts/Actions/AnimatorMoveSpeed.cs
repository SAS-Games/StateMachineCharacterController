using SAS.StateMachineGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class AnimatorMoveSpeed : IStateAction
	{
		private Animator _animator;
		private FSMCharacterController _characterController;
		private int _parameterHash;

		void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
		{
			actor.TryGetComponent(out _animator);
			actor.TryGetComponent(out _characterController);
			_parameterHash = Animator.StringToHash("NormalizedSpeed");
		}

		void IStateAction.Execute()
		{
			float normalisedSpeed = _characterController.NormalizedMoveInput;
			_animator.SetFloat(_parameterHash, normalisedSpeed);
		}
	}
}
