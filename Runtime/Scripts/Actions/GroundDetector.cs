﻿using SAS.StateMachineGraph;
using SAS.Utilities.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class GroundDetector : IStateAction
    {
        private FSMCharacterController _fsmCharacterController;
        private CharacterController _characterController;

        void IStateAction.OnInitialize(Actor actor, Tag tag, string key)
        {
            actor.TryGetComponent(out _fsmCharacterController);
            actor.TryGetComponent(out _characterController);
        }

        void IStateAction.Execute(ActionExecuteEvent executeEvent)
        {
            _fsmCharacterController.IsGrounded = _characterController.isGrounded;
        }
    }
}
