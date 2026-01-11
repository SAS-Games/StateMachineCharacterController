using SAS.StateMachineGraph;
using SAS.Core.TagSystem;
using SAS.Core.BlackboardSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class HorizontalMovementSideWays : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private float _speed = default;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(new BlackboardKey(key), out _speed);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _fsmCharacterController.movementVector.x = _speed * Sign(_fsmCharacterController.movementInput.x);
        }

        private float Sign(float val)
        {
            if (val > -0.01f && val < 0.01f)
                return 0;
            return Mathf.Sign(val);
        }
    }
}