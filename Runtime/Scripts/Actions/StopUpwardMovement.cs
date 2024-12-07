using SAS.StateMachineGraph;
using UnityEngine;
using SAS.Utilities.TagSystem;

namespace SAS.StateMachineCharacterController
{
	public class StopUpwardMovement : IStateAction
	{
		[FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
			_fsmCharacterController.movementVector.y = 0;
		}
    }
}
