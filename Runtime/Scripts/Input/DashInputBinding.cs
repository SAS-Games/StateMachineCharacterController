using SAS.Core.TagSystem;
using UnityEngine;

namespace SAS.StateMachineCharacterController
{
    public class DashInputBinding : MonoBehaviour
    {
        [FieldRequiresSelf] InputHandler _inputHandler;
        [FieldRequiresSelf] private FSMCharacterController _fsmCharacterController;

        void Awake()
        {
            this.Initialize();
        }

        private void Start()
        {
            _inputHandler.RegisterInputCommand("Dash", new DashCommand(_fsmCharacterController), true);
        }
    }
}