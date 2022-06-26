using SAS.StateMachineGraph;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
	public class StopMovement : IStateAction
	{
		private FSMCharacterController _characterController;
        void IStateAction.OnInitialize(Actor actor, string tag, string key, State state)
        {
			actor.TryGetComponent(out _characterController);
        }

        void IStateAction.Execute()
        {
			_characterController.movementVector = Vector3.zero;
		}
    }
}
