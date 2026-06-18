using SAS.Core.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class JumpInputBinding : MonoBehaviour
    {
        [FieldRequiresSelf] InputHandler _inputHandler; 
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;

        void Awake()
        {
            this.Initialize();
        }

        private void Start()
        {
            _inputHandler.RegisterInputCommand("Jump", new JumpCommand(_fsmCharacterController), true);
        }
    }
}