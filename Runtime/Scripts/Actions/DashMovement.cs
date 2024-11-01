using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class DashMovement : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;

        private float _dashSpeed;
        private Vector3 _dashDirection = Vector2.zero;
        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(new BlackboardKey(key), out _dashSpeed);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            if (executeEvent == ActionExecuteEvent.OnStateEnter)
            {
                _dashDirection = new Vector3(_fsmCharacterController.movementInput.x, _fsmCharacterController.movementInput.y).normalized;
                if (_dashDirection == Vector3.zero)
                    _dashDirection = _fsmCharacterController.isFacingRight ? Vector3.right : Vector3.left;
                return;
            }

            _fsmCharacterController.movementVector = _dashDirection * _dashSpeed;
        }
    }
}
