using SAS.StateMachineGraph;
using UnityEngine;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
	public class StopMovement : IStateAction
	{
		[FieldRequiresSelf] private FSMCharacterController _characterController;
        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
			_characterController.movementVector = Vector3.zero;
		}
    }
}
