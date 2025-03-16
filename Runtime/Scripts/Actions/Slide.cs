using SAS.StateMachineGraph;
using SAS.Utilities.BlackboardSystem;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class Slide : IStateAction
    {
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;
        private float _slideSpeed;
        private float _gravity;

        public void OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
            actor.TryGet(new BlackboardKey(key), out _slideSpeed);
            actor.TryGet(new BlackboardKey(FSMCharacterBlackboardKey.Gravity), out _gravity);
        }

        public void Execute(ActionExecuteEvent executeEvent)
        {
            float speed = -_gravity * _slideSpeed;
            Vector3 hitNormal = _fsmCharacterController.LastHit.normal;
            Vector3 slideDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
            Vector3.OrthoNormalize(ref hitNormal, ref slideDirection);

            _fsmCharacterController.movementVector = slideDirection * speed;
        }
    }
}
