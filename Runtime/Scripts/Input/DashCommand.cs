using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SAS.StateMachineCharacterController
{
    public class DashCommand : IInputCommand
    {
        private readonly FSMCharacterController _controller;
        private Action<CallbackContext> _dashPerformed;
        private InputAction _inputAction;

        public DashCommand(FSMCharacterController controller)
        {
            _controller = controller;
            _dashPerformed = _ => _controller.OnDashInitiated();
        }

        public void Enable(InputConfig inputConfig)
        {
            _inputAction = inputConfig.GetInputAction("Dash");
            _inputAction.performed += _dashPerformed;
            _inputAction.Enable();
        }

        public void Disable(InputConfig inputConfig)
        {
            if (_inputAction != null)
            {
                _inputAction.performed -= _dashPerformed;
                _inputAction.Disable();
            }
        }
    }
}
