using SAS.StateMachineGraph;
using SAS.Core.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class SnapToPlatformAction : IStateAction
    {
        [SerializeField] private float _positionOffset = 0.1f;
        [FieldRequiresSelf] FSMCharacterController _fsmCharacterController;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.Initialize(this);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            // Calculate final position with offset
            Vector3 targetPos = _fsmCharacterController.transform.position + Vector3.forward * _positionOffset;

            // Apply position
            _fsmCharacterController.transform.position = targetPos;
        }
    }
}