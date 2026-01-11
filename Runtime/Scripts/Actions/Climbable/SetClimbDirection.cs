using SAS.StateMachineGraph;
using SAS.Core.TagSystem;
using SAS.Core.BlackboardSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class SetClimbDirection : IStateAction
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
            _fsmCharacterController.isFacingRight = _fsmCharacterController.IsTouchingLayerSide(Vector3.right, _fsmCharacterController.ClimbableLayer, out var raycastHit);
        }
    }
}