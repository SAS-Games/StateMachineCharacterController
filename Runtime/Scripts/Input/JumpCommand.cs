using System;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace SAS.StateMachineCharacterController
{
    public class JumpCommand : IInputCommand
    {
        private readonly FSMCharacterController _controller;
        private Action<CallbackContext> _jumpPerformed;
        private Action<CallbackContext> _jumpCanceled;
        private InputAction _inputAction;

        public JumpCommand(FSMCharacterController controller)
        {
            _controller = controller;
            _jumpPerformed = _ => _controller.OnJumpInitiated();
            _jumpCanceled = _ => _controller.OnJumpCanceled();
        }

        public void Enable(InputConfig inputConfig)
        {
            _inputAction = inputConfig.GetInputAction("Jump");
            _inputAction.performed += _jumpPerformed;
            _inputAction.canceled += _jumpCanceled;
            _inputAction.Enable();
        }

        public void Disable(InputConfig inputConfig)
        {
            if (_inputAction != null)
            {
                _inputAction.performed -= _jumpPerformed;
                _inputAction.canceled -= _jumpCanceled;
                _inputAction.Disable();
            }
        }
    }
}
